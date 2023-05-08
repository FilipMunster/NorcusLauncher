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
        public MainWindow()
        {
            InitializeComponent();
            modeComboBox.ItemsSource = Enum.GetValues(typeof(NorcusLauncher.Clients.ClientInfo.Mode)).Cast<NorcusLauncher.Clients.ClientInfo.Mode>();
            ((MainViewModel)DataContext).DataGridChanged += MainWindow_DataGridChanged;
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
    }
}
