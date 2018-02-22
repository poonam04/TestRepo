using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    [Table("UsersData")]
    public class UsersDataEntity
    {
        [Key]
        [Column("UserID", Order = 1, TypeName = "int")]
        public int ID { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "First Name can't be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last name can't be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Id")]
        [MaxLength(100)]
        public string EmailID { get; set; }

      
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public bool IsDeleted { get; set; }
              
        public DateTime? LastModified { get; set; }
    }
}
