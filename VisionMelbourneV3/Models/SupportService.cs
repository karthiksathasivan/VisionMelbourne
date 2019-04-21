namespace VisionMelbourneV3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SupportService
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        public string Website { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string Coordinates { get; set; }
    }
}
