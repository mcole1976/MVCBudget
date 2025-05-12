namespace MVCBudget.Models
{
    public class EntryResponse
    {
        private bool _success;
        private string _message;
        private decimal? _income;
        private decimal? _costs;    
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal? Income { get; set; }
        public decimal? Costs { get; set; }
    }
}
