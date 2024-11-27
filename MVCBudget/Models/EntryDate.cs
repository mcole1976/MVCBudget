using Microsoft.Build.Framework;

namespace MVCBudget.Models
{
    public class EntryDate
    {
        private DateOnly _dateOnly;
        private Dictionary<int, string> _period;
        private int _selected;
        [Required]
        public DateOnly DateOnly { get => _dateOnly; set => _dateOnly = value; }
        public Dictionary<int, string> Period { get => _period; set => _period = value; }

        [Required]
        public int Selected { get => _selected; set => _selected = value; }
    }
}
