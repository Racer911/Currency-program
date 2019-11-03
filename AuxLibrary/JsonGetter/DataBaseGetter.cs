using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxLibrary
{
    public class DataBaseGetter : IJsonGetter<int>
    {
        public int Path { get; }

        public DataBaseGetter(int path)
        {
            Path = path;
        }

        public string GetJson()
        {
            throw new NotImplementedException();
        }
    }
}
