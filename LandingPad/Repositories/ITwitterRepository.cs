using LandingPad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandingPad.Repositories
{
    public interface ITwitterRepository : IRepository<Twitter>
    {
        bool FindTwitter(int id);

        int GetTwitterId(int id);

        String GetTwitterTag(int id);

        DateTime GetTwitterEndTime(int id);

        void Save();

        void Remove(int id);
    }
}