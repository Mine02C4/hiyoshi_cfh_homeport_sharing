﻿using Grabacr07.KanColleViewer.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hiyoshi_cfh_client
{
    [Export(typeof(IToolPlugin))]
    [ExportMetadata("Title", "日吉連合艦隊司令部クライアント")]
    [ExportMetadata("Description", "日吉連合艦隊司令部のシステムを利用するためのクライアントです。")]
    [ExportMetadata("Version", "1.0")]
    [ExportMetadata("Author", "@mine_studio")]
    public class HiyoshiCfhClient : IToolPlugin
    {
        public object GetToolView()
        {
            return new ClientView();
        }

        public string ToolName
        {
            get { return "日吉"; }
        }

        public object GetSettingsView()
        {
            return null;
        }
    }
}
