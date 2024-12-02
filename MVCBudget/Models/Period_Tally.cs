namespace MVCBudget.Models
{
    public class Period_Tally
    {
        Dictionary<int, DateOnly> _date;
        Dictionary<string, decimal> _period_Data;
        private int _selected;

        

        public Dictionary<int, DateOnly> Date { get => _date; set => _date = value; }
        public Dictionary<string, decimal> Period_Data { get => _period_Data; set => _period_Data = value; }
        public int Selected { get => _selected; set => _selected = value; }
    }
}
