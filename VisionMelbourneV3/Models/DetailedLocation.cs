namespace VisionMelbourneV3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailedLocation")]
    public partial class DetailedLocation
    {
        public int Id { get; set; }

        [Required]
        public string Location { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [StringLength(50)]
        public string PeopleCount { get; set; }

        [StringLength(50)]
        public string Tactile { get; set; }

        [StringLength(10)]
        public string Weather { get; set; }

        public DateTime? Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Theme { get; set; }

        [Required]
        [StringLength(50)]
        public string Radius { get; set; }

        public string AccessibilityLevel { get; set; }

        public string AccessibilityRating { get; set; }


        public string StartLocation { get; set; }
    }

}
