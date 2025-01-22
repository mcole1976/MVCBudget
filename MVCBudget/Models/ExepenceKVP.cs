namespace MVCBudget.Models
{
    public class ExepenceKVP
    {

        private int _period;
        private decimal _income;
        private DateOnly _date;
        private string _name;

        public DateOnly Date { get => _date; set => _date = value; }
        public int Period { get => _period; set => _period = value; }
        public decimal Income { get => _income; set => _income = value; }
        public string Name { get => _name; set => _name = value; }



    }
}
