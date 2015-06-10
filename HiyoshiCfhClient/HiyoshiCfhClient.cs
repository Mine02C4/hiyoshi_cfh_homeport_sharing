using Grabacr07.KanColleViewer.Composition;
using HiyoshiCfhClient.ViewModels;
using HiyoshiCfhClient.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiyoshiCfhClient
{
    [Export(typeof(IToolPlugin))]
    [ExportMetadata("Title", "日吉連合艦隊司令部クライアント")]
    [ExportMetadata("Description", "日吉連合艦隊司令部のシステムを利用するためのクライアントです。")]
    [ExportMetadata("Version", "1.0")]
    [ExportMetadata("Author", "@mine_studio")]
    public class HiyoshiCfhClient : IToolPlugin
    {
        private readonly ClientViewModel cvm = new ClientViewModel();
        public object GetToolView()
        {
            return new ClientView { DataContext = cvm };
        }

        public string ToolName
        {
            get { return "日吉連合艦隊司令部"; }
        }

        public object GetSettingsView()
        {
            return null;
        }
    }
}
