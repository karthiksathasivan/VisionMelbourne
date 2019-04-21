namespace VisionMelbourneV3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SensorLocations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string sensor_id { get; set; }

        [Required]
        public string latitude { get; set; }

        [Required]
        public string longitude { get; set; }
    }
}
