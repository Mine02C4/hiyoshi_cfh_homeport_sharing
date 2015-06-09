using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiyoshiCfhClient.HiyoshiCfhWeb.Models
{
    public partial class ShipType
    {
        public override string ToString()
        {
            return string.Format("ID = {0}, Name = \"{1}\"", this.ShipTypeId, this.Name);
        }
    }
}
