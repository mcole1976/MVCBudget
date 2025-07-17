using MVCBudget.Service;

namespace MVCBudget.Models
{
    public class Visual_Grid
    {
        private int _selected;
        private decimal _income_amount;
        private decimal _total_costs;
        private decimal _net_income;

        private Service.Service _s;

        public Service.Service S { get => _s; set => _s = value; }

        public List<KeyValuePair<int, DateOnly>> Income_Lots { get; private set; }
        public List<Income_Lots> Income { get; private set; }

        
        public List<string> Possibles { get; private set; } = new List<string>();
        public int Selected { get => _selected; set => _selected = value; }
        public decimal Income_amount { get => _income_amount; set => _income_amount = value; }
        public decimal Total_costs { get => _total_costs; set => _total_costs = value; }
        public decimal Net_income { get => _net_income; set => _net_income = value; }

        public Visual_Grid()
        {
            // Initialize the properties
            Income_Lots = new List<KeyValuePair<int, DateOnly>>();
            Income = new List<Income_Lots>();
            Possibles = new List<string>();

            // Initialize the non-nullable field '_s' with a default instance
            _s = new Service.Service();
            Income_Lots = S.GetMontlyIncome().Result;
            Income = S.GetIncomeLotsData().Result;
  

            // Populate properties with data from the database
            //PopulateFromDatabase();
        }

       
 public void SetSelected(int selected)
        {
            List<Income_Lots> CheckList = Income.Where(i => i.Id == selected).ToList();

            Income = Income.Where(i => i.Id == selected).ToList();

            // Await the task to resolve the CS0029 error
            Task<List<string>> task = S.GetEntryPossibles(selected);
            Possibles = task.Result; // Use .Result to get the result of the Task

            // Fix the CS1002 error by completing the statement with a semicolon
            // Possibles is already assigned above, so no additional code is needed here

            if (CheckList.Count > 0)
            {
                bool check = false;
                decimal dC;
                var tc = (from f in Income select f.Amount).Sum().ToString();
                check = decimal.TryParse(tc, out dC);
                if (check) { Total_costs = dC; }

                var dec = (from f in Income where f.Id == selected select f.Income).FirstOrDefault().ToString();
                check = false;
                dC = -1;
                bool t = decimal.TryParse(dec.ToString(), out dC);
                if (t)
                {
                    Income_amount = dC;
                }

                Net_income = Income_amount - Total_costs;
            }
        }
    }
}
