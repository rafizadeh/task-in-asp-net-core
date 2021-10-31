using Application.Common.Base.Models;
using Application.CQRS.Cloths.Queries.GetCloths;
using Application.CQRS.Customers.Queries.GetCustomers;
using Application.CQRS.Shelves.Commands.AddToShelf;
using Application.CQRS.Shelves.Commands.DeleteFromShelf;
using Application.CQRS.Shelves.Queries.GetShelves;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Threading.Tasks;
using WEB.ViewModels;

namespace WEB.Controllers
{
    public class ShelfController : BaseController
    {
        [HttpGet("shelves/{page?}")]
        [HttpGet("shelf/list/{page?}")]
        public async Task<IActionResult> List(int page = 1)
        {
            ShelfViewModel model = new()
            {
                Customers = (await Mediator.Send(new GetCustomersQuery())).Response,
                ShelfModel = (await Mediator.Send(new GetShelvesQuery(page))).Response
            };

            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> Add(int clothId)
        {
            CQRSResponse<int> response = await Mediator.Send(new AddToShelfCommand(clothId));
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
                return Json(new { status = 500, message = "Internal Server error!" });

            CQRSResponse<int> response = await Mediator.Send(new DeleteFromShelfCommand(id));

            if (response == null)
                return Json(new { status = 500, message = "Internal Server error!" });

            return Json(response);
        }
    }
}