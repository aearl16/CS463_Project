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
        [Display(Name ="Record #")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Display(Name ="Athlete")]
        public int AthleteID { get; set; }

        [Display(Name ="Workout Type")]
        public int WorkoutID { get; set; }

        [StringLength(64)]
        [Display(Name ="Time")]
        public string WorkoutTime { get; set; }

        public double? Distance { get; set; }

        public int? Steps { get; set; }

        [Display(Name ="Heart Rate")]
        public int? HeartRate { get; set; }

        [Display(Name ="Date")]
        public DateTime WorkoutDate { get; set; }

        [StringLength(255)]
        [Display(Name = "GPS Log")]
        public string GPSLog { get; set; }

        public virtual Athlete Athlete { get; set; }

        public virtual Workout Workout { get; set; }
    }
}
