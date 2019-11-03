using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxLibrary
{
    public class Deserializator
    {
        public static T Deserialize<T>(string json)
        {
           return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
