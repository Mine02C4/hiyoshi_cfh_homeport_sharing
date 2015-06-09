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

namespace HiyoshiCfhClient
{
    /// <summary>
    /// ClientView.xaml の相互作用ロジック
    /// </summary>
    public partial class ClientView : UserControl
    {
        public ClientView()
        {
            InitializeComponent();
        }

        private async void Send(object sender, RoutedEventArgs e)
        {
            string baseUri = TargetHost.Text;
            // TODO: 送信処理の実装
            Client client = new Client(baseUri);
            DebugConsole.Text += await client.CollectShipsData();
        }

        private async void GetShipTypes(object sender, RoutedEventArgs e)
        {
            Client client = new Client();
            DebugConsole.Text += await client.GetShipTypes();
        }
    }
}
