using Application.Common.Base;
using Application.Common.Base.Models;
using Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Cloths.Commands.DeleteCloth
{
    public class DeleteClothCommand : BaseRequest<int>
    {
        public int Id { get; set; }
        public DeleteClothCommand(int id) => Id = id;
        public class DeleteClothCommandHandler : IBaseRequestHandler<DeleteClothCommand, int>
        {
            public async Task<CQRSResponse<int>> Handle(DeleteClothCommand request, CancellationToken cancellationToken)
            {
                CQRSResponse<int> response = new(200)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "The Cloth successfully deleted!"
                };

                Cloth deletedCloth = await request.Context.Cloths.FindAsync(request.Id);

                if (deletedCloth is null)
                {
                    return new CQRSResponse<int>(404)
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "There is not such kind of Cloth record!"
                    };
                }

                request.Context.Cloths.Remove(deletedCloth);
                await request.Context.SaveChangesAsync();

                return response;
            }
        }
    }
}