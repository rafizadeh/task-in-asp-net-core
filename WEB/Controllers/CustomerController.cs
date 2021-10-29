using Application.CQRS.Customers.Queries.GetCustomers;
using Application.Models.Customers;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WEB.Controllers
{
    public class CustomerController : BaseController
    {
        [HttpGet("customer/list")]
        public async Task<IActionResult> List()
        {
            List<CustomerDto> customers = (await Mediator.Send(new GetCustomersQuery())).Response;

            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerCommandRequestModel model)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }
    }
}