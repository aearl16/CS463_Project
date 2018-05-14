using LandingPad.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandingPad.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LandingPadContext _context;

        public UnitOfWork(LandingPadContext context)
        {
            _context = context;
            Twitter = new TwitterRepository(_context);
        }

        public ITwitterRepository Twitter { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}