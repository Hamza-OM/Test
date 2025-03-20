using PersonalInfoManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace PersonalInfoManagement.Controllers
{
    public class HomeController : Controller
    {
        private PersonalInfoDbContext db = new PersonalInfoDbContext();

        public ActionResult Index()
        {
            ViewBag.PersonCount = db.Persons.Count();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
} 