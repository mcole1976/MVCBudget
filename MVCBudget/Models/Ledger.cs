namespace MVCBudget.Models
{
    public class Ledger
    {
        private int year;
        private int month;
        private int periaod;

        public Ledger(int year, int month, int periaod)
        {
            Year = year;
            Month = month;
            Periaod = periaod;
        }

        public int Year { get => year; set => year = value; }
        public int Month { get => month; set => month = value; }
        public int Periaod { get => periaod; set => periaod = value; }
    }
}
