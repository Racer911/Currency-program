using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxLibrary
{
    public interface IJsonGetter<T>
    {
        T Path { get; }
        string GetJson();
    }
}
