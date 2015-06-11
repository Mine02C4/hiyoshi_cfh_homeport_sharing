using Livet;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Navigation;

namespace HiyoshiCfhClient.ViewModels
{
    class LoginViewModel : ViewModel
    {
        public void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var uri = e.Uri;
            var accessToken = getParamFromFragment(uri.Fragment, "access_token");
            var tokenType = getParamFromFragment(uri.Fragment, "token_type");
            if (uri.DnsSafeHost == "hiyoshicfhweb.azurewebsites.net" && accessToken != null && tokenType != null)
            {
                // TODO: Tokenを保管
                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
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
