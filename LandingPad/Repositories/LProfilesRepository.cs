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
    public class LProfilesRepository : Repository<LPProfile>, ILProfilesRepository
    {

        public LProfilesRepository(LandingPadContext context) : base(context)
        {

        }

        public LPProfile GetLProlfile(int id)
        {
            return LandingPadContext.LPProfiles.Where(u => u.UserID == id).FirstOrDefault();
        }

        public LandingPadContext LandingPadContext
        {
            get { return Context as LandingPadContext; }
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Remove(int id)
        {
            Context.LPProfiles.Remove(LandingPadContext.LPProfiles.Where(u => u.UserID == id).SingleOrDefault());
        }
    }
}