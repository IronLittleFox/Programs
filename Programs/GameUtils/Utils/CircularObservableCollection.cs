using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUtils.Utils
{
    public class CircularObservableCollection<T> : ObservableCollection<T>
    {
        private IEnumerator<T> enumerator;

        public CircularObservableCollection()
        {
            CollectionChanged += (s, e) =>
            {
                enumerator = GetEnumerator();
                enumerator.MoveNext();
            };
        }

        public T GetNext()
        {
            if (!enumerator.MoveNext())
            {
                enumerator.Reset();
                enumerator.MoveNext();
            }
            return enumerator.Current;
        }

        public void SetCurrent(T value)
        {
            while(true)
            {
                if (!enumerator.MoveNext())
                {
                    enumerator.Reset();
                    enumerator.MoveNext();
                }
                if (enumerator.Current.Equals(value))
                    return;
            }
        }
    }
}
