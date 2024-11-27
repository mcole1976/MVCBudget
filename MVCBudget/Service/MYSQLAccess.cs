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
        public static bool InsertEntryWithIntermediate(Entry entry)
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

        public static Dictionary<int,string> GetDictionaryData()
        {

            Dictionary<int, string> res = new Dictionary<int, string>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("GetPeriodDescriptions", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader =  command.ExecuteReader()) 
                        { 
                            while ( reader.Read()) 
                            { int period = reader.GetInt32("Period"); 
                                string description = reader.GetString("Description"); 
                                res.Add(period, description); 
                            } 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return res;



        }
    

    public static bool InsertLedger(Ledger ledger)
        {

            bool ret = true;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("InsertLedger", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Month", ledger.Month);
                        command.Parameters.AddWithValue("@p_Year", ledger.Year);
                        command.Parameters.AddWithValue("@p_Period", ledger.Periaod);

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

        public static bool InsertPeriaod_and_Date(EntryDate entryDate)
        {

            bool ret = true;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("InsertPeriodAndDate", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_date", entryDate.DateOnly);
                        command.Parameters.AddWithValue("@p_period", entryDate.Selected);
                        

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
