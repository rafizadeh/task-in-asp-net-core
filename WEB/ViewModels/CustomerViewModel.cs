using Domain.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB.ViewModels
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage ="Name is required!")]
        public string Name { get; set; }
        [StringLength(15,ErrorMessage ="Phone number cannot be more than 15 characters.")]
        public string Phone { get; set; }

        public List<CustomerDto> Customers { get; set; }
    }
}