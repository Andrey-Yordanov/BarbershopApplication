using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            Console.WriteLine($"[ERROR] Status code received: {statusCode}");

            return statusCode switch
            {
                404 => View("Error404"),
                500 => View("Error500"),
                _ => View("General")
            };
        }
    }
}
