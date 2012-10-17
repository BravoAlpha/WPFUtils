using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace WPFUtils.Behaviors
{
    public class DynamicGridFormationBehavior : Behavior<Grid>
    {
        public static readonly DependencyProperty FormationsProperty =
            DependencyProperty.Register("Formations",
                                        typeof(DynamicGridFormationCollection),
                                        typeof(DynamicGridFormationBehavior),
                                        new PropertyMetadata(new DynamicGridFormationCollection(), OnFormationsChanged));

        public DynamicGridFormationCollection Formations
        {
            get { return (DynamicGridFormationCollection)GetValue(FormationsProperty); }
            set { SetValue(FormationsProperty, value); }
        }

        private static void OnFormationsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (DynamicGridFormationBehavior)obj;
            behavior.RearrangeChildren();
        }

        public static readonly DependencyProperty NumberOfVisibleChildrenProperty =
            DependencyProperty.Register("NumberOfVisibleChildren",
                                        typeof (int),
                                        typeof (DynamicGridFormationBehavior),
                                        new PropertyMetadata(1, OnVisibleChildrenCountChanged));

        public int NumberOfVisibleChildren
        {
            get { return (int) GetValue(NumberOfVisibleChildrenProperty); }
            set { SetValue(NumberOfVisibleChildrenProperty, value); }
        }

        private static void OnVisibleChildrenCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (DynamicGridFormationBehavior) obj;
            behavior.RearrangeChildren();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            Action action = RearrangeChildren;
            Application.Current.Dispatcher.BeginInvoke(action, DispatcherPriority.Loaded);
        }

        private void RearrangeChildren()
        {
            if (AssociatedObject == null)
                return;

            DynamicGridFormation formation = GetFormationFor(NumberOfVisibleChildren);
            UpdateGridLayout(formation);
        }

        private DynamicGridFormation GetFormationFor(int visibleChildren)
        {
            DynamicGridFormation mathingFormation = Formations.FirstOrDefault(layout => layout.NumberOfChildren == visibleChildren);
            if (mathingFormation != null)
                return mathingFormation;

            string message = String.Format("No grid formation for {0} visible children", visibleChildren);
            Trace.WriteLine(message);
            return null;
        }

        private void UpdateGridLayout(DynamicGridFormation formation)
        {
            if (formation == null)
                return;

            AssociatedObject.RowDefinitions.Clear();
            for (int rowCount = 0; rowCount < formation.NumberOfRows; ++rowCount)
                AssociatedObject.RowDefinitions.Add(new RowDefinition());

            AssociatedObject.ColumnDefinitions.Clear();
            for (int columnCount = 0; columnCount < formation.NumberOfColumns; ++columnCount)
                AssociatedObject.ColumnDefinitions.Add(new ColumnDefinition());

            if (AssociatedObject.Children == null)
                return;

            for (int childIndex = 0; childIndex < AssociatedObject.Children.Count; ++childIndex)
            {
                DynamicGridChildProperties childProperties = formation.GetChildPropertiesByIndex(childIndex);
                UIElement child = AssociatedObject.Children[childIndex];
                SetChildProperties(child, childProperties);
            }
        }

        private static void SetChildProperties(UIElement child, DynamicGridChildProperties properties)
        {
            child.SetValue(Grid.RowProperty, properties.Row);
            child.SetValue(Grid.ColumnProperty, properties.Column);
            child.SetValue(Grid.RowSpanProperty, properties.RowSpan);
            child.SetValue(Grid.ColumnSpanProperty, properties.ColumnSpan);
            child.Visibility = properties.Visibility;
        }
    }
}