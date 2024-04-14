namespace RoxCorp.Models
{
    public class EmployeeWithLeaveType
    {
        public string EmployeeName { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeEmail { get; set; }
        public string ApplyNote { get; set; }
        public DateTime ApplyRegisteredDate { get; set; }
        public string LeaveType { get; set; }
        public bool Granted { get; set; }
    }
}
