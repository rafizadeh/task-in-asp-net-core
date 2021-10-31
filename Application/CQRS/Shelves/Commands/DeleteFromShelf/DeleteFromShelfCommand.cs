using Application.Common.Base;
using Application.Common.Base.Models;
using Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Shelves.Commands.DeleteFromShelf
{
    public class DeleteFromShelfCommand : BaseRequest<int>
    {
        public int ShelfId { get; set; }
        public DeleteFromShelfCommand(int shelfId) => ShelfId = shelfId;
        public class DeleteFromShelfCommandHandler : IBaseRequestHandler<DeleteFromShelfCommand, int>
        {
            public async Task<CQRSResponse<int>> Handle(DeleteFromShelfCommand request, CancellationToken cancellationToken)
            {
                CQRSResponse<int> response = new(200)
                {
                    Message = "Cloth successfully taken off!",
                    StatusCode = (int)HttpStatusCode.OK
                };

                Shelf shelf = await request.Context.Shelves.FindAsync(request.ShelfId);

                if (shelf is null)
                {
                    response.Message = "Shelf is not found.";
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }

                shelf.ClothId = null;
                await request.Context.SaveChangesAsync();

                return response;
            }
        }
    }
}