using Application.Models.Shelves;
using Domain.DTOs;
using System.Collections.Generic;

namespace WEB.ViewModels
{
    public class ShelfViewModel
    {
        public List<ClothDto> Cloths { get; set; }
        public List<CustomerDto> Customers { get; set; }
        public GetShelvesQueryResponseModel ShelfModel { get; set; }
    }
}