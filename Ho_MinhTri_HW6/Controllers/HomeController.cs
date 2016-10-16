using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ho_MinhTri_HW6.DAL;
using Ho_MinhTri_HW6.Models;

namespace Ho_MinhTri_HW6.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }
    }
}