using LandingPad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandingPad.Repositorys
{
    public interface ITwitterRepository : IRepository<Twitter>
    {
        int GetTwitterId(int id);

        String GetTwitterTag(int id);

        DateTime GetTwitterEndTime(int id);

        void Save();

        void Remove(int id);
    }
}