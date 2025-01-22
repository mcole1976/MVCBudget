namespace MVCBudget.Models
{
    public class Income
    {
        private DateOnly _dateOnly;
        private Dictionary<int, DateOnly> _dates;
        private int _selected;
        private decimal _net_Income;


        public DateOnly DateOnly { get => _dateOnly; set => _dateOnly = value; }
        public int Selected { get => _selected; set => _selected = value; }
        public decimal Net_Income { get => _net_Income; set => _net_Income = value; }
        public Dictionary<int, DateOnly> Dates { get => _dates; set => _dates = value; }
    }
}
