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
            ViewData["UpcomingGames"] = new string[] { "MI", "CSK", "DD", "KP", "KKR", "RCB", "SRH", "RR", "CSK", "KKR", "RR", "DD", "SRH", "MI", "RCB", "KP", "MI", "DD", "KKR", "SRH", "RCB", "RR", "KP", "CSK", "KKR", "DD", "MI", "RCB", "RR", "KKR", "KP", "SRH", "CSK", "RR", "KKR", "KP", "DD", "RCB", "SRH", "CSK", "RR", "MI", "DD", "KP", "MI", "SRH", "RCB", "CSK", "SRH", "KP", "DD", "KKR", "CSK", "MI", "RR", "SRH", "RCB", "KKR", "CSK", "DD", "RCB", "MI", "DD", "RR", "KKR", "CSK", "KP", "MI", "CSK", "RCB", "SRH", "DD", "MI", "KKR", "KP", "RR", "SRH", "RCB", "RR", "KP", "KKR", "MI", "DD", "SRH", "RR", "CSK", "KP", "KKR", "RCB", "DD", "CSK", "SRH", "MI", "RR", "KP", "RCB", "KKR", "RR", "MI", "KP", "RCB", "SRH", "DD", "CSK", "RR", "RCB", "SRH", "KKR", "DD", "MI", "CSK", "KP" };
            ViewData["DefaultWinners"] = new string[] { "CSK", "KP", "RCB", "RR", "KKR", "DD", "MI", "KP", "DD", "SRH", "RR", "CSK", "DD", "RCB", "KKR", "SRH", "RR", "KP", "RCB", "CSK", "MI", "KP", "SRH", "CSK", "KP", "KKR", "MI", "SRH", "KKR", "DD", "MI", "RR", "CSK", "MI", "RCB", "DD", "KKR", "RR", "RCB", "KP", "MI", "SRH", "CSK", "KKR", "DD", "SRH", "RR", "RCB", "RR", "KP", "SRH", "CSK", "RCB", "KKR", "MI", "KP" };
            ViewData["Standings"] = new string[] { "CSK", "0", "0", "0", "0", "0", "50", "DD", "0", "0", "0", "0", "0", "50", "MI", "0", "0", "0", "0", "0", "50", "KKR", "0", "0", "0", "0", "0", "50", "KP", "0", "0", "0", "0", "0", "50", "RCB", "0", "0", "0", "0", "0", "50", "RR", "0", "0", "0", "0", "0", "50", "SRH", "0", "0", "0", "0", "0", "50" };

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