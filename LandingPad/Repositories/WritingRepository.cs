using LandingPad.DAL;
using LandingPad.Models;
using System;
using LandingPad.Repositories;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandingPad.Repositories
{
    public class WritingRepository : Repository<Writing>, IWritingRepository
    {

        public WritingRepository(LandingPadContext context) : base(context)
        {

        }

        public Writing GetWriting(int id)
        {
            return LandingPadContext.Writings.Where(u => u.ProfileID == id).FirstOrDefault();
        }

        public LandingPadContext LandingPadContext
        {
            get { return Context as LandingPadContext; }
        }

        public void Remove(int id)
        {
            Context.Writings.Remove(LandingPadContext.Writings.Where(u => u.ProfileID == id).SingleOrDefault());
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}