namespace LandingPad.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProfileUser
    {
        public LPProfile LPProfile { get; set; }
        public LPUser LPUser { get; set; }
    }
}