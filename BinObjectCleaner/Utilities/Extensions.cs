using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace BinObjectCleaner.Utilities
{
    public static class Extensions
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject dependecyObject) where T : DependencyObject
        {
            if (dependecyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependecyObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependecyObject, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
