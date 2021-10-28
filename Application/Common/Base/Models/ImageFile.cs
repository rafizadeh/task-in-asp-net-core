using Microsoft.AspNetCore.Http;

namespace Application.Common.Base.Models
{
    public class ImageFile
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
    }
}