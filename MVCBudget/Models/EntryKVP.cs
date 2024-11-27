namespace MVCBudget.Models
{
    public class EntryKVP
    {
        private Dictionary<int, string> _period;
        private DateOnly _date;
        public Dictionary<int, string> Period { get => _period; set => _period = value; }
        public DateOnly Date { get => _date; set => _date = value; }
    }
}
