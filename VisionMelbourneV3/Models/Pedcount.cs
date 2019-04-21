namespace VisionMelbourneV3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pedcount")]
    public partial class Pedcount
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(3)]
        public string SensorID { get; set; }

        public string SensorName { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [StringLength(50)]
        public string Day { get; set; }

        [StringLength(5)]
        public string Time { get; set; }

        [Column("PedCount")]
        public double? PedCount1 { get; set; }
    }
}
