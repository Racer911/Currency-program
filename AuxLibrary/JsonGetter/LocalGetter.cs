using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxLibrary
{
    public class LocalGetter : IJsonGetter<string>
    {
        public string Path { get; }

        public LocalGetter(string path)
        {
            Path = path;
        }

        public string GetJson()
        {
            return IsExist ? File.ReadAllText(Path, Encoding.UTF8) : string.Empty;
        }

        private bool IsExist => File.Exists(Path);
    }
}
