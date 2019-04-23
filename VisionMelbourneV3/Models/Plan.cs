namespace VisionMelbourneV3.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Plan : DbContext
    {
        public Plan()
            : base("name=Plan2")
        {
        }

        public virtual DbSet<DetailedLocation> DetailedLocations { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Pedcount> Pedcounts { get; set; }
        public virtual DbSet<SensorLocation> SensorLocations { get; set; }
        public virtual DbSet<SupportService> SupportServices { get; set; }
        public virtual DbSet<TactileGround> TactileGrounds { get; set; }
        public virtual DbSet<UserPlan> UserPlans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailedLocation>()
                .Property(e => e.Weather)
                .IsFixedLength();

            modelBuilder.Entity<UserPlan>()
                .Property(e => e.Weather)
                .IsFixedLength();
        }
    }
}
