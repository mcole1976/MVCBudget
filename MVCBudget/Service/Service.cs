using System.Reflection.Metadata.Ecma335;
using MVCBudget.Models;
using MySqlConnector;
namespace MVCBudget.Service
{
    public class Service
    {

        private readonly string _connectionString;
        public Service() { _connectionString = CostandIncomeService.GetConnectionString(); }
        public async Task InsertEntryWithIntermediate(Entry entry)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new MySqlCommand("InsertEntryWithIntermediate", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@p_Description", entry.Description);
            command.Parameters.AddWithValue("@p_Amount", entry.Amount);
            await command.ExecuteNonQueryAsync();
        }

        public async  Task DeleteEntry(int EntryID)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new MySqlCommand("Delete_Cost", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@P_Id", EntryID);


            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Income_Lots>> GetIncomeLotsData()
        {
            var incomeLots = new List<Income_Lots>();
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new MySqlCommand("GetIncomeandCostDataPerMonth", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var incomeLot = new Income_Lots
                {
                    Id = reader.GetInt32(0),
                    Period = reader.GetInt32(1),
                    Description_time = reader.GetString(2),
                    Entry_id = reader.GetInt32(3),
                    Entry_name = reader.GetString(4),
                    Amount = reader.GetDecimal(5),
                    Income = reader.GetDecimal(6)
                };
                incomeLots.Add(incomeLot);
            }
            return incomeLots;
        }

        public async Task < List<KeyValuePair<int, DateOnly>>> GetMontlyIncome()
        {
            List<KeyValuePair<int, DateOnly>> res = new();
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = new MySqlCommand("GetIncomeDataLastMonth", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    int period = reader.GetInt32("id");
                    DateOnly d = reader.GetDateOnly("Date");
                    KeyValuePair<int, DateOnly> kvp = new(period, d);
                    res.Add(kvp);
                }
            }
            catch (Exception)
            {

            }

            return res;
        }

        public async Task<KeyValuePair<decimal, decimal>> MakeRetrunDataKVP(int Id)
        {
            KeyValuePair<decimal, decimal> res = new();
            decimal d1 = 0;
            decimal d2 = 0;
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = new MySqlCommand("GeTPeriodCostByID", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@_ID", Id);

                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {


                    d1 = reader.GetDecimal("Income");
                    d2 = reader.GetDecimal("SummedAmnt");


                    res = new KeyValuePair<decimal, decimal>(d1, d2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return res;
        }


        public async Task<List<String>> GetEntryPossibles(int Id)
        {
            var entries = new List<string>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand("Get_Description_Data", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PI_ID", Id);
            using var readerTask = command.ExecuteReaderAsync();
            var reader = readerTask.Result; // Await the task to get the actual reader
            while (reader.Read()) // Use the reader object directly
            {
                string x = "new Entry";
                {
                    x = reader.GetString(0);
                }
                ;
                entries.Add(x);
            }
            return entries;
        }

    }
}
