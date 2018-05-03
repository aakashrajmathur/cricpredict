using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cricpredict.Controllers
{
    public class IPL18Controller : Controller
    {
        // GET: IPL
        public ActionResult Index()
        {
            ViewData["Results"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Results.txt"));
            ViewData["Standings"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Standings.txt"));
            ViewData["Defaults"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Defaults.txt"));
            ViewData["PlayoffPerc"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/PlayoffPerc.txt"));            
            return View("Predictions");
        }

        public ActionResult Games()
        {
            ViewData["UpcomingGames"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/UpcomingGames.txt")); //"Sat Apr 7,Mumbai Indians,Chennai Super Kings,Mumbai,Sat Apr 7,Kings XI Punjab,Delhi Daredevils,Mohali,Sun Apr 8 ,Kolkata Knight Riders,Royal Challengers Bangalore,Kolkata,Mon Apr 9 ,Sunrisers Hyderabad,Rajasthan Royals,Hyderabad,Tue Apr 10 ,Chennai Super Kings,Kolkata Knight Riders,Chennai,Wed Apr 11,Rajasthan Royals,Delhi Daredevils,Jaipur,Thu Apr 12 ,Sunrisers Hyderabad,Mumbai Indians,Hyderabad,Fri Apr 13 ,Royal Challengers Bangalore,Kings XI Punjab,Bengaluru,Sat Apr 14,Mumbai Indians,Delhi Daredevils,Mumbai,Sat Apr 14 ,Kolkata Knight Riders,Sunrisers Hyderabad,Kolkata,Sun Apr 15,Royal Challengers Bangalore,Rajasthan Royals,Bengaluru,Sun Apr 15 ,Kings XI Punjab,Chennai Super Kings,Mohali,Mon Apr 16,Kolkata Knight Riders,Delhi Daredevils,Kolkata,Tue Apr 17,Mumbai Indians,Royal Challengers Bangalore,Mumbai,Wed Apr 18,Rajasthan Royals,Kolkata Knight Riders,Jaipur,Thu Apr 19,Kings XI Punjab,Sunrisers Hyderabad,Mohali,Fri Apr 20,Chennai Super Kings,Rajasthan Royals,Chennai,Sat Apr 21,Kolkata Knight Riders,Kings XI Punjab,Kolkata,Sat Apr 21,Royal Challengers Bangalore,Delhi Daredevils,Bengaluru,Sun Apr 22,Sunrisers Hyderabad,Chennai Super Kings,Hyderabad,Sun Apr 22,Rajasthan Royals,Mumbai Indians,Jaipur,Mon Apr 23,Delhi Daredevils,Kings XI Punjab,Delhi,Tue Apr 24,Mumbai Indians,Sunrisers Hyderabad,Mumbai,Wed Apr 25,Royal Challengers Bangalore,Chennai Super Kings,Bengaluru,Thu Apr 26,Sunrisers Hyderabad,Kings XI Punjab,Hyderabad,Fri Apr 27,Delhi Daredevils,Kolkata Knight Riders,Delhi,Sat Apr 28,Chennai Super Kings,Mumbai Indians,Chennai,Sun Apr 29,Rajasthan Royals,Sunrisers Hyderabad,Jaipur,Sun Apr 29,Royal Challengers Bangalore,Kolkata Knight Riders,Bengaluru,Mon Apr 30,Chennai Super Kings,Delhi Daredevils,Chennai,Tue May 1,Royal Challengers Bangalore,Mumbai Indians,Bengaluru,Wed May 2,Delhi Daredevils,Rajasthan Royals,Delhi,Thu May 3 ,Kolkata Knight Riders,Chennai Super Kings,Kolkata,Fri May 4 ,Kings XI Punjab,Mumbai Indians,Indore,Sat May 5,Chennai Super Kings,Royal Challengers Bangalore,Chennai,Sat May 5 ,Sunrisers Hyderabad,Delhi Daredevils,Hyderabad,Sun May 6,Mumbai Indians,Kolkata Knight Riders,Mumbai,Sun May 6,Kings XI Punjab,Rajasthan Royals,Indore,Mon May 7,Sunrisers Hyderabad,Royal Challengers Bangalore,Hyderabad,Tue May 8,Rajasthan Royals,Kings XI Punjab,Jaipur,Wed May 9,Kolkata Knight Riders,Mumbai Indians,Kolkata,Thu May 10,Delhi Daredevils,Sunrisers Hyderabad,Delhi,Fri May 11,Rajasthan Royals,Chennai Super Kings,Jaipur,Sat May 12,Kings XI Punjab,Kolkata Knight Riders,Indore,Sat May 12,Delhi Daredevils,Royal Challengers Bangalore,Delhi,Sun May 13,Chennai Super Kings,Sunrisers Hyderabad,Chennai,Sun May 13,Mumbai Indians,Rajasthan Royals,Mumbai,Mon May 14,Kings XI Punjab,Royal Challengers Bangalore,Indore,Tue May 15,Kolkata Knight Riders,Rajasthan Royals,Kolkata,Wed May 16,Mumbai Indians,Kings XI Punjab,Mumbai,Thu May 17,Royal Challengers Bangalore,Sunrisers Hyderabad,Bengaluru,Fri May 18,Delhi Daredevils,Chennai Super Kings,Delhi,Sat May 19,Rajasthan Royals,Royal Challengers Bangalore,Jaipur,Sat May 19,Sunrisers Hyderabad,Kolkata Knight Riders,Hyderabad,Sun May 20,Delhi Daredevils,Mumbai Indians,Delhi,Sun May 20,Chennai Super Kings,Kings XI Punjab,Chennai";
            ViewData["Winners"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/DefaultWinner.txt")); //"Chennai Super Kings,Delhi Daredevils,Royal Challengers Bangalore,Rajasthan Royals,Kolkata Knight Riders,Delhi Daredevils,Mumbai Indians,Kings XI Punjab,Delhi Daredevils,Sunrisers Hyderabad,Rajasthan Royals,Chennai Super Kings,Delhi Daredevils,Royal Challengers Bangalore,Kolkata Knight Riders,Sunrisers Hyderabad,Rajasthan Royals,Kings XI Punjab,Royal Challengers Bangalore,Chennai Super Kings,Mumbai Indians,Kings XI Punjab,Sunrisers Hyderabad,Chennai Super Kings,Kings XI Punjab,Kolkata Knight Riders,Mumbai Indians,Sunrisers Hyderabad,Kolkata Knight Riders,Delhi Daredevils,Mumbai Indians,Rajasthan Royals,Chennai Super Kings,Mumbai Indians,Royal Challengers Bangalore,Delhi Daredevils,Kolkata Knight Riders,Rajasthan Royals,Royal Challengers Bangalore,Kings XI Punjab,Mumbai Indians,Sunrisers Hyderabad,Chennai Super Kings,Kolkata Knight Riders,Delhi Daredevils,Sunrisers Hyderabad,Rajasthan Royals,Royal Challengers Bangalore,Rajasthan Royals,Kings XI Punjab,Sunrisers Hyderabad,Chennai Super Kings,Royal Challengers Bangalore,Kolkata Knight Riders,Mumbai Indians,Kings XI Punjab";
            ViewData["Standings"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Standings.txt")); //"Chennai Super Kings,0,0,0,0,0,50,Delhi Daredevils,0,0,0,0,0,50,Mumbai Indians,0,0,0,0,0,50,Kolkata Knight Riders,0,0,0,0,0,50,Kings XI Punjab,0,0,0,0,0,50,Royal Challengers Bangalore,0,0,0,0,0,50,Rajasthan Royals,0,0,0,0,0,50,Sunrisers Hyderabad,0,0,0,0,0,50";

            return View();
        }

        public ActionResult Standings()
        {
            ViewData["UpcomingGames"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/UpcomingGames.txt")); //"Sat Apr 7,Mumbai Indians,Chennai Super Kings,Mumbai,Sat Apr 7,Kings XI Punjab,Delhi Daredevils,Mohali,Sun Apr 8 ,Kolkata Knight Riders,Royal Challengers Bangalore,Kolkata,Mon Apr 9 ,Sunrisers Hyderabad,Rajasthan Royals,Hyderabad,Tue Apr 10 ,Chennai Super Kings,Kolkata Knight Riders,Chennai,Wed Apr 11,Rajasthan Royals,Delhi Daredevils,Jaipur,Thu Apr 12 ,Sunrisers Hyderabad,Mumbai Indians,Hyderabad,Fri Apr 13 ,Royal Challengers Bangalore,Kings XI Punjab,Bengaluru,Sat Apr 14,Mumbai Indians,Delhi Daredevils,Mumbai,Sat Apr 14 ,Kolkata Knight Riders,Sunrisers Hyderabad,Kolkata,Sun Apr 15,Royal Challengers Bangalore,Rajasthan Royals,Bengaluru,Sun Apr 15 ,Kings XI Punjab,Chennai Super Kings,Mohali,Mon Apr 16,Kolkata Knight Riders,Delhi Daredevils,Kolkata,Tue Apr 17,Mumbai Indians,Royal Challengers Bangalore,Mumbai,Wed Apr 18,Rajasthan Royals,Kolkata Knight Riders,Jaipur,Thu Apr 19,Kings XI Punjab,Sunrisers Hyderabad,Mohali,Fri Apr 20,Chennai Super Kings,Rajasthan Royals,Chennai,Sat Apr 21,Kolkata Knight Riders,Kings XI Punjab,Kolkata,Sat Apr 21,Royal Challengers Bangalore,Delhi Daredevils,Bengaluru,Sun Apr 22,Sunrisers Hyderabad,Chennai Super Kings,Hyderabad,Sun Apr 22,Rajasthan Royals,Mumbai Indians,Jaipur,Mon Apr 23,Delhi Daredevils,Kings XI Punjab,Delhi,Tue Apr 24,Mumbai Indians,Sunrisers Hyderabad,Mumbai,Wed Apr 25,Royal Challengers Bangalore,Chennai Super Kings,Bengaluru,Thu Apr 26,Sunrisers Hyderabad,Kings XI Punjab,Hyderabad,Fri Apr 27,Delhi Daredevils,Kolkata Knight Riders,Delhi,Sat Apr 28,Chennai Super Kings,Mumbai Indians,Chennai,Sun Apr 29,Rajasthan Royals,Sunrisers Hyderabad,Jaipur,Sun Apr 29,Royal Challengers Bangalore,Kolkata Knight Riders,Bengaluru,Mon Apr 30,Chennai Super Kings,Delhi Daredevils,Chennai,Tue May 1,Royal Challengers Bangalore,Mumbai Indians,Bengaluru,Wed May 2,Delhi Daredevils,Rajasthan Royals,Delhi,Thu May 3 ,Kolkata Knight Riders,Chennai Super Kings,Kolkata,Fri May 4 ,Kings XI Punjab,Mumbai Indians,Indore,Sat May 5,Chennai Super Kings,Royal Challengers Bangalore,Chennai,Sat May 5 ,Sunrisers Hyderabad,Delhi Daredevils,Hyderabad,Sun May 6,Mumbai Indians,Kolkata Knight Riders,Mumbai,Sun May 6,Kings XI Punjab,Rajasthan Royals,Indore,Mon May 7,Sunrisers Hyderabad,Royal Challengers Bangalore,Hyderabad,Tue May 8,Rajasthan Royals,Kings XI Punjab,Jaipur,Wed May 9,Kolkata Knight Riders,Mumbai Indians,Kolkata,Thu May 10,Delhi Daredevils,Sunrisers Hyderabad,Delhi,Fri May 11,Rajasthan Royals,Chennai Super Kings,Jaipur,Sat May 12,Kings XI Punjab,Kolkata Knight Riders,Indore,Sat May 12,Delhi Daredevils,Royal Challengers Bangalore,Delhi,Sun May 13,Chennai Super Kings,Sunrisers Hyderabad,Chennai,Sun May 13,Mumbai Indians,Rajasthan Royals,Mumbai,Mon May 14,Kings XI Punjab,Royal Challengers Bangalore,Indore,Tue May 15,Kolkata Knight Riders,Rajasthan Royals,Kolkata,Wed May 16,Mumbai Indians,Kings XI Punjab,Mumbai,Thu May 17,Royal Challengers Bangalore,Sunrisers Hyderabad,Bengaluru,Fri May 18,Delhi Daredevils,Chennai Super Kings,Delhi,Sat May 19,Rajasthan Royals,Royal Challengers Bangalore,Jaipur,Sat May 19,Sunrisers Hyderabad,Kolkata Knight Riders,Hyderabad,Sun May 20,Delhi Daredevils,Mumbai Indians,Delhi,Sun May 20,Chennai Super Kings,Kings XI Punjab,Chennai";
            ViewData["Winners"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/DefaultWinner.txt")); //"Chennai Super Kings,Delhi Daredevils,Royal Challengers Bangalore,Rajasthan Royals,Kolkata Knight Riders,Delhi Daredevils,Mumbai Indians,Kings XI Punjab,Delhi Daredevils,Sunrisers Hyderabad,Rajasthan Royals,Chennai Super Kings,Delhi Daredevils,Royal Challengers Bangalore,Kolkata Knight Riders,Sunrisers Hyderabad,Rajasthan Royals,Kings XI Punjab,Royal Challengers Bangalore,Chennai Super Kings,Mumbai Indians,Kings XI Punjab,Sunrisers Hyderabad,Chennai Super Kings,Kings XI Punjab,Kolkata Knight Riders,Mumbai Indians,Sunrisers Hyderabad,Kolkata Knight Riders,Delhi Daredevils,Mumbai Indians,Rajasthan Royals,Chennai Super Kings,Mumbai Indians,Royal Challengers Bangalore,Delhi Daredevils,Kolkata Knight Riders,Rajasthan Royals,Royal Challengers Bangalore,Kings XI Punjab,Mumbai Indians,Sunrisers Hyderabad,Chennai Super Kings,Kolkata Knight Riders,Delhi Daredevils,Sunrisers Hyderabad,Rajasthan Royals,Royal Challengers Bangalore,Rajasthan Royals,Kings XI Punjab,Sunrisers Hyderabad,Chennai Super Kings,Royal Challengers Bangalore,Kolkata Knight Riders,Mumbai Indians,Kings XI Punjab";
            ViewData["Standings"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Standings.txt")); //"Chennai Super Kings,0,0,0,0,0,50,Delhi Daredevils,0,0,0,0,0,50,Mumbai Indians,0,0,0,0,0,50,Kolkata Knight Riders,0,0,0,0,0,50,Kings XI Punjab,0,0,0,0,0,50,Royal Challengers Bangalore,0,0,0,0,0,50,Rajasthan Royals,0,0,0,0,0,50,Sunrisers Hyderabad,0,0,0,0,0,50";
            ViewData["Results"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Results.txt"));
            return View();
        }

        public ActionResult Teams()
        {
            if (RouteData.Values["id"] != null) { ViewData["id"] = RouteData.Values["id"].ToString(); } else { ViewData["id"] = -1; }
            ViewData["PlayerInfo"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/PlayerInfo.txt"));
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

        public ActionResult Predictions()
        {            
            ViewData["Results"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Results.txt"));
            ViewData["Standings"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Standings.txt"));
            ViewData["Defaults"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Defaults.txt"));
            ViewData["PlayoffPerc"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/PlayoffPerc.txt"));
            return View();
        }

        public ActionResult Graph()
        {
            int startingGameIndex = 30;
            int gameIndex = startingGameIndex;
            List<string> playoffPerc = new List<string>();
            while (true)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Content/IPL/Data/PlayoffPerc_After_" + gameIndex + ".txt")))
                {
                     playoffPerc.Add( gameIndex + "," + System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/PlayoffPerc_After_" + gameIndex + ".txt")));
                }
                else
                {
                    break;
                }
                gameIndex++;
            }

            ViewData["GamesWisePlayoffPercentages"] = string.Join("|", playoffPerc.ToArray());
            ViewData["Results"] = System.IO.File.ReadAllText(Server.MapPath("~/Content/IPL/Data/Results.txt"));            
            return View();
        }

        public ActionResult Data()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Data(string password)
        {
            if (password == "123")
            {
                //Scrap the website and update the text files.
                new DataParser().RefreshStandings(this);
                new DataParser().RefreshResults(this);
                new DataParser().RefreshPlayoffPercentages(this);
                ViewData["status"] = "Standings and Results refreshed at " + DateTime.Now.ToLocalTime().ToLongDateString() + " " + DateTime.Now.ToLocalTime().ToLongTimeString();
            }
            else
            {
                ViewData["status"] = "Password was incorrect.";
            }
            return View();
        }

    }
}