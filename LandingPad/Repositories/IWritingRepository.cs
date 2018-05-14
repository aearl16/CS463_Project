using LandingPad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandingPad.Repositories
{
    public interface IWritingRepository : IRepository<Writing>
    {
        Writing GetWriting(int id);

        void Save();

        void Remove(int id);
    }
}