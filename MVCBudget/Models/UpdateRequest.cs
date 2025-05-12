namespace MVCBudget.Models
{
    public class UpdateRequest
    {
        private int _entryId;
        private decimal _amount;
        private int _id;
        public int EntryId { get; set; }
        public decimal Amount { get; set; }
        public int Id { get; set; }
    }
}
