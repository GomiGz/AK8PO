using Microsoft.Data.Sqlite;
using NLog;
using TP_MAUI.Models;
using Dapper;
using Windows.Networking;

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
                $"({match.TournamentId},{match.TournamentDepth},{match.TournamentLevel},{match.Player0},{match.Player1}),";
            }
            query = query.Substring(0,query.Length - 1);
            _conn.Execute(query);
            CloseConnection();
        }
    }
}
