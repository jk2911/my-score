using MyScore.Model.DB;
using MyScore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Logic
{
    public class ResultsMatch
    {
        public static void EditResults()//изменение результатов в лиге
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                ResetResults();
                var matches = db.Matches.Where(p => p.Goal1 != null);
                if (matches.Count() == 0)
                    return;
                foreach(var i in matches)
                {
                    var home = db.Leagues.FirstOrDefault(p => p.TeamId == i.HomeId);
                    var away = db.Leagues.FirstOrDefault(p => p.TeamId == i.AwayId);
                    home.ScoreGoals += (int)i.Goal1;
                    away.ScoreGoals += (int)i.Goal2;
                    home.ConcededGoals += (int)i.Goal2;
                    away.ConcededGoals += (int)i.Goal1;
                    home.CountMatches += 1;
                    away.CountMatches += 1;
                    if (i.Goal1 > i.Goal2)
                    {
                        home.Win += 1;
                        home.Point += 3;
                        away.Lose += 1;
                    }
                    else if (i.Goal1 < i.Goal2)
                    {
                        away.Win += 1;
                        away.Point += 3;
                        home.Lose += 1;
                    }
                    else
                    {
                        home.Draw += 1;
                        home.Point += 1;
                        away.Draw += 1;
                        away.Point += 1;
                    }
                }
                db.SaveChanges();
                SortByPlace();
            }
        }
        public static void ResetResults()//обнуление показателей в лиге
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                var league = db.Leagues.Where(p => p.ChampionshipId == CurrentObject.IdChampionship);
                if (league.Count() == 0)
                    return;
                int place = 1;
                foreach (var i in league)
                {
                    i.Place = place;
                    place++;
                    i.CountMatches = 0;
                    i.Point = 0;
                    i.ScoreGoals = 0;
                    i.ConcededGoals = 0;
                    i.Win = 0;
                    i.Draw = 0;
                    i.Lose = 0;
                }
                db.SaveChanges();
            }
        }
        public static void SortByPlace()//определение мест в лиге
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                var league = db.Leagues.Where(p => p.ChampionshipId == CurrentObject.IdChampionship).OrderByDescending(p => p.Point).ThenByDescending(p => p.ScoreGoals).ThenBy(p => p.ConcededGoals);
                if (league.Count() == 0)
                    return;
                int place = 1;
                foreach (var i in league)
                {
                    i.Place = place;
                    place++;
                }
                db.SaveChanges();
            }

        } 
        public static void DeleteMatches(int ChampionshipId, int TeamId)//удаление матчей команды из чемпионата
        {
            using(MyScoreDB db=new MyScoreDB())
            {
                var matches = db.Matches.Where(p => p.ChampionshipId == ChampionshipId && (p.HomeId == TeamId || p.AwayId == TeamId));
                if (matches.Count() == 0)
                    return;
                Statistic statistic = null;
                foreach (var i in matches)
                {
                    db.Matches.Remove(i);
                    statistic = db.Statistics.FirstOrDefault(p => p.MatchId == i.Id);
                    if (statistic != null)
                        db.Statistics.Remove(statistic);
                }
                db.SaveChanges();
            }
        }
        public static void DeleteChampionship(int ChampionshipId)//удаление чемпионата
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                var matches = db.Matches.Where(p => p.ChampionshipId == ChampionshipId);
                Statistic statistic = null;
                foreach (var i in matches)
                {
                    db.Matches.Remove(i);
                    statistic = db.Statistics.FirstOrDefault(p => p.MatchId == i.Id);
                    if (statistic != null)
                        db.Statistics.Remove(statistic);
                }

                var leagues = db.Leagues.Where(p => p.ChampionshipId == ChampionshipId);
                foreach (var i in leagues)
                {
                    db.Leagues.Remove(i);
                }

                var championship = db.Championships.FirstOrDefault(p => p.Id == ChampionshipId);
                db.Championships.Remove(championship);
                db.SaveChanges();
            }
        }
        public static void DeleteTeam(int TeamId)//удаление команды
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                var matches = db.Matches.Where(p => p.HomeId == TeamId || p.AwayId == TeamId);
                Statistic statistic = null;
                foreach (var i in matches)
                {
                    db.Matches.Remove(i);
                    statistic = db.Statistics.FirstOrDefault(p => p.MatchId == i.Id);
                    if (statistic != null)
                        db.Statistics.Remove(statistic);
                }

                var leagues = db.Leagues.Where(p => p.TeamId == TeamId);
                foreach (var i in leagues)
                {
                    db.Leagues.Remove(i);
                }
                

                

                var selectedTeam = db.SelectedTeams.Where(p => p.TeamId == TeamId);
                foreach (var i in leagues)
                {
                    db.Leagues.Remove(i);
                }
                
                var team = db.Teams.FirstOrDefault(p => p.Id == TeamId);
                db.Teams.Remove(team);
                db.SaveChanges();
            }
            EditResults();
        }
    }
}
