using MVCBudget.Models;
using MySqlConnector;
namespace MVCBudget.Service
{
    public class Service
    {

        private readonly string _connectionString; 
        public Service() { _connectionString = MYSQLAccess.GetConnectionString(); }
        public async Task InsertEntryWithIntermediate(Entry entry) 
        { 
            using (var connection = new MySqlConnection(_connectionString)) 
            { await connection.OpenAsync(); 
                using (var command = new MySqlCommand("InsertEntryWithIntermediate", connection)) 
                { 
                    command.CommandType = System.Data.CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@p_Description", entry.Description); 
                    command.Parameters.AddWithValue("@p_Amount", entry.Amount); 
                    await command.ExecuteNonQueryAsync(); 
                } 
            } 
        }
        //public Dictionary<string, int> GetEntryDictionary()
        //{
        //    var entries = new Dictionary<string, int>();
        //    using (var connection = new MySqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        using (
        //            var command = new MySqlCommand("SELECT Id, Description FROM Entry", connection))
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                entries.Add(reader["Description"].ToString(), (int)reader["Id"]);

        //            }

        //            return entries;
        //        }
        //    }
        //}

    }
}
