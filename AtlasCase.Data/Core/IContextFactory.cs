using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Core.EntityFramework;

namespace AtlasCase.Data.Core
{
    public interface IContextFactory
    {
        EfContext Create();
    }
}
