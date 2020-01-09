using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace Personal_App.Styles
{
    public class ListBoxAssistEx
    {
        static ListBoxAssistEx()
        {
            EventManager.RegisterClassHandler(typeof(ListBox), UIElement.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(ListBoxMouseButtonEvent));
        }

        private static void ListBoxMouseButtonEvent(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var senderElement = (UIElement)sender;

            if (!GetIsToggle(senderElement)) return;

            var point = mouseButtonEventArgs.GetPosition(senderElement);
            var result = VisualTreeHelper.HitTest(senderElement, point);

            if (result == null) return;

            ListBoxItem listBoxItem = null;
            Ripple ripple = null;
            foreach (var dependencyObject in result.VisualHit.GetVisualAncestry().TakeWhile(_ => listBoxItem == null))
            {
                listBoxItem = dependencyObject as ListBoxItem;
                if (ripple == null)
                    ripple = dependencyObject as Ripple;
            }

            if (listBoxItem == null) return;

            if (listBoxItem.IsSelected == true) return;

            listBoxItem.SetCurrentValue(ListBoxItem.IsSelectedProperty, !listBoxItem.IsSelected);
            mouseButtonEventArgs.Handled = true;

            if (ripple != null && listBoxItem.IsSelected)
            {
                ripple.RaiseEvent(new MouseButtonEventArgs(mouseButtonEventArgs.MouseDevice, mouseButtonEventArgs.Timestamp, mouseButtonEventArgs.ChangedButton)
                { RoutedEvent = Control.PreviewMouseLeftButtonDownEvent, Source = ripple }
                );
            }
        }

        public static readonly DependencyProperty IsToggleProperty = DependencyProperty.RegisterAttached(
            "IsToggle", typeof(bool), typeof(ListBoxAssistEx), new FrameworkPropertyMetadata(default(bool)));

        public static void SetIsToggle(DependencyObject element, bool value)
        {
            element.SetValue(IsToggleProperty, value);
        }

        public static bool GetIsToggle(DependencyObject element)
        {
            return (bool)element.GetValue(IsToggleProperty);
        }
    }

    internal static class Extensions
    {
        public static IEnumerable<DependencyObject> VisualDepthFirstTraversal(this DependencyObject node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            yield return node;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);
                foreach (var descendant in child.VisualDepthFirstTraversal())
                {
                    yield return descendant;
                }
            }
        }

        public static IEnumerable<DependencyObject> VisualBreadthFirstTraversal(this DependencyObject node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);
                yield return child;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);

                foreach (var descendant in child.VisualDepthFirstTraversal())
                {
                    yield return descendant;
                }
            }
        }

        public static bool IsAncestorOf(this DependencyObject parent, DependencyObject node)
        {
            return node != null && parent.VisualDepthFirstTraversal().Contains(node);
        }

        /// <summary>
        /// Returns full visual ancestry, starting at the leaf.
        /// </summary>
        /// <param name="leaf"></param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetVisualAncestry(this DependencyObject leaf)
        {
            while (leaf != null)
            {
                yield return leaf;
                leaf = VisualTreeHelper.GetParent(leaf);
            }
        }

        public static IEnumerable<DependencyObject> GetLogicalAncestry(this DependencyObject leaf)
        {
            while (leaf != null)
            {
                yield return leaf;
                leaf = LogicalTreeHelper.GetParent(leaf);
            }
        }

        public static bool IsDescendantOf(this DependencyObject leaf, DependencyObject ancestor)
        {
            DependencyObject parent = null;
            foreach (var node in leaf.GetVisualAncestry())
            {
                if (Equals(node, ancestor))
                    return true;

                parent = node;
            }

            return parent.GetLogicalAncestry().Contains(ancestor);
        }
    }
}
