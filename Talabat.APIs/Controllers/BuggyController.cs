using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    public class BuggyController :ApiBaseController
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext context)//Clr
        {
            this.context = context;
        }
        [HttpGet("notfound")]//api/buggy/notfound
        public ActionResult GetNotFoundRequest()
        {
            var product = context.Products.Find(100);
            if (product is null) return NotFound(new ApiErrorResponse(404));

            return Ok(product);
        }
        [HttpGet("servererror")]//api/buggy/servererror
        public ActionResult GetServerError()
        {
            var product = context.Products.Find(100);
            var ProducttoReturn = product.ToString();//Will Throw Exception
            return Ok(ProducttoReturn);
        }

        [HttpGet("badRequest")]//api/buggy/badRequest
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiErrorResponse(400));
        }

        [HttpGet("badRequest/{id}")]//api/buggy/badRequest/five
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }



    }
}
