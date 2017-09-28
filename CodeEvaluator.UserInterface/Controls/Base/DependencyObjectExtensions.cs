using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CodeEvaluator.UserInterface.Controls.Base
{
    public static class DependencyObjectExtensions
    {
        #region Public Methods and Operators

        public static IEnumerable<T> FindLogicalChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            List<DependencyObject> dependencyObjects =
                LogicalTreeHelper.GetChildren(obj).Cast<DependencyObject>().ToList();
            foreach (DependencyObject child in dependencyObjects)
            {
                if (child is T)
                {
                    yield return (T)child;
                }

                foreach (T childOfChild in FindLogicalChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            return FindParent<T>(parentObject);
        }

        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                {
                    yield return (T)child;
                }

                foreach (T childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        #endregion
    }
}