using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aurochses.Data.Extensions.MsSql;
using Microsoft.AspNetCore.Identity;

namespace Aurochses.AspNetCore.Identity.EntityFrameworkCore
{
    /// <summary>
    /// User
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>, IApplicationUser
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <value>The first name.</value>
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        /// <value>The last name.</value>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Registration date
        /// </summary>
        /// <value>The date of user registration.</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = ColumnTypes.DateTime)]
        public DateTime CreatedDate { get; set; }
    }
}