using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using System;
using System.Collections.Generic;

namespace Camp_rating.Models
{
    public class Review
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;


        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public Campsite Campsite { get; set; } = null!;
    }
}
