using Grabacr07.KanColleViewer.Composition;
using HiyoshiCfhClient.ViewModels;
using HiyoshiCfhClient.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HiyoshiCfhClient
{
    [Export(typeof(IPlugin))]
    [Export(typeof(ITool))]
    [ExportMetadata("Title", "日吉連合艦隊司令部クライアント")]
    [ExportMetadata("Description", "日吉連合艦隊司令部のシステムを利用するためのクライアントです。")]
    [ExportMetadata("Version", "1.0")]
    [ExportMetadata("Author", "@mine_studio")]
    [ExportMetadata("Guid", "673349DA-5502-4C0F-B2D5-B1284B347B6E")]
    public class HiyoshiCfhClient : IPlugin, ITool
    {
        private readonly ClientViewModel cvm = new ClientViewModel();
        private ClientView _View;
        private Window BackgroundParent;

        public object View
        {
            get { return _View; }
        }

        public string Name
        {
            get { return "日吉連合艦隊司令部"; }
        }

        public void Initialize()
        {
            _View = new ClientView { DataContext = cvm };
            _View.Unloaded += _View_Unloaded;
            _View.Loaded += _View_Loaded;
            BackgroundParent = new Window
            {
                Height = 0,
                Width = 0,
                Top = 200,
                Left = 200,
                WindowStartupLocation = WindowStartupLocation.Manual,
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false,
                Background = Brushes.Transparent,
                AllowsTransparency = true
            };
            BackgroundParent.Content = _View;
        }

        private void _View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            cvm.OutDebugConsole("_View_Loaded");
            BackgroundParent.Content = null;
        }

        private void _View_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            cvm.OutDebugConsole("_View_Unloaded");
            BackgroundParent.Content = _View;
        }
    }
}
