using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    /// <summary>
    /// Represents a point of a protocol
    /// example: name: stove, condition: new
    /// </summary>
    public class ProtocolPoint
    {
        public int protocolPointID { get; private set; }
        public string protocolPointName { get; set; } = "";
        public PointCondition condition { get; set; }

        #region CONSTRUCTORS
        public ProtocolPoint()
        {
        }

        public ProtocolPoint(string protocolPointName, PointCondition condition)
        {
            this.protocolPointName = protocolPointName;
            this.condition = condition;
        }
        #endregion


    }
}
