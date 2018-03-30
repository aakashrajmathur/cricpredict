using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cricpredict.Controllers
{
    public class BBLController : Controller
    {
        // GET: BBL
        public ActionResult Index()
        {
            return View("Standings");
        }
        
        public ActionResult Standings(string date)
        {
            if(date == null)
            {
                date = "Today";
            }

            ViewData["PointsTable"] = GetPointsTableForDate(date);
            ViewData["Date"] = date;
            return View();
        }
        
        public ActionResult Games()
        {
            return View();
        }

        public ActionResult Graph()
        {

            return View();
        }

        public ActionResult Data()
        {
            ViewBag.Message = "Refresh underlying data";

            return View();
        }

        [HttpPost]
        public ActionResult Data(string input)
        {
            ViewBag.Title = "Data";

            if (input == "Password1")
            {

                //BigBashLeague18 bbl = new BigBashLeague18();
                //List<string> teamOrder = bbl.GetTeamOrder(); //TODO: Will be replaced by the one call to update the text file in Contents.

                ////foreach(string team in teamOrder)
                ////{
                ////    ViewBag.Message += team + Environment.NewLine;
                ////}

                //List<string> resultDisplayText = bbl.GetResult();

                ViewBag.Message += "Data Refreshed at " + DateTime.Now.ToString();
            }
            else
            {
                ViewBag.Message = "Could not refresh data because the password did not match.";
            }

            return View();
        }

        private List<string> GetPointsTableForDate(string date)
        {
            //Read file.

            List<string> pointsTable =  new List<string>();

            if(date == "Today")
            {
                //pointsTable.Add()
            }
            else
            {
                //back track... 

            }

            return pointsTable;
        }

    }
}