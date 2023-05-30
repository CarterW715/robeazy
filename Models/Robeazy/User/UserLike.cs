using RobeazyCore.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public class UserLike
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public UserLike()
        {
            Enabled = true;
            CreateDate = DateTime.UtcNow;
        }

        public UserLike(ApplicationUser user)
        {
            Enabled = true;
            User = user;
            CreateDate = DateTime.UtcNow;
        }
    }
}