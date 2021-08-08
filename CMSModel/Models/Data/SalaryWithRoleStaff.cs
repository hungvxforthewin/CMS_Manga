using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CRMModel.Models.Data
{
    [Table("tblSalaryWithRoleStaff")]
    public class SalaryWithRoleStaff
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }

        public virtual byte RoleAccount { get; set; }

        public virtual byte? TeamSize { get; set; }

        public virtual byte? TotalTeams { get; set; }
        [Required]
        public virtual decimal Salary { get; set; }

        public virtual decimal? SalaryMin { get; set; }

        public virtual byte? TimeProbationary { get; set; }

        public virtual decimal? ProbationarySalary{ get; set; }
        [Required]
        public virtual bool Status { get; set; }
    }
}
