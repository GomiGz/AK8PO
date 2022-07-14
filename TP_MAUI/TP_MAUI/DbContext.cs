using Microsoft.Data.Sqlite;
using NLog;
using TP_MAUI.Models;
using Dapper;
using Windows.Networking;
using System.Text.RegularExpressions;
using Match = TP_MAUI.Models.Match;

namespace TP_MAUI
{
    public class DbContext
    {
        private readonly SqliteConnection _conn;
        private readonly Logger _logger;

        public DbContext()
        {
            _conn = new SqliteConnection("Data Source=C:\\School\\AK8PO\\TP_MAUI\\TP_MAUI\\db.sqlite");
            _logger = LogManager.GetCurrentClassLogger();
        }

        private void OpenConnection()
        {
            _conn.Open();
        }

        private void CloseConnection()
        {
            _conn.Close();
        }

        public List<Level> GetLevels()
        {
            OpenConnection();
            try
            {
                var query = $"SELECT * " +
                            $"FROM Levels";
                var result = _conn.Query<Level>(query);
                CloseConnection();

                return result.AsList();
            }
            catch (Exception e)
            {
                _logger.Debug(e, "Chyba DB");
                CloseConnection();
                throw;
            }
        }

        public List<Player> GetPlayers()
        {
            OpenConnection();
            try
            {
                var query = $"SELECT * " +
                            $"FROM Players";
                var result = _conn.Query<Player>(query);
                CloseConnection();

                return result.AsList();
            }
            catch (Exception e)
            {
                _logger.Debug(e, "Chyba DB");
                CloseConnection();
                throw;
            }
        }

        public Player GetPlayer(int playerId)
        {
            OpenConnection();
            var query = $"SELECT " +
                        $"p.PlayerId, " +
                        $"p.FirstName, " +
                        $"p.LastName, " +
                        $"p.Email, " +
                        $"p.Age, " +
                        $"p.LevelId " +
                        $"FROM Players p " +
                        $"WHERE p.PlayerId = {playerId}";
            var result = _conn.QueryFirst<Player>(query);
            CloseConnection();
            return result;
        }

        public void SavePlayer(Player player)
        {
            OpenConnection();
            var query = $"INSERT INTO Players ("+
                        $"FirstName,"+
                        $"LastName,"+
                        $"Email,"+
                        $"Age,"+
                        $"LevelId"+
                    $")"+
                    $"VALUES("+
                        $"'{player.FirstName}',"+
                        $"'{player.LastName}',"+
                        $"'{player.Email}',"+
                        $"'{player.Age}',"+
                        $"'{player.LevelId}');";

            _conn.Execute(query);
            CloseConnection();
        }

        public void UpdatePlayer(Player player)
        {
            OpenConnection();
            var query =$"UPDATE Players SET " +
                $"FirstName = '{player.FirstName}'," +
                $"LastName = '{player.LastName}'," +
                $"Email = '{player.Email}'," +
                $"Age = '{player.Age}'," +
                $"LevelId = {player.LevelId} " +
                $"Where PlayerId = {player.PlayerId}";
            _conn.Execute(query);
            CloseConnection();
        }

        public void DeletePlayer(int playerId)
        {
            OpenConnection();
            var query = $"DELETE FROM Players " +
                $"Where PlayerId = {playerId}";
            _conn.Execute(query);
            CloseConnection();
        }

        public List<Player> FindPlayers(int upperAge, int lowerAge, int upperSkill, int lowerSkill)
        {
            OpenConnection();
            var query = $@"SELECT 
                               PlayerId,
                               FirstName,
                               LastName,
                               Age
                            FROM Players
                            WHERE
                            Age >= {lowerAge} and Age <= {upperAge} and
                            LevelId >= {lowerSkill} and LevelId <= {upperSkill}";
            var result = _conn.Query<Player>(query);
            CloseConnection();
            return result.ToList();
        }

        public int InsertTournament(string tournamentName)
        {
            OpenConnection();

            var query = $"INSERT INTO Tournaments (" +
                            $"TournamentName" +
                        $")" +
                        $"VALUES('{tournamentName}')";
            _conn.Execute(query);
            query = "SELECT last_insert_rowid()";
            var id = _conn.QueryFirstOrDefault<int>(query,0);
            CloseConnection();
            return id;
        }

        public void InsertMatches(List<Match> matches)
        {
            OpenConnection();

            string query = $"INSERT INTO Matches (" +
                            $"TournamentId," +
                            $"TournamentDepth," +
                            $"TournamentLevel," +
                            $"Player0," +
                            $"Player1" +
                        $") VALUES ";

            foreach (Match match in matches) {
                query += 
                $"({match.TournamentId},{match.TournamentDepth},{match.TournamentLevel},{(match.Player0.HasValue? match.Player0.Value.ToString() : "null")},{(match.Player1.HasValue ? match.Player1.Value.ToString() : "null")}),";
            }
            query = query.Substring(0,query.Length - 1);
            _conn.Execute(query);
            CloseConnection();
        }
        
        public List<Match> GetMatchesOfTournament(int tournamentId)
        {
            OpenConnection();
            var query = $"SELECT " +
                            $"m.MatchId, " +
                            $"m.TournamentId, " +
                            $"m.TournamentDepth, " +
                            $"m.TournamentLevel, " +
                            $"m.Player0, " +
                            $"(p0.FirstName || ' ' || p0.LastName) as Player0Name, " +
                            $"m.Player1, " +
                            $"(p1.FirstName || ' ' || p1.LastName) as Player1Name, " +
                            $"m.WinnerId, " +
                            $"IFNULL(m.Score0,'') as Score0, " +
                            $"IFNULL(m.Score1,'') as Score1 " +
                        $"FROM Matches m " +
                        $"LEFT JOIN Players p0 on p0.PlayerId = m.Player0 " +
                        $"LEFT JOIN Players p1 on p1.PlayerId = m.Player1 " +
                        $"WHERE TournamentId = {tournamentId}";
            var result = _conn.Query<Match>(query);
            CloseConnection();
            return result.ToList();
        }

        public List<Tournament> GetTournaments()
        {
            OpenConnection();
            var query = $"SELECT " +
                            $"TournamentId, " +
                            $"TournamentName " +
                        $"FROM Tournaments ";
            var result = _conn.Query<Tournament>(query);
            CloseConnection();
            return result.ToList();
        }

        public void SetWinner(Match match)
        {
            OpenConnection();

            var query = "SELECT WinnerId " +
                            "FROM Matches " +
                            $"WHERE TournamentId = {match.TournamentId} and " +
                            $"TournamentLevel = {Convert.ToInt32(match.TournamentLevel / 2)} and " +
                            $"TournamentDepth = {match.TournamentDepth + 1} ";
            var winner = _conn.QueryFirstOrDefault<int?>(query);
            if(winner.HasValue)
            {
                return;
            }
            using (var transaction = _conn.BeginTransaction())
            {
                query = $"UPDATE Matches " +
                            $"SET WinnerId = {match.WinnerId} " +
                            $"WHERE MatchId = {match.MatchId} ";
                _conn.Execute(query);

                query = $"UPDATE Matches ";
                if ((match.TournamentLevel % 2) == 0)
                {
                    query += $"SET Player0 = {match.WinnerId} "; 

                }
                else
                {
                    query +=$"SET Player1 = {match.WinnerId} ";
                }

                query +=$"WHERE TournamentId = {match.TournamentId} and " +
                        $"TournamentLevel = {Convert.ToInt32(match.TournamentLevel / 2)} and " +
                        $"TournamentDepth = {match.TournamentDepth + 1} ";
                _conn.Execute(query);
                transaction.Commit();
            }
            CloseConnection();
        }

        public void DeleteTournament(int tournamentId)
        {
            OpenConnection();
            using (var transaction = _conn.BeginTransaction())
            {
                var query = "DELETE FROM Matches " +
                            $"WHERE TournamentId = {tournamentId}";
                _conn.Execute(query);
                query = "DELETE FROM Tournaments " +
                            $"WHERE TournamentId = {tournamentId}";
                _conn.Execute(query);
                transaction.Commit();
            }
            CloseConnection();
        }

        public void ResetTournament(int tournamentId)
        {
            OpenConnection();
            using (var transaction = _conn.BeginTransaction())
            {
                var query = $"UPDATE Matches " +
                            $"SET WinnerId = null " +
                            $"WHERE TournamentId = {tournamentId} ";
                _conn.Execute(query);
                query = $"UPDATE Matches " +
                            $"SET Player0 = null, Player1 = null " +
                            $"WHERE TournamentId = {tournamentId} " +
                            $"AND TournamentDepth > 0";
                _conn.Execute(query);
                transaction.Commit();
            }
            CloseConnection();
        }
    }
}
