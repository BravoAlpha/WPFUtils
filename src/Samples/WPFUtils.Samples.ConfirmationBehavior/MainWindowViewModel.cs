using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace WPFUtils.Samples.ConfirmationBehavior
{
    public class MainWindowViewModel : NotificationObject
    {
        private string m_info;

        public string ConfirmationMessage
        {
            get { return "Message Here..."; }
        }

        public string ConfirmationCaption
        {
            get { return "Caption Here..."; }
        }

        public string Info
        {
            get { return m_info; }
            set
            {
                if (m_info == value)
                    return;

                m_info = value;

                RaisePropertyChanged(() => Info);
            }
        }

        public DelegateCommand FirstCommand { get; private set; }
        public DelegateCommand SecondCommand { get; private set; }

        public MainWindowViewModel()
        {
            FirstCommand = new DelegateCommand(() => Info = "First Command Executed!");
            SecondCommand = new DelegateCommand(() => Info = "Second Command Executed!");
        }
    }
}