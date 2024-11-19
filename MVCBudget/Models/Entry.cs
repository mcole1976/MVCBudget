namespace MVCBudget.Models
{
    public class Entry
    {
        private string description;
        private decimal amount;

        public Entry(string description, decimal amount)
        {
            Description = description;
            Amount = amount;
        }

        public string Description { get => description; set => description = value; }
        public decimal Amount { get => amount; set => amount = value; }
    }
}
