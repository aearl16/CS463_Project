namespace HelloWorld.Models
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
        [Display(Name = "Record ID")]
        public int ID { get; set; }

        [Display(Name = "Athlete ID")]
        public int AthleteID { get; set; }

        [Display(Name = "Workout ID")]
        public int WorkoutID { get; set; }

        [StringLength(30)]
        [Display(Name = "Workout Time")]
        public string WorkoutTime { get; set; }

        [Display(Name = "Run Distance")]
        public double? Distance { get; set; }

        [Display(Name = "Step Count")]
        public int? Steps { get; set; }

        [Display(Name = "Heart Rate")]
        public int? HeartRate { get; set; }

        [Display(Name = "Workout Date")]
        public DateTime WorkoutDate { get; set; }

        [StringLength(255)]
        [Display(Name = "GPS Log")]
        public string GPSLog { get; set; }

        public virtual Athlete Athlete { get; set; }

        public virtual Workout Workout { get; set; }
    }
}
