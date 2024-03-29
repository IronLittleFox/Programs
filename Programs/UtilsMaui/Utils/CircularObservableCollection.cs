﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsMaui.Utils
{
    public class CircularObservableCollection<T> : ObservableCollection<T>
    {
        private IEnumerator<T> enumerator;

        public CircularObservableCollection()
        {
            enumerator = GetEnumerator();
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
            if (enumerator.Current == null)
                return;
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
