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
        public string Unit { get; set; }
        public string Pin { get; set; }
        public string Value { get; set; }
        public string Min { get; set; }
        public string Nom { get; set; }
        public string Max { get; set; }
               
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
        public Dictionary<int, Parameters> ProductOrderDetails()
        {
            Parameters ProductOrderDetails1 = new Parameters() { ID = 001, Parameter = "Order Unit", RefSemanticPrefix = "" };
            Parameters ProductOrderDetails2 = new Parameters() { ID = 002, Parameter = "Content Unit", RefSemanticPrefix = "" };
            Parameters ProductOrderDetails3 = new Parameters() { ID = 003, Parameter = "Price Quantity", RefSemanticPrefix = "" };
            Parameters ProductOrderDetails4 = new Parameters() { ID = 004, Parameter = "Quantity Min", RefSemanticPrefix = "" };
            Parameters ProductOrderDetails5 = new Parameters() { ID = 005, Parameter = "Quantity Interval", RefSemanticPrefix = "" };
            Parameters ProductOrderDetails6 = new Parameters() { ID = 006, Parameter = "Quantity Max", RefSemanticPrefix = "" };
            Parameters ProductOrderDetails7 = new Parameters() { ID = 007, Parameter = "Packing Units", RefSemanticPrefix = "" };
            Parameters ProductOrderDetails8 = new Parameters() { ID = 008, Parameter = "Packing Size", RefSemanticPrefix = "" };


            // Dictionary for Identification data parameters of eClass 
            Dictionary<int, Parameters> ProductOrderDetailsdataParameters = new Dictionary<int, Parameters>();
            ProductOrderDetailsdataParameters.Add(ProductOrderDetails1.ID, ProductOrderDetails1);
            ProductOrderDetailsdataParameters.Add(ProductOrderDetails2.ID, ProductOrderDetails2);
            ProductOrderDetailsdataParameters.Add(ProductOrderDetails3.ID, ProductOrderDetails3);
            ProductOrderDetailsdataParameters.Add(ProductOrderDetails4.ID, ProductOrderDetails4);
            ProductOrderDetailsdataParameters.Add(ProductOrderDetails5.ID, ProductOrderDetails5);
            ProductOrderDetailsdataParameters.Add(ProductOrderDetails6.ID, ProductOrderDetails6);
            ProductOrderDetailsdataParameters.Add(ProductOrderDetails7.ID, ProductOrderDetails7);
            ProductOrderDetailsdataParameters.Add(ProductOrderDetails8.ID, ProductOrderDetails8);


            return ProductOrderDetailsdataParameters;

        }
        public Dictionary<int, Parameters> ProductPriceDetails()
        {
            Parameters ProductPriceDetails1 = new Parameters() { ID = 001, Parameter = "Price Amount", RefSemanticPrefix = "" };
            Parameters ProductPriceDetails2 = new Parameters() { ID = 002, Parameter = "Price Currency", RefSemanticPrefix = "" };
            Parameters ProductPriceDetails3 = new Parameters() { ID = 003, Parameter = "Tax", RefSemanticPrefix = "" };
            Parameters ProductPriceDetails4 = new Parameters() { ID = 004, Parameter = "Price Factor", RefSemanticPrefix = "" };
            Parameters ProductPriceDetails5 = new Parameters() { ID = 005, Parameter = "Lower Bound", RefSemanticPrefix = "" };
            Parameters ProductPriceDetails6 = new Parameters() { ID = 006, Parameter = "Territory", RefSemanticPrefix = "" };
           

            // Dictionary for Identification data parameters of eClass 
            Dictionary<int, Parameters> ProductPriceDetailsdataParameters = new Dictionary<int, Parameters>();
            ProductPriceDetailsdataParameters.Add(ProductPriceDetails1.ID, ProductPriceDetails1);
            ProductPriceDetailsdataParameters.Add(ProductPriceDetails2.ID, ProductPriceDetails2);
            ProductPriceDetailsdataParameters.Add(ProductPriceDetails3.ID, ProductPriceDetails3);
            ProductPriceDetailsdataParameters.Add(ProductPriceDetails4.ID, ProductPriceDetails4);
            ProductPriceDetailsdataParameters.Add(ProductPriceDetails5.ID, ProductPriceDetails5);
            ProductPriceDetailsdataParameters.Add(ProductPriceDetails6.ID, ProductPriceDetails6);
           
            return ProductPriceDetailsdataParameters;

        }
        public Dictionary<int, Parameters>ManufacturerDetails()
        {
            Parameters ManufacturerDetails1 = new Parameters() { ID = 001, Parameter = "Name", RefSemanticPrefix = "" };
            Parameters ManufacturerDetails2 = new Parameters() { ID = 002, Parameter = "Address 1", RefSemanticPrefix = "" };
            Parameters ManufacturerDetails3 = new Parameters() { ID = 003, Parameter = "Address 2", RefSemanticPrefix = "" };
            Parameters ManufacturerDetails4 = new Parameters() { ID = 004, Parameter = "Zip Code", RefSemanticPrefix = "" };
            Parameters ManufacturerDetails5 = new Parameters() { ID = 005, Parameter = "City", RefSemanticPrefix = "" };
            Parameters ManufacturerDetails6 = new Parameters() { ID = 006, Parameter = "Country", RefSemanticPrefix = "" };
            Parameters ManufacturerDetails7 = new Parameters() { ID = 007, Parameter = "Contact Mail", RefSemanticPrefix = "" };
            Parameters ManufacturerDetails8 = new Parameters() { ID = 008, Parameter = "Contact Phone", RefSemanticPrefix = "" };
            Parameters ManufacturerDetails9 = new Parameters() { ID = 009, Parameter = "Website", RefSemanticPrefix = "" };


            // Dictionary for Identification data parameters of eClass 
            Dictionary<int, Parameters> ManufacturerDetailsdataParameters = new Dictionary<int, Parameters>();
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails1.ID, ManufacturerDetails1);
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails2.ID, ManufacturerDetails2);
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails3.ID, ManufacturerDetails3);
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails4.ID, ManufacturerDetails4);
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails5.ID, ManufacturerDetails5);
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails6.ID, ManufacturerDetails6);
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails7.ID, ManufacturerDetails7);
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails8.ID, ManufacturerDetails8);
            ManufacturerDetailsdataParameters.Add(ManufacturerDetails9.ID, ManufacturerDetails9);

            return ManufacturerDetailsdataParameters;

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
        public Dictionary<int, Parameters> IRDIIdentificationdata()
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
        public Dictionary<int, Parameters> IRDIMechanicalData()
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
        public Dictionary<int, Parameters> IRDITemperatureData()
        {
            Parameters IRDITemperatureData1 = new Parameters() { ID = 001, Parameter = "stress temperature min (T_stress(min))", RefSemanticPrefix = "0112/2///61360_4#AAF276#002",Value = "",Unit = "°C" };
            Parameters IRDITemperatureData2 = new Parameters() { ID = 002, Parameter = "stress temperature max (T_stress(max))", RefSemanticPrefix = "0112/2///61360_4#AAF277#002", Value = "", Unit = "°C" };
            Parameters IRDITemperatureData3 = new Parameters() { ID = 003, Parameter = "stress ambient temperature (T_stesss(amb))", RefSemanticPrefix = "0112/2///61360_4#AAF278#002", Value = "", Unit = "°C" };
            Parameters IRDITemperatureData4 = new Parameters() { ID = 004, Parameter = "upper category temperature (T_ucat)", RefSemanticPrefix = "0112/2///61360_4#AAH007#002", Value = "", Unit = "°C" };
            Parameters IRDITemperatureData5= new Parameters() { ID = 006, Parameter = "lower category temperature (T_lcat)", RefSemanticPrefix = "0112/2///61360_4#AAH008#002", Value = "", Unit = "°C" };
            Parameters IRDITemperatureData6= new Parameters() { ID = 007, Parameter = "temperature (@T)", RefSemanticPrefix = "0112/2///61360_4#AAE685#001", Value = "", Unit = "°C" };
            Parameters IRDITemperatureData7 = new Parameters() { ID = 008, Parameter = "storage temperature (T_stg)", RefSemanticPrefix = "0112/2///61360_4#AAE841#002", Value = "", Unit = "°C" };

            Dictionary<int, Parameters> IRDITemperatureDataParameters = new Dictionary<int, Parameters>();
            IRDITemperatureDataParameters.Add(IRDITemperatureData1.ID, IRDITemperatureData1);
            IRDITemperatureDataParameters.Add(IRDITemperatureData2.ID, IRDITemperatureData2);
            IRDITemperatureDataParameters.Add(IRDITemperatureData3.ID, IRDITemperatureData3);
            IRDITemperatureDataParameters.Add(IRDITemperatureData4.ID, IRDITemperatureData4);
            IRDITemperatureDataParameters.Add(IRDITemperatureData5.ID, IRDITemperatureData5);
            IRDITemperatureDataParameters.Add(IRDITemperatureData6.ID, IRDITemperatureData6);
            IRDITemperatureDataParameters.Add(IRDITemperatureData7.ID, IRDITemperatureData7);

            return IRDITemperatureDataParameters;
        }
            public Dictionary<int, Parameters> IRDIMaterialData()
        {
            Parameters IRDIMaterialData1 = new Parameters() { ID = 001, Parameter = "Housing or Body material", RefSemanticPrefix = "0112/2///61360_4#AAE351#006", Value = "" };
            Parameters IRDIMaterialData2 = new Parameters() { ID = 002, Parameter = "Sensing face material", RefSemanticPrefix = "0112/2///62683#ACE261", Value = "" };
            Parameters IRDIMaterialData3 = new Parameters() { ID = 003, Parameter = "Surface Protection", RefSemanticPrefix = "", Value = "" };
            Parameters IRDIMaterialData4 = new Parameters() { ID = 004, Parameter = "Contact body material", RefSemanticPrefix = "0112/2///61360_4#AAE355#001", Value = "" };
            Parameters IRDIMaterialData5 = new Parameters() { ID = 005, Parameter = "terminal material", RefSemanticPrefix = "0112/2///61360_4#AAE634#001", Value = "" };
            Parameters IRDIMaterialData6 = new Parameters() { ID = 006, Parameter = "contact spring material", RefSemanticPrefix = "0112/2///61360_4#AAF125#001", Value = "" };
            Parameters IRDIMaterialData7 = new Parameters() { ID = 007, Parameter = "insulating material group", RefSemanticPrefix = "0112/2///61360_4#AAH025#002", Value = "" };
            Parameters IRDIMaterialData8 = new Parameters() { ID = 008, Parameter = "body insulation material", RefSemanticPrefix = "0112/2///61360_4#AAH056#002", Value = "" };
            Parameters IRDIMaterialData9 = new Parameters() { ID = 009, Parameter = "contact finish", RefSemanticPrefix = "0112/2///61360_4#AAE350#001", Value = "" };
            Parameters IRDIMaterialData10 = new Parameters() { ID = 010, Parameter = "housing finish", RefSemanticPrefix = "0112/2///61360_4#AAH005#002", Value = "" };
            Parameters IRDIMaterialData11 = new Parameters() { ID = 011, Parameter = "terminal finish", RefSemanticPrefix = "0112/2///61360_4#AAH028#002", Value = "" };

            Dictionary<int, Parameters> IRDIMaterialDataParameters = new Dictionary<int, Parameters>();
            IRDIMaterialDataParameters.Add(IRDIMaterialData1.ID, IRDIMaterialData1);
            IRDIMaterialDataParameters.Add(IRDIMaterialData2.ID, IRDIMaterialData2);
            IRDIMaterialDataParameters.Add(IRDIMaterialData3.ID, IRDIMaterialData3);
            IRDIMaterialDataParameters.Add(IRDIMaterialData4.ID, IRDIMaterialData4);
            IRDIMaterialDataParameters.Add(IRDIMaterialData5.ID, IRDIMaterialData5);
            IRDIMaterialDataParameters.Add(IRDIMaterialData6.ID, IRDIMaterialData6);
            IRDIMaterialDataParameters.Add(IRDIMaterialData7.ID, IRDIMaterialData7);
            IRDIMaterialDataParameters.Add(IRDIMaterialData8.ID, IRDIMaterialData8);
            IRDIMaterialDataParameters.Add(IRDIMaterialData9.ID, IRDIMaterialData9);
            IRDIMaterialDataParameters.Add(IRDIMaterialData10.ID, IRDIMaterialData10);
            IRDIMaterialDataParameters.Add(IRDIMaterialData11.ID, IRDIMaterialData11);


            return IRDIMaterialDataParameters;
        }
        public Dictionary<int, Parameters> IRDIElectricalData()
        {
            Parameters IRDIElectricalData1 = new Parameters() { ID = 001, Parameter = "Current rms (I_rms)", RefSemanticPrefix = "0112/2///61360_4#AAE540#001", Unit = "A" };
            Parameters IRDIElectricalData2 = new Parameters() { ID = 002, Parameter = "Rated Voltage (U_rat)", RefSemanticPrefix = "0112/2///61360_4#AAH012#002", Unit = "V" };
            Parameters IRDIElectricalData3 = new Parameters() { ID = 003, Parameter = "Rated Impulse Voltage (U_Irat)", RefSemanticPrefix = "0112/2///61360_4#AAH013#002", Unit = "V" };
            Parameters IRDIElectricalData4 = new Parameters() { ID = 004, Parameter = "Voltage Proof (U_proof)", RefSemanticPrefix = "0112/2///61360_4#AAH014#002", Unit = "V" };
            Parameters IRDIElectricalData5 = new Parameters() { ID = 005, Parameter = "Dielectric Withstand Voltage (U_wdi)", RefSemanticPrefix = "0112/2///61360_4#AAH015#002", Unit = "V" };
            Parameters IRDIElectricalData6 = new Parameters() { ID = 006, Parameter = "Over Voltage Category", RefSemanticPrefix = "0112/2///61360_4#AAH022#002", Unit = "" };
            Parameters IRDIElectricalData7 = new Parameters() { ID = 007, Parameter = "Rated Current (I_rat)", RefSemanticPrefix = "0112/2///61360_4#AAH066#002", Unit = "A" };
            Parameters IRDIElectricalData8 = new Parameters() { ID = 008, Parameter = "Connector rated Voltage (V_r)", RefSemanticPrefix = "0112/2///61360_4#AAJ042#002", Unit = "V" };
            Parameters IRDIElectricalData9 = new Parameters() { ID = 009, Parameter = "Connector rated Current (I_r)", RefSemanticPrefix = "0112/2///61360_4#AAJ043#002", Unit = "A" };
            Parameters IRDIElectricalData10 = new Parameters() { ID = 010, Parameter = "Contact current max (I_cont)", RefSemanticPrefix = "0112/2///61360_4#AAE358#001", Unit = "A" };
            Parameters IRDIElectricalData11 = new Parameters() { ID = 011, Parameter = "power dissipation (P_dis)", RefSemanticPrefix = "0112/2///61360_4#AAE257#003", Unit = "W" };

            Dictionary<int, Parameters> IRDIElectricalDataParameters = new Dictionary<int, Parameters>();
            IRDIElectricalDataParameters.Add(IRDIElectricalData1.ID, IRDIElectricalData1);
            IRDIElectricalDataParameters.Add(IRDIElectricalData2.ID, IRDIElectricalData2);
            IRDIElectricalDataParameters.Add(IRDIElectricalData3.ID, IRDIElectricalData3);
            IRDIElectricalDataParameters.Add(IRDIElectricalData4.ID, IRDIElectricalData4);
            IRDIElectricalDataParameters.Add(IRDIElectricalData5.ID, IRDIElectricalData5);
            IRDIElectricalDataParameters.Add(IRDIElectricalData6.ID, IRDIElectricalData6);
            IRDIElectricalDataParameters.Add(IRDIElectricalData7.ID, IRDIElectricalData7);
            IRDIElectricalDataParameters.Add(IRDIElectricalData8.ID, IRDIElectricalData8);
            IRDIElectricalDataParameters.Add(IRDIElectricalData9.ID, IRDIElectricalData9);
            IRDIElectricalDataParameters.Add(IRDIElectricalData10.ID, IRDIElectricalData10);
            IRDIElectricalDataParameters.Add(IRDIElectricalData11.ID, IRDIElectricalData11);


            return IRDIElectricalDataParameters;

        }
        public Dictionary<int, Parameters> IRDIMountingSquareFlangeData()
        {
            Parameters IRDIMountingSquareFlangeData1 = new Parameters() { ID = 001, Parameter = "Flange Length (I_flange)", RefSemanticPrefix = "0112/2///61360_4#AAF317#001", Unit = "mm", Value = "20" };
            Parameters IRDIMountingSquareFlangeData2 = new Parameters() { ID = 002, Parameter = "Flange breadth (b_flange)", RefSemanticPrefix = "0112/2///61360_4#AAF318#001", Unit = "mm", Value = "20" };
            Parameters IRDIMountingSquareFlangeData3 = new Parameters() { ID = 003, Parameter = "Flange height (h_flg)", RefSemanticPrefix = "0112/2///61360_4#AAF317#001", Unit = "mm", Value = "20" };
            Parameters IRDIMountingSquareFlangeData4 = new Parameters() { ID = 004, Parameter = "Flange hole Diameter", RefSemanticPrefix = "0112/2///61360_4#AAF319#001", Unit = "mm", Value = "3.2" };
            Parameters IRDIMountingSquareFlangeData5 = new Parameters() { ID = 005, Parameter = "Flange holes distance", RefSemanticPrefix = "", Unit = "mm", Value = "14" };
            Parameters IRDIMountingSquareFlangeData6 = new Parameters() { ID = 006, Parameter = "Flange Thickness", RefSemanticPrefix = "", Unit = "mm", Value = "4" };
            Parameters IRDIMountingSquareFlangeData7 = new Parameters() { ID = 007, Parameter = "Terminal Length (I_term)", RefSemanticPrefix = "0112/2///61360_4#AAE072#001", Unit = "mm", Value = "28" };
            Parameters IRDIMountingSquareFlangeData8 = new Parameters() { ID = 008, Parameter = "Outside Diameter (d_out)", RefSemanticPrefix = "0112/2///61360_4#AAE022#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData9 = new Parameters() { ID = 009, Parameter = "Terminal Diameter (d_term)", RefSemanticPrefix = "0112/2///61360_4#AAE022#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData10 = new Parameters() { ID = 010, Parameter = "terminal Pitch (p_term)", RefSemanticPrefix = "0112/2///61360_4#AAE024#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData11 = new Parameters() { ID = 011, Parameter = "lacquered length (l_lacq)", RefSemanticPrefix = "0112/2///61360_4#AAE633#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
          /*  Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData12 = new Parameters() { ID = 0012, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };*/

            Dictionary<int, Parameters> IRDIMountingSquareFlangeDataParameters = new Dictionary<int, Parameters>();
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData1.ID, IRDIMountingSquareFlangeData1);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData2.ID, IRDIMountingSquareFlangeData2);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData3.ID, IRDIMountingSquareFlangeData3);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData4.ID, IRDIMountingSquareFlangeData4);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData5.ID, IRDIMountingSquareFlangeData5);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData6.ID, IRDIMountingSquareFlangeData6);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData7.ID, IRDIMountingSquareFlangeData7);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData8.ID, IRDIMountingSquareFlangeData8);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData9.ID, IRDIMountingSquareFlangeData9);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData10.ID, IRDIMountingSquareFlangeData10);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData11.ID, IRDIMountingSquareFlangeData11);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData12.ID, IRDIMountingSquareFlangeData12);

            return IRDIMountingSquareFlangeDataParameters;
        }
        public Dictionary<int, Parameters> IRDIMountingSingleHoleData()
        {
            Parameters IRDIMountingSingleHoleData1 = new Parameters() { ID = 001, Parameter = "Single Hole Diameter", RefSemanticPrefix = "", Unit = "mm", Value = "20" };
            Parameters IRDIMountingSingleHoleData2 = new Parameters() { ID = 002, Parameter = "Hole Pitch (p_hole)", RefSemanticPrefix = "0112/2///61360_4#AAF316#001", Unit = "mm", Value = "3.2" };
            Parameters IRDIMountingSingleHoleData3 = new Parameters() { ID = 003, Parameter = "Length of Wire ends", RefSemanticPrefix = "", Unit = "mm", Value = "14" };
            Parameters IRDIMountingSquareFlangeData5 = new Parameters() { ID = 004, Parameter = "Terminal Length (I_term)", RefSemanticPrefix = "0112/2///61360_4#AAE072#001", Unit = "mm", Value = "28" };
            Parameters IRDIMountingSquareFlangeData6 = new Parameters() { ID = 005, Parameter = "Outside Diameter (d_out)", RefSemanticPrefix = "0112/2///61360_4#AAE022#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData7 = new Parameters() { ID = 006, Parameter = "Terminal Diameter (d_term)", RefSemanticPrefix = "0112/2///61360_4#AAE022#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData8 = new Parameters() { ID = 008, Parameter = "terminal Pitch (p_term)", RefSemanticPrefix = "0112/2///61360_4#AAE024#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData9 = new Parameters() { ID = 009, Parameter = "lacquered length (l_lacq)", RefSemanticPrefix = "0112/2///61360_4#AAE633#001", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData10 = new Parameters() { ID = 0010, Parameter = "Pitch Circle Diameter (d_p)", RefSemanticPrefix = "0112/2///61360_4#AAF337#001", Unit = "mm", Value = "" };

            Dictionary<int, Parameters> IRDIMountingSingleHoleDataParameters = new Dictionary<int, Parameters>();
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSingleHoleData1.ID, IRDIMountingSingleHoleData1);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSingleHoleData2.ID, IRDIMountingSingleHoleData2);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSingleHoleData3.ID, IRDIMountingSingleHoleData3);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData5.ID, IRDIMountingSquareFlangeData5);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData6.ID, IRDIMountingSquareFlangeData6);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData7.ID, IRDIMountingSquareFlangeData7);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData8.ID, IRDIMountingSquareFlangeData8);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData9.ID, IRDIMountingSquareFlangeData9);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData10.ID, IRDIMountingSquareFlangeData10);

            return IRDIMountingSingleHoleDataParameters;
        }
        public Dictionary<int, Parameters> IRDIMaleConnectorDimensionData()
        {
            Parameters IRDIMaleConnectorDimensioneData1 = new Parameters() { ID = 001, Parameter = "AA", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData2 = new Parameters() { ID = 002, Parameter = "AB", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData3 = new Parameters() { ID = 003, Parameter = "AC", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData4 = new Parameters() { ID = 004, Parameter = "AD", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData5 = new Parameters() { ID = 005, Parameter = "AE", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData6 = new Parameters() { ID = 006, Parameter = "AF", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData7 = new Parameters() { ID = 007, Parameter = "AG", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData8 = new Parameters() { ID = 008, Parameter = "AH", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData9 = new Parameters() { ID = 009, Parameter = "AI", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData10 = new Parameters() { ID = 010, Parameter = "AJ", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData11 = new Parameters() { ID = 011, Parameter = "AK", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData12 = new Parameters() { ID = 012, Parameter = "AL", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData13 = new Parameters() { ID = 013, Parameter = "AM", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData14 = new Parameters() { ID = 014, Parameter = "AN", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData15 = new Parameters() { ID = 015, Parameter = "AO", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData16 = new Parameters() { ID = 016, Parameter = "AP", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData17 = new Parameters() { ID = 017, Parameter = "AQ", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData18 = new Parameters() { ID = 018, Parameter = "AR", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData19 = new Parameters() { ID = 019, Parameter = "AS", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData20 = new Parameters() { ID = 020, Parameter = "AT", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData21 = new Parameters() { ID = 021, Parameter = "AU", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData22 = new Parameters() { ID = 022, Parameter = "AV", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData23 = new Parameters() { ID = 023, Parameter = "AW", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData24 = new Parameters() { ID = 024, Parameter = "AX", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIMaleConnectorDimensioneData25 = new Parameters() { ID = 025, Parameter = "AY", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };


            Dictionary<int, Parameters> IRDIMaleConnectorDimensioneDataParameters = new Dictionary<int, Parameters>();
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData1.ID, IRDIMaleConnectorDimensioneData1);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData2.ID, IRDIMaleConnectorDimensioneData2);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData3.ID, IRDIMaleConnectorDimensioneData3);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData4.ID, IRDIMaleConnectorDimensioneData4);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData5.ID, IRDIMaleConnectorDimensioneData5);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData6.ID, IRDIMaleConnectorDimensioneData6);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData7.ID, IRDIMaleConnectorDimensioneData7);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData8.ID, IRDIMaleConnectorDimensioneData8);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData9.ID, IRDIMaleConnectorDimensioneData9);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData10.ID, IRDIMaleConnectorDimensioneData10);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData11.ID, IRDIMaleConnectorDimensioneData11);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData12.ID, IRDIMaleConnectorDimensioneData12);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData13.ID, IRDIMaleConnectorDimensioneData13);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData14.ID, IRDIMaleConnectorDimensioneData14);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData15.ID, IRDIMaleConnectorDimensioneData15);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData16.ID, IRDIMaleConnectorDimensioneData16);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData17.ID, IRDIMaleConnectorDimensioneData17);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData18.ID, IRDIMaleConnectorDimensioneData18);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData19.ID, IRDIMaleConnectorDimensioneData19);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData20.ID, IRDIMaleConnectorDimensioneData20);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData21.ID, IRDIMaleConnectorDimensioneData21);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData22.ID, IRDIMaleConnectorDimensioneData22);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData23.ID, IRDIMaleConnectorDimensioneData23);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData24.ID, IRDIMaleConnectorDimensioneData24);
            IRDIMaleConnectorDimensioneDataParameters.Add(IRDIMaleConnectorDimensioneData25.ID, IRDIMaleConnectorDimensioneData25);



            return IRDIMaleConnectorDimensioneDataParameters;
        }
        public Dictionary<int, Parameters> IRDIFemaleConnectorDimensionData()
        {
            Parameters IRDIFemaleConnectorDimensioneData1 = new Parameters() { ID = 001, Parameter = "BA", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIFemaleConnectorDimensioneData2 = new Parameters() { ID = 002, Parameter = "BB", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIFemaleConnectorDimensioneData3 = new Parameters() { ID = 003, Parameter = "BC", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIFemaleConnectorDimensioneData4 = new Parameters() { ID = 004, Parameter = "BD", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };

            Dictionary<int, Parameters> IRDIFemaleConnectorDimensioneDataParameters = new Dictionary<int, Parameters>();
            IRDIFemaleConnectorDimensioneDataParameters.Add(IRDIFemaleConnectorDimensioneData1.ID, IRDIFemaleConnectorDimensioneData1);
            IRDIFemaleConnectorDimensioneDataParameters.Add(IRDIFemaleConnectorDimensioneData2.ID, IRDIFemaleConnectorDimensioneData2);
            IRDIFemaleConnectorDimensioneDataParameters.Add(IRDIFemaleConnectorDimensioneData3.ID, IRDIFemaleConnectorDimensioneData3);
            IRDIFemaleConnectorDimensioneDataParameters.Add(IRDIFemaleConnectorDimensioneData4.ID, IRDIFemaleConnectorDimensioneData4);

            return IRDIFemaleConnectorDimensioneDataParameters;
        }
        public Dictionary<int, Parameters> IRDIConnectorOrientationData()
        {
            Parameters IRDIConnectorOrientationData1 = new Parameters() { ID = 001, Parameter = "Pitch (X-axis) (p_x)", RefSemanticPrefix = "0112/2///61360_4#AAF321#001", Unit = "m", Value ="" };
            Parameters IRDIConnectorOrientationData2 = new Parameters() { ID = 002, Parameter = "Pitch (Y-axis) (p_y)", RefSemanticPrefix = "0112/2///61360_4#AAF322#001", Unit = "m", Value = "" };
            Parameters IRDIConnectorOrientationData3 = new Parameters() { ID = 003, Parameter = "Offset (y-axis) (s_y)", RefSemanticPrefix = "0112/2///61360_4#AAF340#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData4 = new Parameters() { ID = 004, Parameter = "Offset (x-axis) (s_x)", RefSemanticPrefix = "0112/2///61360_4#AAF341#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData5 = new Parameters() { ID = 005, Parameter = "Number of pitches (x-axis) (N_p(x))", RefSemanticPrefix = "0112/2///61360_4#AAF374#001", Unit = "", Value = "" };
            Parameters IRDIConnectorOrientationData6 = new Parameters() { ID = 006, Parameter = "Number of pitches (y-axis) (N_p(y))", RefSemanticPrefix = "0112/2///61360_4#AAF375#001", Unit = "", Value = "" };
            Parameters IRDIConnectorOrientationData7= new Parameters() { ID = 007, Parameter = "x-coordinate of the reference point (x-coor ref pt)", RefSemanticPrefix = "0112/2///61360_4#AAF393#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData8 = new Parameters() { ID = 008, Parameter = "y-coordinate of the reference point (y-coor ref pt)", RefSemanticPrefix = "0112/2///61360_4#AAF394#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData9= new Parameters() { ID = 009, Parameter = "z-coordinate of the reference point (z-coor ref pt)", RefSemanticPrefix = "0112/2///61360_4#AAF395#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData10= new Parameters() { ID = 010, Parameter = "Angle axis to x-axis (α_x)", RefSemanticPrefix = "0112/2///61360_4#AAF411#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData11 = new Parameters() { ID = 011, Parameter = "Angle axis to y-axis (α_y)", RefSemanticPrefix = "0112/2///61360_4#AAF412#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData12 = new Parameters() { ID = 012, Parameter = "Angle axis to z-axis (α_z)", RefSemanticPrefix = "0112/2///61360_4#AAF413#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData13 = new Parameters() { ID = 013, Parameter = "y-coordinate of centre (y-c_sphere)", RefSemanticPrefix = "0112/2///61360_4#AAF419#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData14 = new Parameters() { ID = 014, Parameter = "z-coordinate of centre (z-c_sphere)", RefSemanticPrefix = "0112/2///61360_4#AAF420#001", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData15 = new Parameters() { ID = 015, Parameter = "Contact pitch (x-axis) (l_xpitch)", RefSemanticPrefix = "0112/2///61360_4#AAH045#002", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData16 = new Parameters() { ID = 016, Parameter = "Contact pitch (y-axis) (l_ypitch)", RefSemanticPrefix = "0112/2///61360_4#AAH046#002", Unit = "mm", Value = "" };
            Parameters IRDIConnectorOrientationData17 = new Parameters() { ID = 0117, Parameter = "z-axis displacement of centre of gravity (C_grav(z-axis))", RefSemanticPrefix = "0112/2///61360_4#AAF472#001", Unit = "mm", Value = "" };


            Dictionary<int, Parameters> IRDIConnectorOrientationDataParameters = new Dictionary<int, Parameters>();
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData1.ID, IRDIConnectorOrientationData1);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData2.ID, IRDIConnectorOrientationData2);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData3.ID, IRDIConnectorOrientationData3);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData4.ID, IRDIConnectorOrientationData4);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData5.ID, IRDIConnectorOrientationData5);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData6.ID, IRDIConnectorOrientationData6);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData7.ID, IRDIConnectorOrientationData7);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData8.ID, IRDIConnectorOrientationData8);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData9.ID, IRDIConnectorOrientationData9);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData10.ID, IRDIConnectorOrientationData10);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData11.ID, IRDIConnectorOrientationData11);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData12.ID, IRDIConnectorOrientationData12);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData13.ID, IRDIConnectorOrientationData13);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData14.ID, IRDIConnectorOrientationData14);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData15.ID, IRDIConnectorOrientationData15);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData16.ID, IRDIConnectorOrientationData16);
            IRDIConnectorOrientationDataParameters.Add(IRDIConnectorOrientationData17.ID, IRDIConnectorOrientationData17);

            return IRDIConnectorOrientationDataParameters;
        }
        public Dictionary<int, Parameters> IRDIMiscelliniousData()
        {
            Parameters IRDIMiscelliniousData1 = new Parameters() { ID = 001, Parameter = "engaging force (F_eng)", RefSemanticPrefix = "0112/2///61360_4#AAF045#001", Unit = "F", Value = "" };
            Parameters IRDIMiscelliniousData2 = new Parameters() { ID = 002, Parameter = "separating force (F_sep)", RefSemanticPrefix = "0112/2///61360_4#AAF046#001", Unit = "F", Value = "" };
            Parameters IRDIMiscelliniousData3 = new Parameters() { ID = 003, Parameter = "contact force (F_contact)", RefSemanticPrefix = "0112/2///61360_4#AAH018#002", Unit = "F", Value = "" };
            Parameters IRDIMiscelliniousData4 = new Parameters() { ID = 004, Parameter = "mechanical endurance (N_endu)", RefSemanticPrefix = "0112/2///61360_4#AAE361#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData5 = new Parameters() { ID = 005, Parameter = "creepage distance (d_crpg)", RefSemanticPrefix = "0112/2///61360_4#AAE159#001", Unit = "mm", Value = "" };
            Parameters IRDIMiscelliniousData6 = new Parameters() { ID = 006, Parameter = "polarisation ", RefSemanticPrefix = "0112/2///61360_4#AAE354#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData7 = new Parameters() { ID = 007, Parameter = "quality approval authority", RefSemanticPrefix = "0112/2///61360_4#AAE687#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData8 = new Parameters() { ID = 008, Parameter = "locking device", RefSemanticPrefix = "0112/2///61360_4#AAF051#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData9 = new Parameters() { ID = 009, Parameter = "integrated component", RefSemanticPrefix = "0112/2///61360_4#AAF124#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData10 = new Parameters() { ID = 010, Parameter = "UL flammability", RefSemanticPrefix = "0112/2///61360_4#AAF126#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData11 = new Parameters() { ID = 011, Parameter = "IEC flammability", RefSemanticPrefix = "0112/2///61360_4#AAF127#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData12 = new Parameters() { ID = 012, Parameter = "fireproofness", RefSemanticPrefix = "0112/2///61360_4#AAH038#002", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData13 = new Parameters() { ID = 013, Parameter = "package colour", RefSemanticPrefix = "0112/2///61360_4#AAF128#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData14 = new Parameters() { ID = 014, Parameter = "socket type", RefSemanticPrefix = "0112/2///61360_4#AAF148#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData15 = new Parameters() { ID = 015, Parameter = "terminal connection type", RefSemanticPrefix = "0112/2///61360_4#AAF435#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData16 = new Parameters() { ID = 016, Parameter = "simultaneity factor", RefSemanticPrefix = "0112/2///61360_4#AAF436#001", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData17 = new Parameters() { ID = 017, Parameter = "packaging quality", RefSemanticPrefix = "0112/2///61360_4#AAH001#002", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData18 = new Parameters() { ID = 018, Parameter = "fixing method", RefSemanticPrefix = "0112/2///61360_4#AAH003#002", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData19 = new Parameters() { ID = 019, Parameter = "mounting means", RefSemanticPrefix = "0112/2///61360_4#AAH004#002", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData20 = new Parameters() { ID = 020, Parameter = "protective earth", RefSemanticPrefix = "0112/2///61360_4#AAH006#002", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData21 = new Parameters() { ID = 021, Parameter = "pollution degree", RefSemanticPrefix = "0112/2///61360_4#AAH023#002", Unit = "", Value = "" };
            Parameters IRDIMiscelliniousData22 = new Parameters() { ID = 022, Parameter = "acceleration strength (a_acc)", RefSemanticPrefix = "0112/2///61360_4#AAH029#002", Unit = "m/s2", Value = "" };
            Parameters IRDIMiscelliniousData23 = new Parameters() { ID = 023, Parameter = "shock resistance (a_shock)", RefSemanticPrefix = "0112/2///61360_4#AAH030#002", Unit = "m/s2", Value = "" };
            Parameters IRDIMiscelliniousData24 = new Parameters() { ID = 024, Parameter = "bump resistance (a_bump)", RefSemanticPrefix = "0112/2///61360_4#AAH031#002", Unit = "m/s2", Value = "" };
            Parameters IRDIMiscelliniousData25 = new Parameters() { ID = 025, Parameter = "housing colour", RefSemanticPrefix = "0112/2///61360_4#AAH065#002", Unit = "", Value = "" };


            Dictionary<int, Parameters> IRDIMiscelliniousDataParameters = new Dictionary<int, Parameters>();
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData1.ID, IRDIMiscelliniousData1);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData2.ID, IRDIMiscelliniousData2);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData3.ID, IRDIMiscelliniousData3);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData4.ID, IRDIMiscelliniousData4);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData5.ID, IRDIMiscelliniousData5);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData6.ID, IRDIMiscelliniousData6);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData7.ID, IRDIMiscelliniousData7);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData8.ID, IRDIMiscelliniousData8);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData9.ID, IRDIMiscelliniousData9);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData10.ID, IRDIMiscelliniousData10);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData11.ID, IRDIMiscelliniousData11);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData12.ID, IRDIMiscelliniousData12);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData13.ID, IRDIMiscelliniousData13);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData14.ID, IRDIMiscelliniousData14);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData15.ID, IRDIMiscelliniousData15);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData16.ID, IRDIMiscelliniousData16);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData17.ID, IRDIMiscelliniousData17);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData18.ID, IRDIMiscelliniousData18);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData19.ID, IRDIMiscelliniousData19);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData20.ID, IRDIMiscelliniousData20);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData21.ID, IRDIMiscelliniousData21);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData22.ID, IRDIMiscelliniousData22);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData23.ID, IRDIMiscelliniousData23);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData24.ID, IRDIMiscelliniousData24);
            IRDIMiscelliniousDataParameters.Add(IRDIMiscelliniousData25.ID, IRDIMiscelliniousData25);



            return IRDIMiscelliniousDataParameters;
        }
        public Dictionary<int, Parameters> IRDICableDimensionData()
        {
            Parameters IRDIFemaleConnectorDimensioneData1 = new Parameters() { ID = 001, Parameter = "BA", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIFemaleConnectorDimensioneData2 = new Parameters() { ID = 002, Parameter = "BB", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIFemaleConnectorDimensioneData3 = new Parameters() { ID = 003, Parameter = "BC", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };
            Parameters IRDIFemaleConnectorDimensioneData4 = new Parameters() { ID = 004, Parameter = "BD", RefSemanticPrefix = "", Unit = "mm", Min = "", Nom = "", Max = "" };

            Dictionary<int, Parameters> IRDIFemaleConnectorDimensioneDataParameters = new Dictionary<int, Parameters>();
            IRDIFemaleConnectorDimensioneDataParameters.Add(IRDIFemaleConnectorDimensioneData1.ID, IRDIFemaleConnectorDimensioneData1);
            IRDIFemaleConnectorDimensioneDataParameters.Add(IRDIFemaleConnectorDimensioneData2.ID, IRDIFemaleConnectorDimensioneData2);
            IRDIFemaleConnectorDimensioneDataParameters.Add(IRDIFemaleConnectorDimensioneData3.ID, IRDIFemaleConnectorDimensioneData3);
            IRDIFemaleConnectorDimensioneDataParameters.Add(IRDIFemaleConnectorDimensioneData4.ID, IRDIFemaleConnectorDimensioneData4);

            return IRDIFemaleConnectorDimensioneDataParameters;
        }
    }


}
