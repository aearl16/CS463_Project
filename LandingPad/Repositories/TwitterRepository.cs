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
    public class TwitterRepository : Repository<Twitter>, ITwitterRepository
    {

        public TwitterRepository(LandingPadContext context) : base(context)
        {

        }

        public bool FindTwitter(int id)
        {
            int Exists = LandingPadContext.Twitters.Where(u => u.UserID == id).SingleOrDefault().UserID;

            if (Exists == id)
            {
                return true;
            }else{
                return false;
            }
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