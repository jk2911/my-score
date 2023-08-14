using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyScore.Model;
using System.Threading.Tasks;
using MyScore.Model.DB;

namespace MyScore.ViewModel
{
    public class LastMatchViewModel : NavigateViewModel
    {
        public Match match;
        public string Date
        {
            get => match.Date.ToShortDateString();
        }
        public string Home
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    return db.Teams.FirstOrDefault(p => p.Id == match.HomeId).Name;
                }
            }
        }
        public string Away
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    return db.Teams.FirstOrDefault(p => p.Id == match.AwayId).Name;
                }
            }
        }
        public string Goal1
        {
            get => match.Goal1.ToString();
        }
        public string Goal2
        {
            get => match.Goal2.ToString();
        }
        public LastMatchViewModel(Match i)
        {
            match = i;
            
        }
    }
}
