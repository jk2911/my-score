using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyScore.ViewModel;
using MyScore.Model;
using MyScore.Model.DB;

namespace MyScore.ViewModel
{
    internal class MatchListViewModel
    {
        public ObservableCollection<MatchViewModel> MatchList { get; set; }
        public ObservableCollection<MatchViewModel> MatchDayList { get; set; }

        #region Конструктор

        public MatchListViewModel()
        {
            MatchList = new ObservableCollection<MatchViewModel>();
            MatchDayList = new ObservableCollection<MatchViewModel>();
            using (MyScoreDB db = new MyScoreDB())
            {
                DateTime date = DateTime.Now;
                foreach(var i in db.Matches)
                {
                    if (i.Date.Date > date)
                        MatchDayList.Add(new MatchViewModel(i));
                }
            }
        }

        #endregion
    }
}
