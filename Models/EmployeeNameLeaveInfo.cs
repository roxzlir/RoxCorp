namespace RoxCorp.Models
{
    public class EmployeeNameLeaveInfo
    {
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public string ApplyNote { get; set; }
        public DateTime ApplyRegisteredDate { get; set; }
        public DateTime ApplyFromDate { get; set; }
        public DateTime ApplyToDate { get; set; }
        public bool Granted { get; set; }

    }
}
