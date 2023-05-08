using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NorcusClientManager
{
    internal static class ExtensionMethods
    {
        public static void ForEach<T>(this ObservableCollection<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        }
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            ObservableCollection<T> result = new ObservableCollection<T>();
            foreach (var item in collection)
            {
                result.Add(item);
            }
            return result;
        }
    }
}
