using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesBOs
{
    public partial class Member
    {
        public Member()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int MemberId { get; set; }

        [Required]
        public string Email { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
