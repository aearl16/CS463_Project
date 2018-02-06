namespace HelloWorld.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Record")]
    public partial class Record
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int AthleteID { get; set; }

        public int WorkoutID { get; set; }

        [StringLength(64)]
        public string WorkoutTime { get; set; }

        public double? Distance { get; set; }

        public int? Steps { get; set; }

        public int? HeartRate { get; set; }

        public DateTime WorkoutDate { get; set; }

        [StringLength(255)]
        public string GPSLog { get; set; }

        public virtual Athlete Athlete { get; set; }

        public virtual Workout Workout { get; set; }
    }
}
