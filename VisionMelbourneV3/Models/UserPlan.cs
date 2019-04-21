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

        public string UserID { get; set; }

        public DateTime Date { get; set; }
    }
}
