using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aml.Editor.Plugin
{
    public class MWDevice: MWData.MWObject
    {
        public string deviceType { get; set; }
        public int? vendorID { get; set; }
        public string vendorName { get; set; }
        public string vendorHomepage { get; set; }
        public int? deviceID { get; set; }
        public string deviceName { get; set; }
        public string deviceFamily { get; set; }
        public string productName { get; set; }
        // Can contain letters:
        public string orderNumber { get; set; }
        public string productText { get; set; }
        public string ipProtection { get; set; }
        public string harwareRelease { get; set; }
        public string softwareRelease { get; set; }
        public double minTemperature { get; set; }
        public double maxTemperature { get; set; }
        public string vendorLogo { get; set; }
        public string deviceIcon { get; set; }
        public string devicePicture { get; set; }
    }
}
