using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Model
{
    public class Match
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Stage { get; set; } = 1;
        public int ChampionshipId { get; set; }
        public int HomeId { get; set; } = 0;
        public int AwayId { get; set; } = 0;
        public byte? Goal1 { get; set; } = null;
        public byte? Goal2 { get; set; } = null;
        public int Statistic;
    }
}
