namespace MVCBudget.Models
{
    public class EntryDate
    {
        private DateTime _dateOnly;
        private Dictionary<int, string> _period;
        private int _selected;

        public DateTime DateOnly { get => _dateOnly; set => _dateOnly = value; }
        public Dictionary<int, string> Period { get => _period; set => _period = value; }
        public int Selected { get => _selected; set => _selected = value; }
    }
}
