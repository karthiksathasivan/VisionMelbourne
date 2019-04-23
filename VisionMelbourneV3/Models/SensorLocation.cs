namespace VisionMelbourneV3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SensorLocation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        public string sensor_id { get; set; }

        [Required]
        [StringLength(50)]
        public string latitude { get; set; }

        [Required]
        [StringLength(50)]
        public string longitude { get; set; }
    }
}
