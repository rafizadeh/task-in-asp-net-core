using Application.Common.Base.Models;
using Domain.DTOs;
using System.Collections.Generic;

namespace Application.Models.Shelves
{
    public class GetShelvesQueryResponseModel
    {
        public List<ShelfDto> Shelves { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}
