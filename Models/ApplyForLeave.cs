using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoxCorp.Models
{
    public class ApplyForLeave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplyForLeaveId { get; set; }
        [Required]
        [ForeignKey("Leave")]
        public int FkLeaveId { get; set; }
        public Leave? Leave { get; set; }
        [Required]
        [ForeignKey("Employee")]
        public int FkEmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime ApplyFromDate { get; set; }
        public DateTime ApplyToDate { get; set;}
        [DisplayName("Note")]
        [StringLength(200, ErrorMessage = "Note can only have max 200 characters")]
        public string? ApplyNote { get; set; }
        public DateTime ApplyRegisteredDate {  get; set; }
    }
}
