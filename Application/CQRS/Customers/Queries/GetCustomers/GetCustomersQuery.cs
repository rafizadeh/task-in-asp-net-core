using Application.Common.Base;
using Application.Common.Base.Models;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Customers.Queries.GetCustomers
{
    public class GetCustomersQuery : BaseRequest<List<CustomerDto>>
    {
        public class GetCustomersQueryHandler : IBaseRequestHandler<GetCustomersQuery, List<CustomerDto>>
        {
            public async Task<CQRSResponse<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
            {
                List<CustomerDto> customers = await request.Context.Customers.Select(c => new CustomerDto 
                { 
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone
                }).ToListAsync(cancellationToken: cancellationToken);

                return new CQRSResponse<List<CustomerDto>>(customers);
            }
        }
    }
}