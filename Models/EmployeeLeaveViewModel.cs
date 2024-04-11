namespace RoxCorp.Models
{
    public class EmployeeLeaveViewModel
    {
        public string selectedLeave { get; set; }
        public IEnumerable<EmployeeWithLeaveType> Employees { get; set; }
        public IEnumerable<Leave> Leaves { get; set; }
    }
}
