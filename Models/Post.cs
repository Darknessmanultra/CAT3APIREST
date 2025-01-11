using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CAT3B.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Title { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public DateTime PublishedAt { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}