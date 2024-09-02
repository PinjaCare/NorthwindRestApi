using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Alustetaan tietokantayhteys, eli luodaan instanssi luokasta minkä takana on osaavuus käyttää tietokantaa
        NorthwindOriginalContext db = new NorthwindOriginalContext(); //voisi olla myös pelkkä = new(); ja tarkottaisi samaa asiaa
        //Hakee kaikki asiakkaat:
        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            var asiakkaat = db.Customers.ToList();
            return Ok(asiakkaat);
        }
    }
}
