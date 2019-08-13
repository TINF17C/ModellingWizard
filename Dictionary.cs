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
        public Dictionary<int, Parameters> IRDIMaterialData()
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
            Parameters IRDIElectricalData10 = new Parameters() { ID = 0010, Parameter = "Contact current max (I_cont)", RefSemanticPrefix = "0112/2///61360_4#AAE358#001", Unit = "A" };

            Dictionary<int, Parameters> IRDIElectricalDataParameters = new Dictionary<int, Parameters>();
            IRDIElectricalDataParameters.Add(IRDIElectricalData1.ID, IRDIElectricalData1);
            IRDIElectricalDataParameters.Add(IRDIElectricalData2.ID, IRDIElectricalData2);
            IRDIElectricalDataParameters.Add(IRDIElectricalData3.ID, IRDIElectricalData3);
            IRDIElectricalDataParameters.Add(IRDIElectricalData4.ID, IRDIElectricalData4);
            IRDIElectricalDataParameters.Add(IRDIElectricalData5.ID, IRDIElectricalData5);
            IRDIElectricalDataParameters.Add(IRDIElectricalData6.ID, IRDIElectricalData6);
            IRDIElectricalDataParameters.Add(IRDIElectricalData7.ID, IRDIElectricalData7);
            IRDIElectricalDataParameters.Add(IRDIElectricalData8.ID, IRDIElectricalData8);
            IRDIElectricalDataParameters.Add(IRDIElectricalData10.ID, IRDIElectricalData10);


            return IRDIElectricalDataParameters;

        }
        public Dictionary<int, Parameters> IRDIMountingSquareFlangeData()
        {
            Parameters IRDIMountingSquareFlangeData1 = new Parameters() { ID = 001, Parameter = "Square flange Length", RefSemanticPrefix = "", Unit = "mm", Value = "20" };
            Parameters IRDIMountingSquareFlangeData2 = new Parameters() { ID = 002, Parameter = "Flange hole Diameter", RefSemanticPrefix = "", Unit = "mm", Value = "3.2" };
            Parameters IRDIMountingSquareFlangeData3 = new Parameters() { ID = 003, Parameter = "Flange holes length", RefSemanticPrefix = "", Unit = "mm", Value = "14" };
            Parameters IRDIMountingSquareFlangeData4 = new Parameters() { ID = 004, Parameter = "Flange Thickness", RefSemanticPrefix = "", Unit = "mm", Value = "4" };
            Parameters IRDIMountingSquareFlangeData5 = new Parameters() { ID = 005, Parameter = "Connector Length", RefSemanticPrefix = "", Unit = "mm", Value = "28" };
            Parameters IRDIMountingSquareFlangeData6 = new Parameters() { ID = 006, Parameter = "Connector Diameter", RefSemanticPrefix = "", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData7 = new Parameters() { ID = 007, Parameter = "Connector Thread Pitch", RefSemanticPrefix = "", Unit = "mm", Value = "" };

            Dictionary<int, Parameters> IRDIMountingSquareFlangeDataParameters = new Dictionary<int, Parameters>();
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData1.ID, IRDIMountingSquareFlangeData1);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData2.ID, IRDIMountingSquareFlangeData2);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData3.ID, IRDIMountingSquareFlangeData3);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData4.ID, IRDIMountingSquareFlangeData4);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData5.ID, IRDIMountingSquareFlangeData5);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData6.ID, IRDIMountingSquareFlangeData6);
            IRDIMountingSquareFlangeDataParameters.Add(IRDIMountingSquareFlangeData7.ID, IRDIMountingSquareFlangeData7);

            return IRDIMountingSquareFlangeDataParameters;
        }
        public Dictionary<int, Parameters> IRDIMountingSingleHoleData()
        {
            Parameters IRDIMountingSingleHoleData1 = new Parameters() { ID = 001, Parameter = "Single Hole Diameter", RefSemanticPrefix = "", Unit = "mm", Value = "20" };
            Parameters IRDIMountingSingleHoleData2 = new Parameters() { ID = 002, Parameter = "Single Hole Pitch", RefSemanticPrefix = "", Unit = "mm", Value = "3.2" };
            Parameters IRDIMountingSingleHoleData3 = new Parameters() { ID = 003, Parameter = "Length of Wire ends", RefSemanticPrefix = "", Unit = "mm", Value = "14" };
            Parameters IRDIMountingSquareFlangeData5 = new Parameters() { ID = 004, Parameter = "Connector Length", RefSemanticPrefix = "", Unit = "mm", Value = "28" };
            Parameters IRDIMountingSquareFlangeData6 = new Parameters() { ID = 005, Parameter = "Connector Diameter", RefSemanticPrefix = "", Unit = "mm", Value = "" };
            Parameters IRDIMountingSquareFlangeData7 = new Parameters() { ID = 006, Parameter = "Connector Thread Pitch", RefSemanticPrefix = "", Unit = "mm", Value = "" };

            Dictionary<int, Parameters> IRDIMountingSingleHoleDataParameters = new Dictionary<int, Parameters>();
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSingleHoleData1.ID, IRDIMountingSingleHoleData1);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSingleHoleData2.ID, IRDIMountingSingleHoleData2);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSingleHoleData3.ID, IRDIMountingSingleHoleData3);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData5.ID, IRDIMountingSquareFlangeData5);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData6.ID, IRDIMountingSquareFlangeData6);
            IRDIMountingSingleHoleDataParameters.Add(IRDIMountingSquareFlangeData7.ID, IRDIMountingSquareFlangeData7);


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
    }


}
