namespace MVCBudget.Models
{
    public class Entry
    {
        private string description;
        private decimal amount;
        private int period_ID;



        public string Description { get => description; set => description = value; }
        public decimal Amount { get => amount; set => amount = value; }
        public int Period_ID { get => period_ID; set => period_ID = value; }
    }
}
