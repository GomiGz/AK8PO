using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_MAUI.Models
{
    internal class Tournament
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
        public List<Match> Matches { get; set; }

        public void GenerateMatches(List<int> players)
        {
            Random rng = new Random();
            players = players.OrderBy(a => rng.Next()).ToList();
            Matches = new List<Match>();
            for (int i = 0; i < players.Count; i+=2)
            {
                Matches.Add(
                        new Match(
                            TournamentId,
                            0,
                            i/2,
                            players[i],
                            players[i+1],
                            null,
                            null,
                            null));
            }
            App.dbContext.InsertMatches(Matches);
        }

    }
}
