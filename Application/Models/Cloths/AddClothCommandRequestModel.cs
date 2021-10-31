using Application.Common.Base.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Models.Cloths
{
    public class AddClothCommandRequestModel
    {
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public IFormFile Image { get; set; }
    }
}