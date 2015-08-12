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
using MetroRadiance.Controls;
using System.Text.RegularExpressions;
using System.Web;
using HiyoshiCfhClient.ViewModels;

namespace HiyoshiCfhClient.Views
{
    /// <summary>
    /// LoginDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();
            MetroTrilithon.Controls.WebBrowserHelper.SetScriptErrorsSuppressed(this.WebBrowser, true);
            MetroTrilithon.Controls.WebBrowserHelper.SetAllowWebBrowserDrop(this.WebBrowser, false);
        }
    }
}
