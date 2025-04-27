using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;
using System;
using System.Collections.Generic;
using Camp_rating.Models;

namespace Camp_rating.Models
{
    public class Campsite
    {
            public int Id { get; set; }
            [StringLength(64)]
            public string Name { get; set; }
            [StringLength(255)]
            public string Description { get; set; }

            //coordinates
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string ImagePath { get; set; }

    }
}
