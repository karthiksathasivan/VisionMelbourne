namespace VisionMelbourneV3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TactileGround
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string assetnumber { get; set; }

        public string Description { get; set; }

        public string RoadSegment { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
