using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuxLibrary
{
    public class JsonGetterFactory
    {
        public IJsonGetter<T> Getter<T>(T path)
        {
            if (path is string temp1 && temp1.Contains("http"))
                return (IJsonGetter<T>)new InternetGetter(temp1);

            else if (path is string temp2 && Regex.IsMatch(temp2, "([a-zA-Z]*:[\\[a-zA-Z0-9 .]*]*)"))
                return (IJsonGetter<T>)new LocalGetter(temp2);

            else if (path is Int32 temp3)
                return (IJsonGetter<T>)new DataBaseGetter(temp3);

            else
                throw new ArgumentException("Invalid path");
        }

    }
}
