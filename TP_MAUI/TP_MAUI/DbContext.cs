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
    }
}
