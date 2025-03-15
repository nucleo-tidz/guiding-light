using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Agents
{
    public interface IClassifierAgent
    {        Task<string> Classify(string query);
    }
}
