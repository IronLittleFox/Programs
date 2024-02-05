using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsMaui.Utils
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollectio<T>(this IEnumerable<T> collection)
        {
            ObservableCollection<T> observableCollection = new ObservableCollection<T>();
            foreach (var item in collection)
            {
                observableCollection.Add(item);
            }
            return observableCollection;
        }

        public static void ForAll<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}
