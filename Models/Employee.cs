using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RoxCorp.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        [StringLength(65, ErrorMessage = "Name can only have max 65 characters")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        [StringLength(175, ErrorMessage = "Email can only have max 175 characters")]
        public string EmployeeEmail { get; set; }
        [DisplayName("Phone number")]
        [StringLength(20, ErrorMessage = "Phone number can only have max 20 characters")]
        public string? EmployeePhone { get; set; }
        public IList<ApplyForLeave>? ApplyForLeave { get; set; }
    }
}
