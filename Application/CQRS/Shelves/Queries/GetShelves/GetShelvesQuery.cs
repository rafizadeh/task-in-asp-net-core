using Application.Common.Base;
using Application.Common.Base.Models;
using Application.Models.Shelves;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Shelves.Queries.GetShelves
{
    public class GetShelvesQuery : BaseRequest<GetShelvesQueryResponseModel>
    {
        public int Page { get; set; }
        public GetShelvesQuery(int page) => Page = page;
        public class GetShelvesQueryHandler : IBaseRequestHandler<GetShelvesQuery, GetShelvesQueryResponseModel>
        {
            public async Task<CQRSResponse<GetShelvesQueryResponseModel>> Handle(GetShelvesQuery request, CancellationToken cancellationToken)
            {
                int take = 20;
                List<ShelfDto> shelves = await request.Context.Shelves.Skip((request.Page - 1)*take).Take(take).Select(s => new ShelfDto
                {
                    Id = s.Id,
                    ShelfNo = s.ShelfNo,
                    CustomerName = s.Cloth != null ? s.Cloth.Customer.Name : default,
                    ClothName = s.Cloth != null ? s.Cloth.Name : default,
                    ClothImage = s.Cloth != null ? s.Cloth.Image : default,
                }).ToListAsync();

                GetShelvesQueryResponseModel model = new()
                {
                    Shelves = shelves,
                    Pagination = new PaginationModel(await request.Context.Shelves.CountAsync(), take, request.Page)
                };


                return new CQRSResponse<GetShelvesQueryResponseModel>(model)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
        }
    }
}