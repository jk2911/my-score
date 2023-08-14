using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Model
{
    public class League
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ChampionshipId { get; set; }
        public int TeamId { get; set; }
        public int CountMatches { get; set; } = 0;
        public int Place { get; set; } = 0;
        public int Point { get; set; } = 0;
        public int ScoreGoals { get; set; } = 0;
        public int ConcededGoals { get; set; } = 0;
        public int Win { get; set; } = 0;
        public int Draw { get; set; } = 0;
        public int Lose { get; set; } = 0;



    }
}
