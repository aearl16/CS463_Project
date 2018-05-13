using LandingPad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandingPad.Repositories
{
    public interface ILProfilesRepository : IRepository<LPProfile>
    {
        LPProfile GetLProlfile(int id);

        void Save();

        void Remove(int id);
    }
}