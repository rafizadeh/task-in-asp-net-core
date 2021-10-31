using Application.Common.Base;
using Application.Common.Base.Models;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Shelves.Commands.AddToShelf
{
    public class AddToShelfCommand : BaseRequest<int>
    {
        public int ClothId { get; set; }
        public AddToShelfCommand(int clothId) => ClothId = clothId;
        public class AddToShelfCommandHandler : IBaseRequestHandler<AddToShelfCommand, int>
        {
            public async Task<CQRSResponse<int>> Handle(AddToShelfCommand request, CancellationToken cancellationToken)
            {
                CQRSResponse<int> response = new(200)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Cloth has been successfully added to shelf!"
                };

                Cloth cloth = await request.Context.Cloths.FindAsync(request.ClothId);

                if (cloth is null)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Cloth is not found!";
                    return response;
                }
                Customer customer = cloth.Customer;

                if (customer.Clothes.Count > 1)
                {
                    //int firstShelfId = 0;
                    //int lastShelfId = 0;

                    List<RecommendedShelf> recommendedShelves = new();

                    foreach (Cloth anotherCloth in customer.Clothes.Except(new List<Cloth> { cloth }))
                    {
                        Shelf busyShelf = await request.Context.Shelves.FirstOrDefaultAsync(s => s.ClothId == anotherCloth.Id);

                        if (busyShelf != null)
                            recommendedShelves.Add(await GetNearestShelf(request.Context, busyShelf.Id));

                        //if (busyShelf != null)
                        //{
                        //    if(firstShelfId == 0) firstShelfId = busyShelf.Id;
                        //    if (busyShelf.Id < firstShelfId) firstShelfId = busyShelf.Id;

                        //    #region old
                        //    if (busyShelf.Id > lastShelfId) lastShelfId = busyShelf.Id;
                        //    #endregion
                        //}
                    }

                    if (recommendedShelves.Count > 0)
                    {
                        int leastDistance = recommendedShelves.Min(rs => rs.Distance);

                        RecommendedShelf recommendedShelf = recommendedShelves.OrderBy(rs => rs.Shelf.Id).FirstOrDefault(rs => rs.Distance == leastDistance);
                        recommendedShelf.Shelf.ClothId = request.ClothId;
                        await request.Context.SaveChangesAsync();
                    }
                    else
                    {
                        Shelf shelf = await request.Context.Shelves.Where(s => s.ClothId == null).FirstOrDefaultAsync();
                        shelf.ClothId = request.ClothId;
                        await request.Context.SaveChangesAsync();
                    }

                    //other cloths are not in the shelf
                    //if (firstShelfId == 0 && lastShelfId == 0)
                    //{
                    //    Shelf shelf1 = await request.Context.Shelves.FirstOrDefaultAsync(s => s.ClothId == null);
                    //    shelf1.ClothId = request.ClothId;
                    //    await request.Context.SaveChangesAsync();

                    //    return response;
                    //}

                    //#region old
                    //Shelf shelf = await request.Context.Shelves.FirstOrDefaultAsync(s => (lastShelfId == 0 || s.Id > lastShelfId) && s.ClothId == null);
                    //shelf.ClothId = request.ClothId;
                    //await request.Context.SaveChangesAsync();
                    //#endregion
                }
                else
                {
                    Shelf shelf = await request.Context.Shelves.Where(s => s.ClothId == null).FirstOrDefaultAsync();
                    shelf.ClothId = request.ClothId;
                    await request.Context.SaveChangesAsync();
                }

                return response;
            }

            private async Task<RecommendedShelf> GetNearestShelf(IApplicationDbContext context, int toShelfId)
            {
                Shelf nextShelf = await context.Shelves.FirstOrDefaultAsync(s => s.Id > toShelfId && s.ClothId == null);

                Shelf prevShelf = await context.Shelves.FirstOrDefaultAsync(s => s.Id < toShelfId && s.ClothId == null);

                if (prevShelf == null && nextShelf != null) return RecommendedShelf.Instance(nextShelf, nextShelf.Id - toShelfId);
                else if (prevShelf != null && nextShelf == null) return RecommendedShelf.Instance(prevShelf, toShelfId - prevShelf.Id);

                int differentWithNextShelf = nextShelf.Id - toShelfId;

                int differentWithPrevShelf = toShelfId - prevShelf.Id;

                if (differentWithPrevShelf == differentWithNextShelf) return RecommendedShelf.Instance(nextShelf, differentWithNextShelf);

                return differentWithPrevShelf > differentWithNextShelf ? RecommendedShelf.Instance(nextShelf, differentWithNextShelf) : RecommendedShelf.Instance(prevShelf, differentWithPrevShelf);
            }

            class RecommendedShelf
            {
                public RecommendedShelf(Shelf shelf, int distance)
                {
                    Shelf = shelf;
                    Distance = distance;
                }

                public static RecommendedShelf Instance(Shelf shelf, int distance) => new(shelf, distance);

                public Shelf Shelf { get; set; }

                public int Distance { get; set; }
            }
        }
    }
}