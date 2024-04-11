using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RoxCorp.Models
{
    public class Leave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveId { get; set; }
        [Required(ErrorMessage = "Please specify which type of leave you would like to add")]
        [DisplayName("Type of leave")]
        [StringLength(35, ErrorMessage = "Type can only have max 35 characters")]
        public string LeaveType { get; set; }
        public IList<ApplyForLeave>? ApplyForLeave { get; set; }


    }
}
