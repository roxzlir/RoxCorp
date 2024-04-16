namespace RoxCorp.Models
{
    public class MonthlySummaryViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public IEnumerable<ApplyForLeave> Applies { get; set; }
    }

}
