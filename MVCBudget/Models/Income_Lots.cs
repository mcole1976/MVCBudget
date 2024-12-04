namespace MVCBudget.Models
{
    public class Income_Lots
    {
        private int _id;
        private int _period;
        private string _description_time;
        private int entry_id;
        private string _entry_name;
        private decimal _amount;
        private decimal _income;

        public int Id { get => _id; set => _id = value; }
        public int Period { get => _period; set => _period = value; }
        public string Description_time { get => _description_time; set => _description_time = value; }
        public int Entry_id { get => entry_id; set => entry_id = value; }
        public string Entry_name { get => _entry_name; set => _entry_name = value; }
        public decimal Amount { get => _amount; set => _amount = value; }
        public decimal Income { get => _income; set => _income = value; }


    }
}
