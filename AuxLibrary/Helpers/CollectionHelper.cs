using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currency
{
    public static class CollectionHelper
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> current)
        {
            return current != null ? new ObservableCollection<T>(current) : new ObservableCollection<T>();
        }
    }
}
