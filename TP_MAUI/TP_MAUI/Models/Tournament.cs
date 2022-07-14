namespace TP_MAUI.Models
{
    public class Tournament
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
            var totalDepth = Convert.ToInt32(Math.Sqrt(players.Count));
            for (int i = 1; i < totalDepth; i++)
            {
                var matchCount = Math.Pow(2, totalDepth - 1 - i);
                for (int j = 0; j < matchCount; j++)
                {
                    Matches.Add(
                            new Match(
                                TournamentId,
                                i,
                                j,
                                null,
                                null,
                                null,
                                null,
                                null));
                }
            }
            App.dbContext.InsertMatches(Matches);
        }

    }
}
