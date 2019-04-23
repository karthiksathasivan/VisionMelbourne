namespace VisionMelbourneV3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string Theme { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Latitude { get; set; }

        [Required]
        [StringLength(50)]
        public string Longitude { get; set; }

        public string AccessibilityLevel { get; set; }

        [StringLength(10)]
        public string AccessibilityRating { get; set; }
    }
}
