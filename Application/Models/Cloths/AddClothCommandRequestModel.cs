using Application.Common.Base.Models;

namespace Application.Models.Cloths
{
    public class AddClothCommandRequestModel
    {
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public ImageFile Image { get; set; }
    }
}