namespace TP_MAUI.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int TournamentId { get; set; }
        public int TournamentDepth { get; set; }
        public int TournamentLevel { get; set; }
        public int? Player0 { get; set; }
        public string Player0Name { get; set; }
        public int? Player1 { get; set; }
        public string Player1Name { get; set; }
        public int? WinnerId { get; set; }
        public string Score0 { get; set; }
        public string Score1 { get; set; }

        public Match()
        {

        }
        public Match(Match old)
        {
            this.MatchId = old.MatchId;
            this.TournamentId = old.TournamentId;
            this.TournamentDepth = old.TournamentDepth;
            this.TournamentLevel = old.TournamentLevel;
            this.Player0 = old.Player0;
            this.Player1 = old.Player1;
            this.Player1Name = old.Player1Name;
            this.Player0Name = old.Player0Name;
            this.WinnerId = old.WinnerId;
            this.Score0 = old.Score0;
            this.Score1 = old.Score1;
        }

        public Match(int tournamentId, int tournamentDepth, int tournamentLevel, int? player0, int? player1, int? winnerId, string score0, string score1)
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
