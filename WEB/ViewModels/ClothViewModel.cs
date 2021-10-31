using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB.ViewModels
{
    public class ClothViewModel
    {
        [Required(ErrorMessage ="Item name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Customer has to be selected!")]
        public int CustomerId { get; set; }
        public IFormFile Image { get; set; }


        public List<CustomerDto> Customers { get; set; }
        public List<ClothDto> Cloths { get; set; }
    }
}