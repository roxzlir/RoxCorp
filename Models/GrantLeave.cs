using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RoxCorp.Models
{
    public class GrantLeave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GrantLeaveId { get; set; }
        [Required]
        [ForeignKey("ApplyForLeave")]
        public int FkApplyForLeaveId { get; set; }
        public ApplyForLeave? ApplyForLeave { get; set; }
        [Required]
        public bool Granted { get; set; }
        [Required(ErrorMessage ="Date is required")]
        [DisplayName("Decision Date")]
        public DateTime DecisionDate { get; set; }

         
    }
}
