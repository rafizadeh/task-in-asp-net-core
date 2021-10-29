using Application.Common.Base;
using Application.Common.Base.Models;
using Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Customers.Commands.AddCustomer
{
    public class DeleteCustomerCommand : BaseRequest<int>
    {
        public int Id { get; set; }
        public DeleteCustomerCommand(int id) => Id = id;
        public class DeleteCustomerCommandHandler : IBaseRequestHandler<DeleteCustomerCommand, int>
        {
            public async Task<CQRSResponse<int>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
            {
                CQRSResponse<int> response = new(200)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "The Customer successfully deleted!"
                };

                Customer deletedCustomer = await request.Context.Customers.FindAsync(request.Id);

                if(deletedCustomer is null)
                {
                    return new CQRSResponse<int>(404)
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "There is not such kind of Customer record!"
                    };
                }

                request.Context.Customers.Remove(deletedCustomer);
                await request.Context.SaveChangesAsync();
                
                return response;
            }
        }
    }
}