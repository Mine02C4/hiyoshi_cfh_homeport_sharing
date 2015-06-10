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
            Grabacr07.KanColleViewer.Views.Controls.WebBrowserHelper.SetScriptErrorsSuppressed(this.WebBrowser, true);
            Grabacr07.KanColleViewer.Views.Controls.WebBrowserHelper.SetAllowWebBrowserDrop(this.WebBrowser, false);
            this.WebBrowser.Navigating += WebBrowser_Navigating;
        }

        private void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var uri = e.Uri;
            var accessToken = getParamFromFragment(uri.Fragment, "access_token");
            var tokenType = getParamFromFragment(uri.Fragment, "token_type");
            if (uri.DnsSafeHost == "hiyoshicfhweb.azurewebsites.net" && accessToken != null && tokenType != null)
            {
                // TODO: Tokenを保管
                this.Close();
            }
        }

        private static string getParamFromFragment(string fragment, string key)
        {
            var regex = new Regex("[\\?#&]" + key + "=([^&#]*)");
            var m = regex.Match(fragment);
            if (m.Success)
            {
                return HttpUtility.UrlDecode(Regex.Replace(m.Value, @"\+", " "));
            }
            else
            {
                return null;
            }
        }
    }
}
