namespace MVCBudget.Models
{
    public class IncomeTotals
    {

        
            private string _description;
            private DateOnly _description_time;
            private decimal _income;

        public string Description { get => _description; set => _description = value; }
        public DateOnly Description_time { get => _description_time; set => _description_time = value; }
        public decimal Income { get => _income; set => _income = value; }
    }
}
