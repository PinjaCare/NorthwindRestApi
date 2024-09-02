using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return ("Hello!");
        }
    }
}
