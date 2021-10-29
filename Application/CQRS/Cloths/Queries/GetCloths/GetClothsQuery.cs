using Application.Common.Base;
using Application.Common.Base.Models;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Cloths.Queries.GetCloths
{
    public class GetClothsQuery : BaseRequest<List<ClothDto>>
    {
        public int CustomerId { get; set; }
        public GetClothsQuery(int customerId) => CustomerId = customerId;
        public class GetClothQueryHandler : IBaseRequestHandler<GetClothsQuery, List<ClothDto>>
        {
            public async Task<CQRSResponse<List<ClothDto>>> Handle(GetClothsQuery request, CancellationToken cancellationToken)
            {
                if(request.CustomerId == 0)
                {
                    List<ClothDto> customers = await request.Context.Cloths.Select(c => new ClothDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Image = c.Image
                    }).ToListAsync(cancellationToken: cancellationToken);

                    return new CQRSResponse<List<ClothDto>>(customers);
                }
                else
                {
                    List<ClothDto> customers = await request.Context.Cloths.Select(c => new ClothDto
                    {
                        Name = c.Name,
                    }).ToListAsync(cancellationToken: cancellationToken);

                    return new CQRSResponse<List<ClothDto>>(customers);
                }
            }
        }
    }
}