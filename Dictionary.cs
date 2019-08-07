using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aml.Editor.Plugin
{
    public class Parameters
    {
        public int ID { get; set; }
        public string Parameter { get; set; }
        public string RefSemanticPrefix { get; set; }

               
    }

    public class CommercialDataDictionary
    {
        public Dictionary<int,Parameters> ProductDetails()
        {
            Parameters ProductDetails1 = new Parameters() { ID = 001, Parameter = "Description Short", RefSemanticPrefix = "" };
            Parameters ProductDetails2 = new Parameters() { ID = 002, Parameter = "Manufacturer PID", RefSemanticPrefix = "" };
            Parameters ProductDetails3 = new Parameters() { ID = 003, Parameter = "International PID", RefSemanticPrefix = "" };
            Parameters ProductDetails4 = new Parameters() { ID = 004, Parameter = "Description Long", RefSemanticPrefix = "" };
            Parameters ProductDetails5 = new Parameters() { ID = 005, Parameter = "Special Treaatment Class", RefSemanticPrefix = "" };
            Parameters ProductDetails6 = new Parameters() { ID = 006, Parameter = "Keyword", RefSemanticPrefix = "" };
            Parameters ProductDetails7 = new Parameters() { ID = 007, Parameter = "Remarks", RefSemanticPrefix = "" };


            // Dictionary for Identification data parameters of eClass 
            Dictionary<int, Parameters> ProductDetailsdataParameters = new Dictionary<int, Parameters>();
            ProductDetailsdataParameters.Add(ProductDetails1.ID, ProductDetails1);
            ProductDetailsdataParameters.Add(ProductDetails2.ID, ProductDetails2);
            ProductDetailsdataParameters.Add(ProductDetails3.ID, ProductDetails3);
            ProductDetailsdataParameters.Add(ProductDetails4.ID, ProductDetails4);
            ProductDetailsdataParameters.Add(ProductDetails5.ID, ProductDetails5);
            ProductDetailsdataParameters.Add(ProductDetails6.ID, ProductDetails6);
            ProductDetailsdataParameters.Add(ProductDetails7.ID, ProductDetails7);

            return ProductDetailsdataParameters;
           
        }
    }
    public class DictionaryeClass 
    {
        
        public  Dictionary<int,Parameters> eClassIdentificationdataParameters()
        {
            Parameters Identificationdata1 = new Parameters() { ID = 001, Parameter = "GTIN(Global Trade Item Number)", RefSemanticPrefix = "0173-1#02-AA0663#003" };
            Parameters Identificationdata2 = new Parameters() { ID = 002, Parameter = "Manufacturer Name", RefSemanticPrefix = "0173-1#02-AAO677#002" };
            Parameters Identificationdata3 = new Parameters() { ID = 003, Parameter = "Manufacturer Product Number", RefSemanticPrefix = "0173-1#02-AAO676#003" };
            Parameters Identificationdata4 = new Parameters() { ID = 004, Parameter = "Product Family", RefSemanticPrefix = "0173-1#02-AAU731#001" };
            Parameters Identificationdata5 = new Parameters() { ID = 005, Parameter = "Product Name", RefSemanticPrefix = "0173-1#02-AAW338#001" };
            Parameters Identificationdata6 = new Parameters() { ID = 006, Parameter = "Supplier Name", RefSemanticPrefix = "0173-1#02-AAO735#003" };
            Parameters Identificationdata7 = new Parameters() { ID = 007, Parameter = "Product Online Information URL", RefSemanticPrefix = "0173-1#02-AAQ326#002" };
            Parameters Identificationdata8 = new Parameters() { ID = 008, Parameter = "Customs Tariff Number", RefSemanticPrefix = "0173-1#02-AAQ326#002" };
            Parameters Identificationdata9 = new Parameters() { ID = 009, Parameter = "Supplier Product Number", RefSemanticPrefix = "0173-1#02-AAO736#004" };

            // Dictionary for Identification data parameters of eClass 
            Dictionary<int, Parameters> IdentificationdataParameters = new Dictionary<int, Parameters>();
            IdentificationdataParameters.Add(Identificationdata1.ID, Identificationdata1);
            IdentificationdataParameters.Add(Identificationdata2.ID, Identificationdata2);
            IdentificationdataParameters.Add(Identificationdata3.ID, Identificationdata3);
            IdentificationdataParameters.Add(Identificationdata4.ID, Identificationdata4);
            IdentificationdataParameters.Add(Identificationdata5.ID, Identificationdata5);
            IdentificationdataParameters.Add(Identificationdata6.ID, Identificationdata6);
            IdentificationdataParameters.Add(Identificationdata7.ID, Identificationdata7);
            IdentificationdataParameters.Add(Identificationdata8.ID, Identificationdata8);
            IdentificationdataParameters.Add(Identificationdata9.ID, Identificationdata9);

            return IdentificationdataParameters;
        }
        


    }
    public class DictionaryIRDI
    {
        public Dictionary<int,Parameters> IRDIIdentificationdata()
        {
            Parameters IRDIIdentificationdata1 = new Parameters() { ID = 001, Parameter = "GTIN(Global Trade Item Number)", RefSemanticPrefix = "0112/2///62683#ACE101" };
            Parameters IRDIIdentificationdata2 = new Parameters() { ID = 002, Parameter = "Manufacturer Name", RefSemanticPrefix = "0112/2///62683#ACE102" };
            Parameters IRDIIdentificationdata3 = new Parameters() { ID = 003, Parameter = "Manufacturer Product Number", RefSemanticPrefix = "0112/2///62683#ACE103" };
            Parameters IRDIIdentificationdata4 = new Parameters() { ID = 004, Parameter = "Product Family", RefSemanticPrefix = "0112/2///62683#ACE104" };
            Parameters IRDIIdentificationdata5 = new Parameters() { ID = 005, Parameter = "Product Name", RefSemanticPrefix = "0112/2///62683#ACE105" };
            Parameters IRDIIdentificationdata6 = new Parameters() { ID = 006, Parameter = "Supplier Name", RefSemanticPrefix = "0112/2///62683#ACE106" };
            Parameters IRDIIdentificationdata7 = new Parameters() { ID = 007, Parameter = "Product Online Information URL", RefSemanticPrefix = "0112/2///62683#ACE108" };
            Parameters IRDIIdentificationdata8 = new Parameters() { ID = 008, Parameter = "Customs Tariff Number", RefSemanticPrefix = "0112/2///62683#ACE109" };
            Parameters IRDIIdentificationdata9 = new Parameters() { ID = 009, Parameter = "Supplier Product Number", RefSemanticPrefix = "0112/2///62683#ACE107" };

            // Dictionary for Identification data parameters of eClass 
            Dictionary<int, Parameters> IRDIIdentificationdataParameters = new Dictionary<int, Parameters>();
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata1.ID, IRDIIdentificationdata1);
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata2.ID, IRDIIdentificationdata2);
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata3.ID, IRDIIdentificationdata3);
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata4.ID, IRDIIdentificationdata4);
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata5.ID, IRDIIdentificationdata5);
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata6.ID, IRDIIdentificationdata6);
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata7.ID, IRDIIdentificationdata7);
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata8.ID, IRDIIdentificationdata8);
            IRDIIdentificationdataParameters.Add(IRDIIdentificationdata9.ID, IRDIIdentificationdata9);


            return IRDIIdentificationdataParameters;
        }
        public Dictionary<int,Parameters> IRDIMechanicalData()
        {
            Parameters IRDIMechanicalData1 = new Parameters() { ID = 001, Parameter = "Width of the device", RefSemanticPrefix = "0112/2///62683#ACE802" };
            Parameters IRDIMechanicalData2 = new Parameters() { ID = 002, Parameter = "Height of the device", RefSemanticPrefix = "0112/2///62683#ACE801" };
            Parameters IRDIMechanicalData3 = new Parameters() { ID = 003, Parameter = "Length of the device", RefSemanticPrefix = "0112/2///62683#ACE803" };
            Parameters IRDIMechanicalData4 = new Parameters() { ID = 004, Parameter = "Diameter of the device", RefSemanticPrefix = "0112/2///62683#ACE810" };
            Parameters IRDIMechanicalData5 = new Parameters() { ID = 005, Parameter = "Mounting position of the sensor", RefSemanticPrefix = "0112/2///62683#ACE811" };
            Parameters IRDIMechanicalData6 = new Parameters() { ID = 005, Parameter = "Housing construction", RefSemanticPrefix = "0112/2///62683#ACE813" };

            Dictionary<int, Parameters> IRDIMechanicalDataParameters = new Dictionary<int, Parameters>();
            IRDIMechanicalDataParameters.Add(IRDIMechanicalData1.ID, IRDIMechanicalData1);
            IRDIMechanicalDataParameters.Add(IRDIMechanicalData2.ID, IRDIMechanicalData2);
            IRDIMechanicalDataParameters.Add(IRDIMechanicalData3.ID, IRDIMechanicalData3);
            IRDIMechanicalDataParameters.Add(IRDIMechanicalData4.ID, IRDIMechanicalData4);
            IRDIMechanicalDataParameters.Add(IRDIMechanicalData5.ID, IRDIMechanicalData5);
            IRDIMechanicalDataParameters.Add(IRDIMechanicalData6.ID, IRDIMechanicalData6);

            return IRDIMechanicalDataParameters;
        }
        public Dictionary<int,Parameters> IRDIMaterialData()
        {
            Parameters IRDIMaterialData1 = new Parameters() { ID = 001, Parameter = "Housing or Body material", RefSemanticPrefix = "0112/2///62683#ACE260" };
            Parameters IRDIMaterialData2 = new Parameters() { ID = 002, Parameter = "Sensing face material", RefSemanticPrefix = "0112/2///62683#ACE261" };
            Parameters IRDIMaterialData3 = new Parameters() { ID = 003, Parameter = "Surface Protection", RefSemanticPrefix = "" };

            Dictionary<int, Parameters> IRDIMaterialDataParameters = new Dictionary<int, Parameters>();
            IRDIMaterialDataParameters.Add(IRDIMaterialData1.ID, IRDIMaterialData1);
            IRDIMaterialDataParameters.Add(IRDIMaterialData2.ID, IRDIMaterialData2);
            IRDIMaterialDataParameters.Add(IRDIMaterialData3.ID, IRDIMaterialData3);

            return IRDIMaterialDataParameters;
        }
        
    }


}
