using MetroRadiance.UI.Controls;

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
            MetroTrilithon.UI.Controls.WebBrowserHelper.SetScriptErrorsSuppressed(this.WebBrowser, true);
            MetroTrilithon.UI.Controls.WebBrowserHelper.SetAllowWebBrowserDrop(this.WebBrowser, false);
        }
    }
}
