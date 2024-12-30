using MVCBudget.Service;


namespace MVCBudget.Models
{
    public class GraphData
    {
        private List<IncomeKVP> incomeKVPs;
        private List<ExepenceKVP> exepenceKVPs;
        public GraphData() { }

        public List<IncomeKVP> IncomeKVPs { get => incomeKVPs; set => incomeKVPs = value; }
        public List<ExepenceKVP> ExepenceKVPs { get => exepenceKVPs; set => exepenceKVPs = value; }

        public void setData()
        {
            incomeKVPs = MYSQLAccess.SetIncomeKVP();
            exepenceKVPs = MYSQLAccess.SetExpenceKVP();

        }
    }
}
