using Livet;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Navigation;

namespace HiyoshiCfhClient.ViewModels
{
    class LoginViewModel : ViewModel
    {
        #region AccessToken変更通知プロパティ
        private string _AccessToken;

        public string AccessToken
        {
            get
            { return _AccessToken; }
            set
            {
                if (_AccessToken == value)
                    return;
                _AccessToken = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TokenType変更通知プロパティ
        private string _TokenType;

        public string TokenType
        {
            get
            { return _TokenType; }
            set
            {
                if (_TokenType == value)
                    return;
                _TokenType = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public async void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var uri = e.Uri;
            var accessToken = getParamFromFragment(uri.Fragment, "access_token");
            var tokenType = getParamFromFragment(uri.Fragment, "token_type");
            if (uri.DnsSafeHost == "hiyoshicfhweb.azurewebsites.net" && accessToken != null && tokenType != null)
            //if (uri.AbsoluteUri == "http://hiyoshicfhweb.azurewebsites.net/")
            {
                AccessToken = accessToken;
                TokenType = tokenType;
                await Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
                });
            }
        }

        private static string getParamFromFragment(string fragment, string key)
        {
            var regex = new Regex("[\\?#&]" + key + "=([^&#]*)");
            var m = regex.Match(fragment);
            if (m.Success)
            {
                return HttpUtility.UrlDecode(Regex.Replace(m.Groups[1].Value, @"\+", " "));
            }
            else
            {
                return null;
            }
        }
    }
}
