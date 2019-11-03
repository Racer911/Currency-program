using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuxLibrary
{
    public class InternetGetter : IJsonGetter<string>
    {
        public string Path { get; }

        public InternetGetter(string path)
        {
            Path = path;
        }

        public string GetJson()
        {
            var wc = new WebClient() { Encoding = Encoding.UTF8 };
            try
            {
                return wc.DownloadString(Path);
            }
            catch (WebException e)
            {
                throw new WebException("WebClient is down");
            }
            finally
            {
                wc.Dispose();
            }
        }
    }
}
