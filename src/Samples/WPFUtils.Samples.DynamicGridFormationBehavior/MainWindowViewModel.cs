using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace WPFUtils.Samples.DynamicGridFormationBehavior
{
    public class MainWindowViewModel : NotificationObject
    {
        private int m_numberOfVisibleItems;

        public ObservableCollection<Item> Items { get; private set; }
        public DelegateCommand<int?> ChangeNumberOfVisibleItemsCommand { get; private set; }

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<Item>
            {   new Item {Name = "1"}, 
                new Item {Name = "2"}, 
                new Item {Name = "3"}, 
                new Item {Name = "4"}
            };

            NumberOfVisibleItems = 4;
            ChangeNumberOfVisibleItemsCommand = new DelegateCommand<int?>(OnExecuteChangeNumberOfVisibleItemsCommand);
        }

        public int NumberOfVisibleItems
        {
            get { return m_numberOfVisibleItems; }
            set
            {
                if (m_numberOfVisibleItems == value)
                    return;

                m_numberOfVisibleItems = value;
                RaisePropertyChanged(() => NumberOfVisibleItems);
            }
        }

        private void OnExecuteChangeNumberOfVisibleItemsCommand(int? numberOfvisibleItems)
        {
            if (numberOfvisibleItems.HasValue)
                NumberOfVisibleItems = numberOfvisibleItems.Value;
        }
    }

    public class Item
    {
        public string Name { get; set; }
    }
}