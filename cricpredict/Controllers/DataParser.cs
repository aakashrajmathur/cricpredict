using HtmlAgilityPack;
using System;
using System.Collections.Generic;

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
                    toBeWritten.Add(standings[10 + 12 * i + 1]);
                    toBeWritten.Add(standings[10 + 12 * i + 2]);
                    toBeWritten.Add(standings[10 + 12 * i + 3]);
                    toBeWritten.Add(standings[10 + 12 * i + 4]);
                    toBeWritten.Add(standings[10 + 12 * i + 5]);
                    toBeWritten.Add(standings[10 + 12 * i + 6]);
                    toBeWritten.Add(standings[10 + 12 * i + 8]);
                    toBeWritten.Add(standings[10 + 12 * i + 9]);
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
                        string when = results[i + 2].Substring(2, results[i+2].Length-8);                        
                        string week = GetWeek(when);

                        // IsGameComplete, Result, winner is in line: results[i + 3];  
                        //Match scheduled to begin at 16:00 local time (10:30 GMT)
                        //Kolkata Knight Riders won by 71 runs

                        string futureGame = "";
                        string winner = "";
                        string winnerNotes = "";
                        string homeTeamScore = "";
                        string awayTeamScore = "";

                        if (results[i + 3].Contains("Match scheduled to begin"))
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
    }
}