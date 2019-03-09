﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace AutoHideTabRename.Utility
{
    internal static class DependencyObjectExtensions
    {
        public static IEnumerable<DependencyObject> Children(this DependencyObject obj)
        {
            if(obj == null) throw new ArgumentNullException(nameof(obj));

            var count = VisualTreeHelper.GetChildrenCount(obj);
            for(var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if(child != null)
                    yield return child;
            }
        }

        public static IEnumerable<DependencyObject> Descendants(this DependencyObject obj)
        {
            if(obj == null) throw new ArgumentNullException(nameof(obj));

            foreach(var child in obj.Children())
            {
                yield return child;

                foreach(var grandChild in child.Descendants())
                    yield return grandChild;
            }
        }

        public static IEnumerable<T> Children<T>(this DependencyObject obj) where T : DependencyObject
            => obj.Children().OfType<T>();

        public static IEnumerable<T> Descendants<T>(this DependencyObject obj) where T : DependencyObject
            => obj.Descendants().OfType<T>();
    }
}
