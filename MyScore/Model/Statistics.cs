using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Model
{
    public class Statistic
    {
        [Key]
        public int Id { get; set; }
        public int MatchId { get; set; }
        public byte BallControl1 { get; set; } = 50;
        public byte BallControl2 { get; set; } = 50;
        public byte Offside1 { get; set; } = 0;
        public byte Offside2 { get; set; } = 0;
        public byte Corner1 { get; set; } = 0;
        public byte Corner2 { get; set; } = 0;
        public byte RedCard1 { get; set; } = 0;
        public byte RedCard2 { get; set; } = 0;
        public byte YellowCard1 { get; set; } = 0;
        public byte YellowCard2 { get; set; } = 0;
        public byte Shots1 { get; set; } = 0;
        public byte Shots2 { get; set; } = 0;
    }
}
