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
        public string productRange { get; set; }
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
        public string productFamily { get; set; }
        public string productGroup { get; set; }
        public string decOfConfDocument { get; set; }
        public string shortGuideDocument { get; set; }
        public string billOfMaterialsDocument { get; set; }
        public List<DataGridParameters> dataGridParametersLists { get; set; }
        //Properties for datagridviews

       

        // Properties for Inetrface data

    }
    // This class helps to carry parameters in "identification data table to AutomationML"
   
   public class DataGridParameters
    {
       

       public DataGridParameters() { }

        public DataGridParameters(string refSemantic, string attributes, string value)
        {
            RefSemantic = refSemantic;
            this.Attributes = attributes;
            _value = value;

        }

        public string RefSemantic { get; set; }
        public string Attributes { get; set; }
        public string _value { get; set; }

        public override string ToString()
        {
            return "DataGridParameters(" + RefSemantic + "=" + Attributes + "=" + _value +")";
        }
    }


}
