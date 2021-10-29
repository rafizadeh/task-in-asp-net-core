using Application.Common.Base;
using Application.Common.Base.Models;
using Application.Models.Cloths;
using Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Cloths.Commands.AddCloth
{
    public class AddClothCommand : BaseRequest<int>
    {
        public AddClothCommandRequestModel Model { get; set; }
        public AddClothCommand(AddClothCommandRequestModel model) => Model = model;
        public class AddClothCommandHandler : IBaseRequestHandler<AddClothCommand, int>
        {
            public async Task<CQRSResponse<int>> Handle(AddClothCommand request, CancellationToken cancellationToken)
            {
                CQRSResponse<int> response = new(200)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "The Customer successfully added!"
                };
                AddClothCommandRequestModel model = request.Model;

                Cloth cloth = new()
                {
                    Name = model.Name,
                    Image = model.Image.File.FileName,
                    CustomerId = model.CustomerId
                };

                await request.Context.Cloths.AddAsync(cloth, cancellationToken);
                await request.Context.SaveChangesAsync();

                return response;
            }
        }
    }
}
