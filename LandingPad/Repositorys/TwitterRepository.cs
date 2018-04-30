using LandingPad.DAL;
using LandingPad.Models;
using System;
using LandingPad.Repositorys;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandingPad.Repositorys
{
    public class TwitterRepository : Repository<Twitter>, ITwitterRepository
    {

        public TwitterRepository(LandingPadContext context) : base(context)
        {

        }

        public int GetTwitterId(int id)
        {
            return LandingPadContext.Twitters.Where(u => u.UserID == id).SingleOrDefault().UserID;
        }


        public DateTime GetTwitterEndTime(int id)
        {
            return LandingPadContext.Twitters.Where(u => u.UserID == id).SingleOrDefault().EndDate;
        }

        public String GetTwitterTag(int id)
        {
            return LandingPadContext.Twitters.Where(u => u.UserID == id).SingleOrDefault().TwName;
        }

        public LandingPadContext LandingPadContext
        {
            get { return Context as LandingPadContext; }
        }

        public void Remove(int id)
        {

            Context.Twitters.Remove(LandingPadContext.Twitters.Where(u => u.UserID == id).SingleOrDefault());

        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}