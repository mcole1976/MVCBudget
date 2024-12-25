using Microsoft.Extensions.Hosting;
using MVCBudget.Models;
using MySqlConnector;
using System.Configuration;
using System.Reflection;

namespace MVCBudget.Service
{
    public static class MYSQLAccess
    {
        private static string _connectionString;
        public static string GetConnectionString() { return _connectionString; }
        public static void SetConnectionString(string connectionString) { _connectionString = connectionString; }
        public static bool InsertEntryWithIntermediate(Period_Tally  model)
        {

            bool ret = true;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    foreach (KeyValuePair<string, decimal> kvp in model.Period_Data)
                    {
                        using (var command = new MySqlCommand("AddEntry", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@entryDescription", kvp.Key);
                            command.Parameters.AddWithValue("@entryAmount", kvp.Value);
                            command.Parameters.AddWithValue("@periodAndDateId", model.Selected);


                            int rowsAffected = command.ExecuteNonQuery();

                            Console.WriteLine(rowsAffected);
                        }
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

        //GetPayPeriodIDandDesc

        public static List<IncomeTotals> GetDictionaryDataDateDesc()
        {

            List<IncomeTotals> res = new List<IncomeTotals>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("GetPayPeriodIDandDesc", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                IncomeTotals i = new IncomeTotals();
                                i.Description = reader.GetString("Description");
                                i.Description_time = reader.GetDateOnly("Date");
                                i.Income = reader.GetDecimal("Income");
                                
                                res.Add(i);
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
        internal static Dictionary<int, DateOnly> GetDictionaryDatePeriodData()
        {
            Dictionary<int, DateOnly> res = new Dictionary<int, DateOnly>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("GetPeriodandDate", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int period = reader.GetInt32("id");
                                DateOnly dateData = reader.GetDateOnly("date");
                                res.Add(period, dateData);
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
        internal static Dictionary<int, DateOnly> GetDictionaryIncomeDateData()
        {
            Dictionary<int, DateOnly> res = new Dictionary<int, DateOnly>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("GetPeriodandDateIncome", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int period = reader.GetInt32("id");
                                DateOnly dateData = reader.GetDateOnly("date");
                                res.Add(period, dateData);
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
        internal static List<KeyValuePair<int, DateOnly>> GetMontlyIncome()
        {
            List<KeyValuePair<int, DateOnly>> res = new List<KeyValuePair<int, DateOnly>>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("GetIncomeDataLastMonth", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int period = reader.GetInt32("id");
                                DateOnly d = reader.GetDateOnly("Date");
                                KeyValuePair<int, DateOnly> kvp = new KeyValuePair<int, DateOnly>(period, d);
                                res.Add(kvp);
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

        internal static Dictionary<int, DateOnly> GetDictionaryDateData()
        {
            Dictionary<int, DateOnly> res = new Dictionary<int, DateOnly>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("GetPeriodAndDate", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int period = reader.GetInt32("id");
                                DateOnly dateData = reader.GetDateOnly("date");
                                res.Add(period, dateData);
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

        internal static List<Income_Lots> GetIncomeLotsData()
        {
            {
                List<Income_Lots> res = new List<Income_Lots>();
                try
                {
                    using (var connection = new MySqlConnection(_connectionString))
                    {
                        connection.Open();
                        using (var command = new MySqlCommand("GetIncomeandCostDataPerMonth", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Income_Lots IL = new Income_Lots();

                                    IL.Id = reader.GetInt32("id");
                                    IL.Period = reader.GetInt32("Period");
                                    IL.Description_time = reader.GetString("time desc");
                                    IL.Entry_id = reader.GetInt32("Entry_ID");
                                    IL.Entry_name = reader.GetString("entry Desc");
                                    IL.Amount = reader.GetDecimal("amount");
                                    IL.Income = reader.GetDecimal("income");

                                    res.Add(IL);
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
        }

        internal static bool InsertIncome(Income model)
        {
            bool ret = true;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("InsertIncome", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@amount", model.Net_Income);
                        command.Parameters.AddWithValue("@val_period_and_date_Id", model.Selected);


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

        internal static bool AmendIncome(int entryId, decimal income)
        {
            bool ret = true;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("Amend_Income", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@P_Income", income );
                        command.Parameters.AddWithValue("@P_Id", entryId);


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

        internal static bool Amend_Cost(int id, decimal cost)
        {
            bool ret = true;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("Amend_Cost", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@P_Cost", cost);
                        command.Parameters.AddWithValue("@P_Id", id);


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

        internal static bool DeleteCost(int id)
        {
            bool ret = true;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("Delete_Cost", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@P_Id", id);


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
