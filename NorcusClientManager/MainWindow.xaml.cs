using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NorcusClientManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon _tbi;
        private MainViewModel _dataContext;
        private ICommand _tbiDblCLickCommand;
        private ICommand _TbiDblClickCommand => _tbiDblCLickCommand ??= new RelayCommand<object>((o) => _ShowHideWindow());
        public MainWindow()
        {
            InitializeComponent();
            _tbi = (TaskbarIcon)FindResource("NCMNotifyIcon");
            _tbi.DoubleClickCommand = _TbiDblClickCommand;
            _tbi.LeftClickCommand = _TbiDblClickCommand;
            modeComboBox.ItemsSource = Enum.GetValues(typeof(NorcusLauncher.Clients.ClientInfo.Mode)).Cast<NorcusLauncher.Clients.ClientInfo.Mode>();
            _dataContext = (MainViewModel)DataContext;
            _dataContext.DataGridChanged += MainWindow_DataGridChanged;
            if (((MainViewModel)DataContext).StartInTray) NCMWindow.Visibility = Visibility.Hidden;
        }

        private void MainWindow_DataGridChanged(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() => 
            {
                clientsDataGrid.CommitEdit();
                clientsDataGrid.CommitEdit();
                clientsDataGrid.Items.Refresh();
            });
        }
        private void _ShowHideWindow()
        {
            NCMWindow.Visibility = NCMWindow.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            if (NCMWindow.Visibility == Visibility.Visible)
            {
                _dataContext.SetClientsToBottom();
            }
        }
        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {
            KeyPasswordBox.Visibility = KeyPasswordBox.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            _dataContext.SetClientsToBottom();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            _dataContext.SetClientsTopMost();
        }
    }
}
