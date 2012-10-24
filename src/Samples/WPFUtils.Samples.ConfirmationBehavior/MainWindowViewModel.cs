using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace WPFUtils.Samples.ConfirmationBehavior
{
    public class MainWindowViewModel : NotificationObject
    {
        private string m_info;

        public string ConfirmationMessage
        {
            get { return "Are you sure..."; }
        }

        public string ConfirmationCaption
        {
            get { return "Please Confirm..."; }
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

        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand ActivateCommand { get; private set; }

        public MainWindowViewModel()
        {
            DeleteCommand = new DelegateCommand(() => Info = "Delete Command Executed!");
            ActivateCommand = new DelegateCommand(() => Info = "Activate Command Executed!");
        }
    }
}