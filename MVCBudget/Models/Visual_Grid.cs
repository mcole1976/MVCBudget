using MVCBudget.Service;
using MVCBudget.Models;

namespace MVCBudget.Models
{
    public class Visual_Grid
    {
        private int _selected;
        private decimal _income_amount;
        private decimal _total_costs;
        private decimal _net_income;
        public List<KeyValuePair<int, DateOnly>> Income_Lots { get; private set; }
        public List<Income_Lots> Income { get; private set; }
        public int Selected { get => _selected; set => _selected = value; }
        public decimal Income_amount { get => _income_amount; set => _income_amount = value; }
        public decimal Total_costs { get => _total_costs; set => _total_costs = value; }
        public decimal Net_income { get => _net_income; set => _net_income = value; }

        public Visual_Grid()
        {
            // Initialize the properties
            Income_Lots = new List<KeyValuePair<int, DateOnly>>();
            Income = new List<Income_Lots>();

            // Populate properties with data from the database
            PopulateFromDatabase();
        }

        private void PopulateFromDatabase()
        {
            Income_Lots = MYSQLAccess.GetMontlyIncome();
            Income = MYSQLAccess.GetIncomeLotsData();

            
            
        }

        public void SetSelected(int selected)
        {
            Income = Income.Where(i => i.Id == selected).ToList();
            bool check = false;
            decimal dC;
            var tc = (from f in Income   select f.Amount).Sum().ToString();
            check = decimal.TryParse(tc, out dC);
            if (check) { Total_costs = dC; }
            
            var dec = (from f in Income where f.Id == selected select f.Income).FirstOrDefault().ToString();
            check = false;
            dC = -1;
            bool t = decimal.TryParse(dec.ToString(), out dC);
            if (t) {
                Income_amount = dC;
                    }

            Net_income = Income_amount - Total_costs;

        }
    }
}
