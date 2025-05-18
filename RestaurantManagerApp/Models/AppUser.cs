﻿using Microsoft.AspNetCore.Identity;

namespace RestaurantManagerApp.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }
    }
}