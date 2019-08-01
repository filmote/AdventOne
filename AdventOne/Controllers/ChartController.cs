using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Piechart.Models;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using AdventOne.Models.View;
using PagedList;

namespace AdventOne.Controllers {

    public class ChartController : BaseController {

        private readonly ProjectContext db = new ProjectContext();

        // GET: Chart  
//        public ActionResult Index() {
//            return View();
//        }
        //public JsonResult BarChart() {
        //    var populations = from c in db.Population
        //                    select new AutoCompleteEntry {
        //                        ID = c.ID,
        //                        Label = "asdas"
        //                    };

        //    return Json(populations.ToList());


        //}

        [HttpGet]
        public JsonResult BarChart() {

            //var customers = from c in db.Population
            //                orderby c.Male
            //                select new AutoCompleteEntry {
            //                    ID = c.ID,
            //                    Label = "sss"
            //                };
            var customers = from c in db.Population
                            orderby c.Male, c.Female, c.Others
                            select c;

            //            return Json(customers.ToList());
            return Json(customers.ToList(), JsonRequestBehavior.AllowGet);

        }

    }
}