using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cricpredict.Controllers
{
    internal class DataParser
    {
        internal void RefreshStandings(IPL18Controller iPL18Controller)
        {
            //http://www.espncricinfo.com/table/series/8048/season/2018/ipl
            //responsive-table-wrap

            var url = @"http://www.espncricinfo.com/table/series/8048/season/2018/ipl";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'responsive-table-wrap')]");

            if (nodes.Count > 0)
            {
                string inputText = nodes[0].InnerHtml;
                List<string> standings = GetInnerTextFromHTML(inputText);

                List<string> toBeWritten = new List<string>();

                for (int i = 0; i < 8; i++)
                {
                    //MessageBox.Show("# " + standings[10 + 12 * i]);
                    toBeWritten.Add(standings[10 + 12 * i + 1]);//LongTeamName
                    toBeWritten.Add(standings[10 + 12 * i + 2]);//ShortTeamName
                    toBeWritten.Add(standings[10 + 12 * i + 3]);//P
                    toBeWritten.Add(standings[10 + 12 * i + 4]);//W
                    toBeWritten.Add(standings[10 + 12 * i + 5]);//L
                    toBeWritten.Add(standings[10 + 12 * i + 6]);//T
                    toBeWritten.Add(standings[10 + 12 * i + 8]);//Pts
                    toBeWritten.Add(standings[10 + 12 * i + 9]);//NRR                    
                }
                string newStandings = string.Join(",", toBeWritten.ToArray());
                System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/Standings.txt"), newStandings);
            }
        }

        private string GetWeek(string when)
        {
            //There may be a mid time time reported as  Apr 29-30
            //Remove the second day. 

            if (when.Contains("-"))
            {
                when = when.Split('-')[0];
            }


            DateTime date = DateTime.Parse(when);
            if ((date >= DateTime.Parse("4/7/2018")) && (date <= DateTime.Parse("4/15/2018")))
                return "1";
            else if ((date >= DateTime.Parse("4/16/2018")) && (date <= DateTime.Parse("4/22/2018")))
                return "2";
            else if ((date >= DateTime.Parse("4/23/2018")) && (date <= DateTime.Parse("4/29/2018")))
                return "3";
            else if ((date >= DateTime.Parse("4/30/2018")) && (date <= DateTime.Parse("5/6/2018")))
                return "4";
            else if ((date >= DateTime.Parse("5/7/2018")) && (date <= DateTime.Parse("5/13/2018")))
                return "5";
            else if ((date >= DateTime.Parse("5/14/2018")) && (date <= DateTime.Parse("5/20/2018")))
                return "6";
            else
                return "0";
        }

        private List<string> GetInnerTextFromHTML(string line)
        {
            //clean the input
            line = line.Replace('\r', ' ');
            line = line.Replace('\n', ' ');
            line = line.Replace('\t', ' ');
            line = line.Trim();

            List<string> innerText = new List<string>();

            bool isInTag = false;
            string temp = "";
            foreach (char c in line.ToCharArray())
            {
                if (c == '<')
                {
                    isInTag = true;
                    temp = temp.Trim();
                    if (temp.Length > 0)
                        innerText.Add(temp);
                    temp = "";
                }
                else if (c == '>')
                {
                    isInTag = false;
                }
                else
                {
                    if (!isInTag)
                        temp += c;
                }
            }
            return innerText;
        }

        internal void RefreshResults(IPL18Controller iPL18Controller)
        {
            var url = @"http://www.espncricinfo.com/ci/engine/series/1131611.html";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'news-list large-20 medium-20 small-20')]");

            if (nodes.Count > 0)
            {
                string inputText = nodes[0].InnerHtml;
                List<string> results = GetInnerTextFromHTML(inputText);
                List<string> toBeWritten = new List<string>();

                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].Contains("match:"))
                    {
                        // Team names, where is in line: results[i + 1]; 
                        //Sunrisers Hyderabad v Delhi Daredevils at Hyderabad (Deccan) 
                        string[] delimiters = { " v ", " at " };
                        string[] tokens = results[i + 1].Split(delimiters, StringSplitOptions.None);
                        //if (tokens.Length != 3)
                        //MessageBox.Show("Error in reading teamnames, venue.");
                        string homeTeamFullName = tokens[0];
                        string awayTeamFullName = tokens[1];
                        string where = tokens[2];

                        // when is in line: results[i + 2];         
                        //- May 5, 2018
                        string when = results[i + 2].Substring(2, results[i + 2].Length - 8);
                        string week = GetWeek(when);

                        // IsGameComplete, Result, winner is in line: results[i + 3];  
                        //Match scheduled to begin at 16:00 local time (10:30 GMT)
                        //Kolkata Knight Riders won by 71 runs

                        string futureGame = "";
                        string winner = "";
                        string winnerNotes = "";
                        string homeTeamScore = "";
                        string awayTeamScore = "";

                        if ((!results[i + 3].ToUpper().Contains("WON BY")) && ((results[i + 3].Contains("Match scheduled to begin")) || (results[i + 4].Contains("Live")) || (results[i + 5].Contains("Live"))))
                        {
                            futureGame = "true";
                            winner = homeTeamFullName;
                        }
                        else
                        {
                            futureGame = "false";
                            winner = "";
                            if (results[i + 3].Contains(homeTeamFullName))
                                winner = homeTeamFullName;
                            else
                                winner = awayTeamFullName;
                            winnerNotes = results[i + 3];

                            //Scores is in line: result[i+4]; Rajasthan Royals 217/4 (20/20 ov); Royal Challengers Bangalore 198/6 (20/20 ov)
                            string[] tokens2 = results[i + 4].Split(';');
                            //if (tokens2.Length != 2)
                            //    MessageBox.Show("Error in reading scores");

                            foreach (string s in tokens2)
                            {
                                string s2 = s.Trim();
                                if (s2.Contains(homeTeamFullName))
                                    homeTeamScore = s2;
                                else
                                    awayTeamScore = s2;
                            }
                        }
                        toBeWritten.Add(homeTeamFullName);
                        toBeWritten.Add(awayTeamFullName);
                        toBeWritten.Add(where);
                        toBeWritten.Add(when);
                        toBeWritten.Add(week);
                        toBeWritten.Add(futureGame);
                        toBeWritten.Add(winner);
                        toBeWritten.Add(winnerNotes);
                        toBeWritten.Add(homeTeamScore);
                        toBeWritten.Add(awayTeamScore);
                    }
                }

                string newResults = string.Join("|", toBeWritten.ToArray());
                System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/Results.txt"), newResults);

                //Write CurrentWeek to Defaults.txt
                string toBeWrittenDefaults = GetWeek(DateTime.Now.ToShortDateString());
                System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/Defaults.txt"), toBeWrittenDefaults);
            }
        }

        internal void RefreshPlayoffPercentages(IPL18Controller iPL18Controller)
        {
            //Get Results and standings 
            string results = System.IO.File.ReadAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/Results.txt"));                       


            List<string> homeTeam = new List<string>();
            List<string> awayTeam = new List<string>();

            string[] tokens = results.Split('|');
            var completedGamesCount = 0; 
            for (int i = 0; i < tokens.Length; i = i + 10)
            {
                if (tokens[i + 5] == "true") {
                    homeTeam.Add(tokens[i]);
                    awayTeam.Add(tokens[i + 1]);
                }
                else
                {
                    completedGamesCount++;
                }
            }

            string prevPlayOffPerc = System.IO.File.ReadAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPerc_After_" + (completedGamesCount - 1) + ".txt"));
            string[] tokensPP = prevPlayOffPerc.Split(',');
            Dictionary<string, double> prevPerc = GetPrevPerc(tokensPP);


            string standingsInput = System.IO.File.ReadAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/Standings.txt"));
            string[] tokensStandings = standingsInput.Split(',');
            List<TeamStandings> standings = new List<TeamStandings>();

            for(int i = 0; i < tokensStandings.Length; i = i + 8)
            {
                standings.Add(new TeamStandings(tokensStandings[i], int.Parse(tokensStandings[i + 3]), double.Parse(tokensStandings[i + 7])));
            }
            standings.Sort();
            standings.Reverse();
            Dictionary<string, int> playoffCount = GetPlayoffCount(standings, homeTeam, awayTeam, 0);

            List<KeyValuePair<string, int>> sorted = (from kv in playoffCount orderby kv.Value select kv).ToList();
            sorted.Reverse();

            List<string> toBeWritten = new List<string>();
            List<string> raw = new List<string>();
            raw.Add(count.ToString());
            //toBeWritten.Add(count.ToString());
            foreach (KeyValuePair<string, int> pair in sorted)
            {
                raw.Add(pair.Key);
                toBeWritten.Add(pair.Key);
                raw.Add(pair.Value.ToString());
                double value = (pair.Value * 100.0) / count;
                toBeWritten.Add(value.ToString("0.00"));
                double prevValue = prevPerc[pair.Key];

                if (prevValue < value)
                    toBeWritten.Add("UP");
                else if (prevValue > value)
                    toBeWritten.Add("DOWN");
                else
                    toBeWritten.Add("SAME");
            }
            
            System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPerc.txt"), string.Join(",", toBeWritten.ToArray()));
            System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPerc_After_" + completedGamesCount + ".txt"), string.Join(",", toBeWritten.ToArray()));
            System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPercRaw.txt"), string.Join(",", raw.ToArray()));
            System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPercRaw_After_" + completedGamesCount + ".txt"), string.Join(",", raw.ToArray()));
        }

        private Dictionary<string, double> GetPrevPerc(string[] tokensPP)
        {
            //Chennai Super Kings,97.42,Sunrisers Hyderabad,97.29,Kings XI Punjab,91.42,Kolkata Knight Riders,60.09,Rajasthan Royals,26.46,Royal Challengers Bangalore,16.64,Mumbai Indians,6.96,Delhi Daredevils,3.73
            Dictionary<string, double> prev = new Dictionary<string, double>(); 
            for(int i =0; i< tokensPP.Length; i++)
            {
                if((tokensPP[i] == "Chennai Super Kings") || (tokensPP[i] == "Sunrisers Hyderabad") || (tokensPP[i] == "Kings XI Punjab") || (tokensPP[i] == "Kolkata Knight Riders") || (tokensPP[i] == "Rajasthan Royals") || (tokensPP[i] == "Royal Challengers Bangalore") || (tokensPP[i] == "Mumbai Indians") || (tokensPP[i] == "Delhi Daredevils"))
                {
                    prev.Add(tokensPP[i], double.Parse(tokensPP[i + 1]));
                }
            }
            return prev;
        }

        private int count = 0;

        Dictionary<string, int> GetPlayoffCount(List<TeamStandings> standings, List<string> homeTeam, List<string> awayTeam, int pointer)
        {
            if(homeTeam.Count == pointer)
            {
                count++;
                Dictionary<string, int> res = new Dictionary<string, int>(); 
                for(int i = 0; i < 4; i++)
                {
                    res.Add(standings[i].teamFullName, 1);
                }
                for(int i = 4; i < 8; i++)
                {
                    res.Add(standings[i].teamFullName, 0);
                }
                return res;
            }
            else
            {
                string home = homeTeam[pointer];
                //homeTeam.RemoveAt(0);
                List<TeamStandings> homeStandings = GetStandingsForWinner(standings, home);

                string away = awayTeam[pointer];
                //awayTeam.RemoveAt(0);
                List<TeamStandings> awayStandings = GetStandingsForWinner(standings, away);

                Dictionary<string, int> homeRes = GetPlayoffCount(homeStandings, homeTeam, awayTeam, pointer+1);
                Dictionary<string, int> awayRes = GetPlayoffCount(awayStandings, homeTeam, awayTeam, pointer+1);

                Dictionary<string, int> mergedRes = new Dictionary<string, int>();
                foreach(string team in homeRes.Keys)
                {
                    mergedRes.Add(team, homeRes[team] + awayRes[team]);
                }
                return mergedRes;
            }
        }

        List<TeamStandings> GetStandingsForWinner(List<TeamStandings> standings, string winner)
        {
            List<TeamStandings> dup = new List<TeamStandings>(); 
            foreach(TeamStandings team in standings)
            {
                if (team.teamFullName == winner)
                    dup.Add(new TeamStandings(team.teamFullName, team.W + 1, team.NRR));
                else
                    dup.Add(new TeamStandings(team.teamFullName, team.W, team.NRR));
            }
            dup.Sort();
            dup.Reverse();
            return dup;
        }

    }

    class TeamStandings : IComparable<TeamStandings>
    {
        public string teamFullName;
        public int W;
        public double NRR;

        public int CompareTo(TeamStandings other)
        {
            if (this.W != other.W)
                return this.W - other.W;
            else if (this.NRR != other.NRR)
                return (int)Math.Round(this.NRR * 10000 - other.NRR * 10000);
            else
                return this.teamFullName.CompareTo(other.teamFullName);            
        }
        public TeamStandings(string teamFullName, int W, double NRR)
        {
            this.teamFullName = teamFullName; this.W = W; this.NRR = NRR; 
        }
    }
}
