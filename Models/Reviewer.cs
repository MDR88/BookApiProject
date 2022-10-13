using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Used for [key] notation
using System.ComponentModel.DataAnnotations.Schema; // Used for [DatabaseGenerated] notation
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Models
{
    public class Reviewer
    {
        [Key]  //Key Annotation to create primary key column for property whos name is Id.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Explicitly has Entity Framework Auto Generate ID, not by user
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "First Name must be up to 100 characters in length")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Last Name cannot be more than 200 characters in length")]
        public string LastName { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }  // The Reviewer will have multiple reviews
    }
}
