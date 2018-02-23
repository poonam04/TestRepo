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

    [Table("ImageData")]
    public class ImageDataEntity
    {
        [Key]
        [Column("ImageId", Order = 1, TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual string ImageName { get; set; }
        public virtual string DataResponse { get; set; }
        public virtual string Query { get; set; }
        public virtual byte[] Image { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual bool IsLive { get; set; }
    }
    [Table("TagData")]
    public class ImageTagEntity
    {
        [Key]
        [Column("ImageTagId", Order = 1, TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual int ImageId { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual bool IsLive { get; set; }
    }
}
