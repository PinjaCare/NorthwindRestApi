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

        //PERINTEINEN TAPA:
        //NorthwindOriginalContext db = new NorthwindOriginalContext(); //voisi olla myös pelkkä = new(); ja tarkottaisi samaa asiaa

        //DEPENDENCY INJEKTION TAPA:
        private NorthwindOriginalContext db;

        public CustomersController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }



        //Hakee kaikki asiakkaat:
        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException); //InnerException yleensä hyödyllisin, joka kertoo virheestä tarkemmin kuin e.Message
            }
        }

        //Hakee YHDEN asiakkaan pääavaimella:       
        [HttpGet("{id}")]
        //MYÖS NÄIN VOI TEHDÄ:
        //[HttpGet]       
        //[Route("{id}")]
        public ActionResult GetOneCustomerById(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id); //Find:lla voi pääavaimen perusteella etsiä 
                if (asiakas != null) //if-ehdoissa kannattaa ensimmäiseksi käsitellä kaikista todennäköisin vaihtoehto, tällä säästetään tietokoneen kapasiteettia
                {
                    return Ok(asiakas); //kontrolleri-metodit automaattisesti konvertoi json-muotoon lähetettävän datan
                }
                else
                {
                    //return BadRequest("Asiakasta id:llä " + id + "ei löydy.");
                    return NotFound($"Asiakasta id:llä {id} ei löydy"); //string interpolation - muuttujien ja stringien yhdistely
                }
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e); //vaihtelun vuoksi pelkkä e eikä e.InnerException
            }
        }

        //Uuden lisääminen:
        [HttpPost]
        public ActionResult AddNew([FromBody] Customer cust)
        {
            try
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                return Ok($"Lisättiin uusi asiakas {cust.CompanyName} from {cust.City}"); //interpolation
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        //Asiakkaan poistaminen:
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas != null) //Jos id:llä löytyy asiakas tietokannasta
                {
                    db.Customers.Remove(asiakas); //Remove-metodissa annetaan koko objekti parametrinä
                    db.SaveChanges();
                    return Ok("Asiakas " + asiakas.CompanyName + " poistettu.");
                }                               
                return NotFound("Asiakasta id:llä " + id + " ei löytynyt.");
                
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        //Asiakkaan muokkaus
        [HttpPut("{id}")]
        public ActionResult EditCustomer(string id, [FromBody]Customer customer)
        {
            var asiakas = db.Customers.Find(id);
            if (asiakas != null) 
            {
                asiakas.CompanyName = customer.CompanyName;
                asiakas.Address = customer.Address;
                asiakas.ContactName = customer.ContactName;
                asiakas.Country = customer.Country;
                asiakas.City = customer.City;
                asiakas.PostalCode = customer.PostalCode;
                asiakas.Phone = customer.Phone;
                asiakas.Fax = customer.Fax;
                asiakas.Region = customer.Region;

                db.SaveChanges();
                return Ok("Muokattu asiakasta " + asiakas.CompanyName);
            }
            return NotFound("Asiakasta ei löytynyt id:llä " + id);
        }

        // Hakee nimen osalla: /api/companyname/hakusana
        [HttpGet("companyname/{cname}")]
        public ActionResult GetByName(string cname)
        {
            try
            {
                var cust = db.Customers.Where(c => c.CompanyName.Contains(cname));
                //var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; <-- sama mutta traditional (linq-kysely)
                
                // var cust = db.Customers.Where(c => c.CompanyName == cname); <--- perfect match
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
