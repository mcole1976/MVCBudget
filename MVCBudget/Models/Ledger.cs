namespace MVCBudget.Models
{
    public class Ledger
    {
        private  int entry_ID;
        private int year;
        private int month;
        private int periaod;

       

        public int Year { get => year; set => year = value; }
        public int Month { get => month; set => month = value; }
        public int Periaod { get => periaod; set => periaod = value; }
        public int Entry_ID { get => entry_ID; set => entry_ID = value; }

  
    }
}
