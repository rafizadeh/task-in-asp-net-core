using Application.Common.Base.Models;
using Application.Common.Extensions;
using Application.CQRS.Cloths.Commands.AddCloth;
using Application.CQRS.Cloths.Commands.DeleteCloth;
using Application.CQRS.Cloths.Queries.GetCloths;
using Application.CQRS.Customers.Queries.GetCustomers;
using Application.Models.Cloths;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Threading.Tasks;
using WEB.ViewModels;

namespace WEB.Controllers
{
    public class ClothController : BaseController
    {
        [HttpGet("cloths/list")]
        public async Task<IActionResult> List()
        {
            ClothViewModel model = new()
            {
                Customers = (await Mediator.Send(new GetCustomersQuery())).Response,
                Cloths = (await Mediator.Send(new GetClothsQuery())).Response
            };
            return View(model);
        }
        public async Task<JsonResult> ListForSelect(int customerId)
        {
            var cloths = (await Mediator.Send(new GetClothsQuery(customerId))).Response;
            return Json(new { data = await this.RenderViewAsync("_GetCustomerCloths", cloths, true) });
        }

        [HttpPost]
        public async Task<JsonResult> Add(AddClothCommandRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = 404, message = "NotFound" });
            }
            CQRSResponse<int> response = await Mediator.Send(new AddClothCommand(model));
            if(response is null)
            {
                return Json(new { status = 404, message = "NotFound" });;
            }
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
                return Json(new { status = 500, message = "Internal Server error!" });

            CQRSResponse<int> response = await Mediator.Send(new DeleteClothCommand(id));

            if (response == null)
                return Json(new { status = 500, message = "Internal Server error!" });

            return Json(response);
        }
    }
}