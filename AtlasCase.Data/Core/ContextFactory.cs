using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Core.EntityFramework;
using Microsoft.Extensions.Configuration;

namespace AtlasCase.Data.Core
{
    public class ContextFactory: IContextFactory
    {
       private readonly IConfiguration _configuration;
        public ContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public EfContext Create()
        {
            return new EfContext(_configuration);
        }
    }
}
