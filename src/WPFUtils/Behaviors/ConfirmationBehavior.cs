using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WPFUtils.Behaviors
{
    public class ConfirmationBehavior<T> : Behavior<T> where T : ButtonBase
    {
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof (string), typeof (ConfirmationBehavior<T>));

        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof (string), typeof (ConfirmationBehavior<T>));

        public string Caption
        {
            get { return (string) GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof (ICommand), typeof (ConfirmationBehavior<T>));

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof (object), typeof (ConfirmationBehavior<T>));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OnButtonClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Click -= OnButtonClick;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (Command == null || !Command.CanExecute(CommandParameter))
                return;

            if(!ShouldConfirm())
            {
                OnConfirmed();
                return;
            }

            MessageBoxResult result = MessageBox.Show(Message, Caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                OnConfirmed();
            else
                OnNotConfirmed();
        }

        protected virtual bool ShouldConfirm()
        {
            return true;
        }

        protected virtual void OnConfirmed()
        {
            Command.Execute(CommandParameter);
        }

        protected virtual void OnNotConfirmed() { }
    }

    public class ConfirmationBehavior : ConfirmationBehavior<ButtonBase>
    {
        
    }

    public class ToggleConfirmationBehavior : ConfirmationBehavior<ToggleButton>
    {
        protected override bool ShouldConfirm()
        {
            return AssociatedObject.IsChecked == true;
        }

        protected override void OnNotConfirmed()
        {
            AssociatedObject.IsChecked = false;
        }
    }
}