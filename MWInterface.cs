using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aml.Editor.Plugin
{
    public class MWInterface: MWData.MWObject
    {
        public int numberOfInterface { get; set; }
        public string interfaceDescription { get; set; }
        public string connectorType { get; set; }
        public int amountPins { get; set; }
        public List<MWPin> pinList { get; set; }
    }

    public class MWPin
    {

        public MWPin() { }

        public MWPin(int number, string attribute)
        {
            pinNumber = number;
            this.attribute = attribute;
        }

        public int pinNumber { get; set; }
        public string attribute { get; set; }

        public override string ToString()
        {
            return "MWPin(" + pinNumber + "=" + attribute + ")";
        }
    }

    public class MWConnectorTypes
    {
        public static Dictionary<string, int> ConnectorMap = new Dictionary<string, int>
            {
                {"M5 Connector (4 pins)", 4},
                {"M8 Connector (4 pins)", 4},
                {"M12 Connector (D-coded 4/5 pins)", -1},
                {"RJ45-Ethernet Connector", 0},
                {"M12 (D-coded) Fast-Ethernet Connector", 0},
                {"M12 (X-coded) Gigabit-Ethernet Connector", 0},
                {"M12 (S, L, K, T-coded) Power Supply Connector", 0},
                {"7/8-Inch Power Supply Connector Male", 0},
                {"7/8-Inch Power Supply Connector Female", 0},
                {"Other Connection (1-x pins)", -1},
                {"Cable Connection (1-x wires)", -1}
            };
    }
}
