using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Service
{
    public interface IBibleService
    {
        public Task<(string, bool)> GetVerse(string query);
    }
}
