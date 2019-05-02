namespace VisionMelbourneV3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserPlan")]
    public partial class UserPlan
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [StringLength(50)]
        public string PeopleCount { get; set; }

        [StringLength(50)]
        public string Tactile { get; set; }

        [StringLength(10)]
        public string Weather { get; set; }

        public string UserID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public string StartLocation { get; set; }

        [StringLength(50)]
        public string StartWeather { get; set; }

        public TimeSpan? Time { get; set; }

        [StringLength(50)]
        public string AccessibilityLevel { get; set; }

        [StringLength(50)]
        public string AccessibilityRating { get; set; }
    }
}
