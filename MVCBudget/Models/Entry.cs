namespace MVCBudget.Models
{
    public class Entry
    {
        private string description;
        private decimal amount;

       

        public string Description { get => description; set => description = value; }
        public decimal Amount { get => amount; set => amount = value; }
    }
}
