using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cricpredict.Controllers
{
    public class IPLController : Controller
    {
        // GET: IPL
        public ActionResult Index()
        {
            ViewData["CSK"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Chennai Super Kings.txt"));
            ViewData["DD"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Delhi Daredevils.txt"));
            ViewData["KP"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Kings XI Punjab.txt"));
            ViewData["KKR"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Kolkata Knight Riders.txt"));
            ViewData["MI"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Mumbai Indians.txt"));
            ViewData["RR"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Rajasthan Royals.txt"));
            ViewData["RCB"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Royal Challengers Bangalore.txt"));
            ViewData["SRH"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Sunrisers Hyderabad.txt"));
            return View("Auction");
        }

        public ActionResult Standings()
        {
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
            return View();
        }

        public ActionResult Auction()
        {

            ViewData["CSK"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Chennai Super Kings.txt"));
            ViewData["DD"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Delhi Daredevils.txt"));
            ViewData["KP"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Kings XI Punjab.txt"));
            ViewData["KKR"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Kolkata Knight Riders.txt"));
            ViewData["MI"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Mumbai Indians.txt"));
            ViewData["RR"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Rajasthan Royals.txt"));
            ViewData["RCB"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Royal Challengers Bangalore.txt"));
            ViewData["SRH"] = System.IO.File.ReadAllLines(Server.MapPath("~/Content/IPL/Auction/Sunrisers Hyderabad.txt"));
            return View();
        }
        
        public ActionResult Teams()
        {
            return View();
        }
    }
}