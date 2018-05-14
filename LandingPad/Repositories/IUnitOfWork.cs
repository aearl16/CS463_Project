using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandingPad.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITwitterRepository Twitter { get; }
        //add your IRepos here

        int Complete();
    }
}