using Microsoft.Build.Framework;

namespace MVCBudget.Models
{
    public class EntryDate
    {
        private DateOnly _dateOnly;
        private Dictionary<int, string> _period;
        private int _selected;
        private List<IncomeTotals> _previousEntries;
        private decimal _income;
        [Required]
        public DateOnly DateOnly { get => _dateOnly; set => _dateOnly = value; }
        public Dictionary<int, string> Period { get => _period; set => _period = value; }

        [Required]
        public int Selected { get => _selected; set => _selected = value; }
        public List<IncomeTotals> PreviousEntries { get => _previousEntries; set => _previousEntries = value; }
        public decimal Income { get => _income; set => _income = value; }
    }
}
