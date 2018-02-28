namespace LandingPad.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using LandingPad.DAL;

    public class ProfileUser
    {
        public List<LPProfile> LPProfile { get; set; }
        public List<LPUser> LPUser { get; set; }
        public List<Pseudonym> Pseudonym { get; set; }
    }
}