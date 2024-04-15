namespace RoxCorp.Models
{
    public class EmployeeNameLeaveInfoViewModel
    {
        public string selectedEmployee { get; set; }
        public IEnumerable<EmployeeNameLeaveInfo> Leaves { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
