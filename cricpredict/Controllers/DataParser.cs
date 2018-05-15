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
                            winner = "";//homeTeamFullName;
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
                if (tokens[i + 5] == "true")
                {
                    homeTeam.Add(tokens[i]);
                    awayTeam.Add(tokens[i + 1]);
                }
                else
                {
                    completedGamesCount++;
                }
            }
            string prevPlayOffPerc = "";
            try
            {
                prevPlayOffPerc = System.IO.File.ReadAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPerc_After_" + (completedGamesCount - 1) + ".txt"));
            }
            catch
            {
                prevPlayOffPerc = System.IO.File.ReadAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPerc.txt"));
            }
            string[] tokensPP = prevPlayOffPerc.Split(',');
            Dictionary<string, double> prevPerc = GetPrevPerc(tokensPP);

            string standingsInput = System.IO.File.ReadAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/Standings.txt"));
            string[] tokensStandings = standingsInput.Split(',');
            List<TeamStandings> standings = new List<TeamStandings>();

            for (int i = 0; i < tokensStandings.Length; i = i + 8)
            {
                standings.Add(new TeamStandings(tokensStandings[i], int.Parse(tokensStandings[i + 3]), double.Parse(tokensStandings[i + 7])));
            }
            standings.Sort();
            standings.Reverse();
            Dictionary<string, int> playoffCount = GetPlayoffCount_Definite(standings, homeTeam, awayTeam, 0);

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
                if ((value > 99.95) && (value < 100.0))
                    toBeWritten.Add(">99.9");
                else
                    toBeWritten.Add(value.ToString("0.0"));
                double prevValue = prevPerc[pair.Key];

                if (prevValue < value)
                    toBeWritten.Add("UP");
                else if (prevValue > value)
                    toBeWritten.Add("DOWN");
                else
                    toBeWritten.Add("SAME");
            }

            System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPerc.txt"), string.Join(",", toBeWritten.ToArray()));
            System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPerc_After_" + (completedGamesCount) + ".txt"), string.Join(",", toBeWritten.ToArray()));
            System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPercRaw.txt"), string.Join(",", raw.ToArray()));
            System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/PlayoffPercRaw_After_" + (completedGamesCount) + ".txt"), string.Join(",", raw.ToArray()));
        }

        private Dictionary<string, double> GetPrevPerc(string[] tokensPP)
        {
            //Chennai Super Kings,97.42,Sunrisers Hyderabad,97.29,Kings XI Punjab,91.42,Kolkata Knight Riders,60.09,Rajasthan Royals,26.46,Royal Challengers Bangalore,16.64,Mumbai Indians,6.96,Delhi Daredevils,3.73
            Dictionary<string, double> prev = new Dictionary<string, double>();
            for (int i = 0; i < tokensPP.Length; i++)
            {
                if ((tokensPP[i] == "Chennai Super Kings") || (tokensPP[i] == "Sunrisers Hyderabad") || (tokensPP[i] == "Kings XI Punjab") || (tokensPP[i] == "Kolkata Knight Riders") || (tokensPP[i] == "Rajasthan Royals") || (tokensPP[i] == "Royal Challengers Bangalore") || (tokensPP[i] == "Mumbai Indians") || (tokensPP[i] == "Delhi Daredevils"))
                {
                    prev.Add(tokensPP[i], double.Parse(tokensPP[i + 1]));
                }
            }
            return prev;
        }

        private int count = 0;

        Dictionary<string, int> GetPlayoffCount(List<TeamStandings> standings, List<string> homeTeam, List<string> awayTeam, int pointer)
        {
            if (homeTeam.Count == pointer)
            {
                count++;
                Dictionary<string, int> res = new Dictionary<string, int>();
                for (int i = 0; i < 4; i++)
                {
                    res.Add(standings[i].teamFullName, 1);
                }
                for (int i = 4; i < 8; i++)
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

                Dictionary<string, int> homeRes = GetPlayoffCount(homeStandings, homeTeam, awayTeam, pointer + 1);
                Dictionary<string, int> awayRes = GetPlayoffCount(awayStandings, homeTeam, awayTeam, pointer + 1);

                Dictionary<string, int> mergedRes = new Dictionary<string, int>();
                foreach (string team in homeRes.Keys)
                {
                    mergedRes.Add(team, homeRes[team] + awayRes[team]);
                }
                return mergedRes;
            }
        }

        Dictionary<string, int> GetPlayoffCount_Definite(List<TeamStandings> standings, List<string> homeTeam, List<string> awayTeam, int pointer)
        {
            if (homeTeam.Count == pointer)
            {
                count++;
                Dictionary<string, int> res = new Dictionary<string, int>();

                //Determine reverse ranking, 
                //Add teams with Rank 1 to 4.
                HashSet<string> topFour = GetTopFour(standings);

                for (int i = 0; i < standings.Count; i++)
                {
                    if (topFour.Contains(standings[i].teamFullName))
                    {
                        res.Add(standings[i].teamFullName, 1);
                    }
                    else
                    {
                        res.Add(standings[i].teamFullName, 0);
                    }
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

                Dictionary<string, int> homeRes = GetPlayoffCount_Definite(homeStandings, homeTeam, awayTeam, pointer + 1);
                Dictionary<string, int> awayRes = GetPlayoffCount_Definite(awayStandings, homeTeam, awayTeam, pointer + 1);

                Dictionary<string, int> mergedRes = new Dictionary<string, int>();
                foreach (string team in homeRes.Keys)
                {
                    mergedRes.Add(team, homeRes[team] + awayRes[team]);
                }
                return mergedRes;
            }
        }

        private HashSet<string> GetTopFour(List<TeamStandings> standings)
        {
            HashSet<string> topFour = new HashSet<string>();
            standings.Reverse();

            Dictionary<string, int> rank = new Dictionary<string, int>();

            int prevPoints = -1;
            int prevRank = -1;
            for (int i = 0; i < standings.Count; i++)
            {
                if (standings[i].W * 2 != prevPoints)
                {
                    prevPoints = standings[i].W * 2;
                    prevRank = standings.Count - i;
                    rank.Add(standings[i].teamFullName, prevRank);
                }
                else
                {
                    rank.Add(standings[i].teamFullName, prevRank);
                }
            }

            foreach (string team in rank.Keys)
            {
                if (rank[team] <= 4)
                    topFour.Add(team);
            }

            return topFour;
        }

        List<TeamStandings> GetStandingsForWinner(List<TeamStandings> standings, string winner)
        {
            List<TeamStandings> dup = new List<TeamStandings>();
            foreach (TeamStandings team in standings)
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


        internal void RefreshStats(IPL18Controller iPL18Controller)
        {
            RefreshBatsmenStats(iPL18Controller);
            RefreshBowlersStats(iPL18Controller);
        }

        private void RefreshBowlersStats(IPL18Controller iPL18Controller)
        {
            //Bowling stats:
            var url = @"http://stats.espncricinfo.com/ci/engine/records/averages/bowling.html?id=12210;type=tournament";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//table[contains(@class, 'engineTable')]");

            if (nodes.Count > 0)
            {
                string inputText = nodes[0].InnerHtml;
                List<string> stats = GetInnerTextFromHTML(inputText);

                List<PlayerBowlersStats> BowlersStats = new List<PlayerBowlersStats>();
                /*1- 15 are headers     Player = 0, Mat , Inns = 2, Overs, Mdns, Runs, Wkts = 6, BBI = 7, Ave = 8, Econ = 9, SR = 10, 4, 5, Ct, St, team = 15*/

                for (int i = 16; i < stats.Count; i = i + 16)
                {
                    BowlersStats.Add(new PlayerBowlersStats(stats[i + 0], stats[i + 2], stats[i + 6], stats[i + 7], stats[i + 8], stats[i + 9], stats[i + 10], stats[i + 15]));
                }

                BowlersStats.Sort();
                BowlersStats.Reverse();

                List<string> toBeWritten = new List<string>();

                for (int i = 0; i < 5; i++)
                {
                    toBeWritten.Add(BowlersStats[i].name);
                    toBeWritten.Add(BowlersStats[i].matches);
                    toBeWritten.Add(BowlersStats[i].wickets);
                    toBeWritten.Add(BowlersStats[i].best);
                    toBeWritten.Add(BowlersStats[i].avg);
                    toBeWritten.Add(BowlersStats[i].econ);
                    toBeWritten.Add(BowlersStats[i].strikeRate);
                    toBeWritten.Add(BowlersStats[i].team);
                }
                System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/BowlersStats.txt"), string.Join(",", toBeWritten.ToArray()));

            }
        }

        private void RefreshBatsmenStats(IPL18Controller iPL18Controller)
        {
            var url = @"http://stats.espncricinfo.com/ci/engine/records/averages/batting.html?id=12210;type=tournament";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//table[contains(@class, 'engineTable')]");

            if (nodes.Count > 0)
            {
                string inputText = nodes[0].InnerHtml;
                List<string> stats = GetInnerTextFromHTML(inputText);

                List<PlayerBattingStats> batsmenStats = new List<PlayerBattingStats>();
                /*1- 14 are headers     0 = name, 2 = innings, 4 = runs, 5 = HS, 6 = Avg, 8 = SR, 14= Team*/
                for (int i = 15; i < stats.Count; i = i + 15)
                {
                    batsmenStats.Add(new PlayerBattingStats(stats[i + 0], stats[i + 2], stats[i + 4], stats[i + 5], stats[i + 6], stats[i + 8], stats[i + 14]));
                }

                batsmenStats.Sort();
                batsmenStats.Reverse();

                List<string> toBeWritten = new List<string>();

                for (int i = 0; i < 5; i++)
                {
                    toBeWritten.Add(batsmenStats[i].name);
                    toBeWritten.Add(batsmenStats[i].innings);
                    toBeWritten.Add(batsmenStats[i].runs);
                    toBeWritten.Add(batsmenStats[i].highScore);
                    toBeWritten.Add(batsmenStats[i].avg);
                    toBeWritten.Add(batsmenStats[i].strikeRate);
                    toBeWritten.Add(batsmenStats[i].team);
                }
                System.IO.File.WriteAllText(iPL18Controller.Server.MapPath("~/Content/IPL/Data/BatsmenStats.txt"), string.Join(",", toBeWritten.ToArray()));
            }
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

    class PlayerBattingStats : IComparable<PlayerBattingStats>
    {
        public string name;
        public string innings;
        public string runs;
        public string highScore;
        public string avg;
        public string strikeRate;
        public string team;

        public PlayerBattingStats(string name, string innings, string runs, string highScore, string avg, string strikeRate, string team)
        {
            this.name = name;
            this.innings = innings;
            this.runs = runs;
            this.highScore = highScore;
            this.avg = avg;
            this.strikeRate = strikeRate;
            this.team = team.Substring(1, team.Length - 2);
        }

        public int CompareTo(PlayerBattingStats other)
        {
            bool isThisInt; bool isOtherInt;

            try { int t = int.Parse(this.runs); isThisInt = true; } catch { isThisInt = false; }
            try { int t = int.Parse(other.runs); isOtherInt = true; } catch { isOtherInt = false; }

            if (isThisInt && isOtherInt)
                return int.Parse(this.runs) - int.Parse(other.runs);
            else if (isThisInt)
                return +1;
            else if (isOtherInt)
                return -1;
            else
                return this.name.CompareTo(other.name);
        }
    }

    class PlayerBowlersStats : IComparable<PlayerBowlersStats>
    {
        public string name;
        public string matches;
        public string wickets;
        public string best;
        public string avg;
        public string econ;
        public string strikeRate;
        public string team;

        public PlayerBowlersStats(string name, string matches, string wickets, string best, string avg, string econ, string strikeRate, string team)
        {
            this.name = name;
            this.matches = matches;
            this.wickets = wickets;
            this.best = best;
            this.avg = avg;
            this.econ = econ;
            this.strikeRate = strikeRate;
            this.team = team.Substring(1, team.Length - 2);
        }

        public int CompareTo(PlayerBowlersStats other)
        {
            bool thisWicketInt; bool otherWicketInt;
            try { int w = int.Parse(this.wickets); thisWicketInt = true; } catch { thisWicketInt = false; }
            try { int w = int.Parse(other.wickets); otherWicketInt = true; } catch { otherWicketInt = false; }

            if (thisWicketInt && otherWicketInt)
            {
                if (int.Parse(this.wickets) != int.Parse(other.wickets))
                {
                    return int.Parse(this.wickets) - int.Parse(other.wickets);
                }
                else
                {
                    //if wickets are same, compare the econ.
                    bool thisEconFloat; bool otherEconFloat;
                    try { double a = double.Parse(this.econ); thisEconFloat = true; } catch { thisEconFloat = false; }
                    try { double a = double.Parse(other.econ); otherEconFloat = true; } catch { otherEconFloat = false; }

                    if (thisEconFloat && otherEconFloat)
                    {
                        if (double.Parse(this.econ) != double.Parse(other.econ))
                        {
                            if ((double.Parse(other.econ) - double.Parse(this.econ)) > 0)
                                return 1;
                            else if (double.Parse(other.econ) == double.Parse(this.econ))
                                return 0;
                            else
                                return -1;
                        }
                        else
                        {
                            return this.name.CompareTo(other.name);
                        }
                    }
                    else if (thisEconFloat)
                        return 1;
                    else if (otherEconFloat)
                        return -1;
                    else
                        return this.name.CompareTo(other.name);
                }
            }
            else if (thisWicketInt)
            {
                return +1;
            }
            else if (otherWicketInt)
            {
                return -1;
            }
            else
            {
                return this.name.CompareTo(other.name);
            }
        }
    }

}