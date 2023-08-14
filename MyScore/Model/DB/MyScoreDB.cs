using System;
using System.Data.Entity;
using System.Linq;

namespace MyScore.Model.DB
{
    public class MyScoreDB : DbContext
    {
        public MyScoreDB()
            : base("name=MyScoreDB")
        {
        }
        public DbSet<Match> Matches { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Championship> Championships { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<SelectedTeam> SelectedTeams { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
    }


}