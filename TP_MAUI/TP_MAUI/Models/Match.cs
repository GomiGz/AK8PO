using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_MAUI.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int TournamentId { get; set; }
        public int TournamentDepth { get; set; }
        public int TournamentLevel { get; set; }
        public int Player0 { get; set; }
        public int Player1 { get; set; }
        public int? WinnerId { get; set; }
        public string Score0 { get; set; }
        public string Score1 { get; set; }

        public Match(int tournamentId, int tournamentDepth, int tournamentLevel, int player0, int player1, int? winnerId, string score0, string score1)
        {
            TournamentId = tournamentId;
            TournamentDepth = tournamentDepth;
            TournamentLevel = tournamentLevel;
            Player0 = player0;
            Player1 = player1;
            WinnerId = winnerId;
            Score0 = score0;
            Score1 = score1;
        }
    }
}
