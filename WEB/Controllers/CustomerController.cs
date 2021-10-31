using Application.Common.Base.Models;
using Application.CQRS.Customers.Commands.AddCustomer;
using Application.CQRS.Customers.Queries.GetCustomers;
using Application.Models.Customers;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Threading.Tasks;
using WEB.ViewModels;

namespace WEB.Controllers
{
    public class CustomerController : BaseController
    {
        [HttpGet("customer/list")]
        public async Task<IActionResult> List()
        {
            CustomerViewModel model = new()
            {
                Customers = (await Mediator.Send(new GetCustomersQuery())).Response
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerCommandRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AddConfirmation"] = "false";
                return View();
            }

            CQRSResponse<int> response = await Mediator.Send(new AddCustomerCommand(model));
            TempData["AddConfirmation"] = "true";
            return RedirectToAction("List","Customer");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int id)
        {
            if(id == 0)
                return Json(new { status = 500 , message = "Internal Server error!"});

            CQRSResponse<int> response = await Mediator.Send(new DeleteCustomerCommand(id));

            if (response == null)
                return Json(new { status = 500, message = "Internal Server error!" });

            return Json(response);
        }
    }
}