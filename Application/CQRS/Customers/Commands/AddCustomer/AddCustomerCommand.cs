using Application.Common.Base;
using Application.Common.Base.Models;
using Application.Models.Customers;
using Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Customers.Commands.AddCustomer
{
    public class AddCustomerCommand : BaseRequest<int>
    {
        public AddCustomerCommandRequestModel Model { get; set; }
        public AddCustomerCommand(AddCustomerCommandRequestModel model) => Model = model;
        public class AddCustomerCommandHandler : IBaseRequestHandler<AddCustomerCommand, int>
        {
            public async Task<CQRSResponse<int>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
            {
                CQRSResponse<int> response = new(200)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "The Customer successfully added!"
                };
                AddCustomerCommandRequestModel model = request.Model;

                Customer customer = new()
                {
                    Name = model.Name,
                    Phone = model.Phone
                };

                await request.Context.Customers.AddAsync(customer);
                await request.Context.SaveChangesAsync();

                return response;
            }
        }
    }
}