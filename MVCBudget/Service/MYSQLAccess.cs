using MVCBudget.Models;
using MySqlConnector;
using System.Configuration;

namespace MVCBudget.Service
{
    public static class MYSQLAccess
    {
        private static string _connectionString; 
        public static string GetConnectionString() { return _connectionString; }
        public static void SetConnectionString(string connectionString) { _connectionString = connectionString; }
        public static bool  InsertEntryWithIntermediate(Entry entry)
        {

            bool ret = true;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("InsertEntryWithIntermediate", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Description", entry.Description);
                        command.Parameters.AddWithValue("@p_Amount", entry.Amount);


                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine(rowsAffected);
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            
                return ret;
            
        }
    }
}
