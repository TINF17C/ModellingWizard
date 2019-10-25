using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aml.Engine.AmlObjects;
using Aml.Engine.CAEX;
using System.IO;
using System.IO.Packaging;
using System.Xml;
using System.Collections;

namespace Aml.Editor.Plugin
{
    public partial class DeviceDescription : UserControl
    {
        private MWController mWController;



        bool isEditing = false;
        AnimationClass AMC = new AnimationClass();

        public DeviceDescription()
        {
            InitializeComponent();
        }

        public DeviceDescription(MWController mWController)
        {
            this.mWController = mWController;
            InitializeComponent();
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            mWController.ChangeGui(MWController.MWGUIType.Start);
            clear();

        }

        private void ClearDeviceDataBtn_Click(object sender, EventArgs e)
        {
            vendorNameTxtBx.Text = "";
            productRangeTxtBx.Text = "";
            vendorIDTxtBx.Text = "";
            hardwareReleaseTxtBx.Text = "";
            softwareReleaseTxtBx.Text = "";
            productNumberTxtBx.Text = "";
            orderNumberTxtBx.Text = "";
            vendorHomepageTxtBx.Text = "";
            communicationTechnologyTxtBx.Text = "";
            opTempMaxTxtBx.Text = "";
            opTempMinTxtBx.Text = "";
            producTxtBx.Text = "";
            productGroupTxtBx.Text = "";
            deviceNameTxtBx.Text = "";
            deviceIDTxtBx.Text = "";
            productFamilyTxtBx.Text = "";

            vendorNameRefSemanticBtn.Text = "";
            deviceNameRefSemanticBtn.Text = "";
            vendorHomepageRefSemanticBtn.Text = "";
            productFamilyRefSemanticBtn.Text = "";

            vendorNameRefSemanticBtn.Visible = false;
            deviceNameRefSemanticBtn.Visible = false;
            vendorHomepageRefSemanticBtn.Visible = false;
            productFamilyRefSemanticBtn.Visible = false;
        }

        private void AddSemanticSystemBtn_Click(object sender, EventArgs e)
        {
            // call "Datatables" class 
            Datatables dataTables = new Datatables();

            if (semanticSystemTextBox.Text == "IEC-CDD" && dataHierarchyTreeView.Nodes.Count == 0)
            {
                //Initializing "Pararmeter of data table" and assigning a name.
                DataTable datatableheaderIRDIID = dataTables.Parametersdatatable();
                DataTable datatableheaderIRDIProductDetails = dataTables.Parametersdatatable();

                // Calling "IRDI Dictionary class" and retreving required Dictionary from it in this case the Dictionary is "IRDIIdentificationdata"
                //Later values in the dictionary are assigned to respective "Data Grid Views"
                DictionaryIRDI DIRDI = new DictionaryIRDI();
                Dictionary<int, Parameters> IRDIID = DIRDI.IRDIIdentificationdata();

                dataTables.CreateDataTableWith3Columns(IRDIID, datatableheaderIRDIID, identificationDataGridView);



                // Dispaly buttons

                AMC.DispalySemanticBtn(vendorNameRefSemanticBtn, identificationDataGridView, "Manufacturer Name");
                AMC.DispalySemanticBtn(deviceNameRefSemanticBtn, identificationDataGridView, "Product Name");
                AMC.DispalySemanticBtn(vendorHomepageRefSemanticBtn, identificationDataGridView, "Product Online Information URL");
                AMC.DispalySemanticBtn(productFamilyRefSemanticBtn, identificationDataGridView, "Product Family");

            }
            if (dataHierarchyTreeView.Nodes.Count == 0)
            {
                // Tree view updates on the "dataHiereachyTreeView"
                //TreeNode node;
                TreeNode node1;
                TreeNode node2;
                TreeNode node3;
                TreeNode node4;

                //node = dataHierarchyTreeView.Nodes.Add("Device Data");

                node1 = dataHierarchyTreeView.Nodes.Add("Generic Data");
                node1.Nodes.Add("Identification Data");
                node1.Nodes.Add("Commercial Data");
                node1.Nodes.Add("Product Data");

                node2 = dataHierarchyTreeView.Nodes.Add("Interfaces");
                node2.Nodes.Add("Electrical interface");
                node2.Nodes.Add("Sensor interface");
                node2.Nodes.Add("Mechanical interface");

                node3 = dataHierarchyTreeView.Nodes.Add("Field Attachables");
                node3.Nodes.Add("Add logos");
                node3.Nodes.Add("Add Documents");

                node4 = dataHierarchyTreeView.Nodes.Add("Docs");
                node4.Nodes.Add("Add Docs");

                // Call "ThreeParametersdatatable" method from "Dictionary Class" and store in "datatablesheader" Seperately for PD,POD,PPD,MD
                DataTable datatableheadersPD = dataTables.Parametersdatatable();
                DataTable datatableheadersPPD = dataTables.Parametersdatatable();
                DataTable datatableheadersPOD = dataTables.Parametersdatatable();
                DataTable datatableheadersMD = dataTables.Parametersdatatable();

                // Call ""Commercialdatadictionary Class" From "Dictioanry File/Class" and store in "CDD"
                CommercialDataDictionary CDD = new CommercialDataDictionary();
                // Call "Product Details" method from "CommercialDataDictionary" and store in "PD"
                Dictionary<int, Parameters> PD = CDD.ProductDetails();

                // Call "Product Price Details" method from "CommercialDataDictionary" and store in "PPD"
                Dictionary<int, Parameters> PPD = CDD.ProductPriceDetails();


                // Call "Product Order Details" method from "CommercialDataDictionary" and store in "POD"
                Dictionary<int, Parameters> POD = CDD.ProductOrderDetails();


                // Call "Manufacturer Details" method from "CommercialDataDictionary" and store in "MD"
                Dictionary<int, Parameters> MD = CDD.ManufacturerDetails();

                //
                dataTables.CreateDataTableWith3Columns(PD, datatableheadersPD, dataGridViewProductDetails);

                dataTables.CreateDataTableWith3Columns(PPD, datatableheadersPPD, dataGridViewProductPriceDetails);
                dataTables.CreateDataTableWith3Columns(POD, datatableheadersPOD, dataGridViewProductOrderDetails);
                dataTables.CreateDataTableWith3Columns(MD, datatableheadersMD, dataGridViewManufacturerDetails);
                // Intetating dictionary values to DataGridViews 

            }





        }

        // This Save button method takes multiple electrical interfaces intances and store them in a list 

        List<List<ElectricalParameters>> electricalInterfaceslistoflists = new List<List<ElectricalParameters>>();

        public void saveElectricalInterfaceBtn_Click(object sender, EventArgs e)
        {

            if (electricalInterfacesComboBox.Items.Count == Convert.ToInt32(electricalInterfacenumbTxtBox.Text))
            {
                MessageBox.Show("Specified Interfaces Limit has been reached, Change Number of Interfaces to add more Interfaces", " Interface Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                if (electricalInterfacesComboBox.Items.Count < Convert.ToInt32(electricalInterfacenumbTxtBox.Text))
                {
                    electricalInterfacesComboBox.Items.Add(connectorCombBox.Text);
                }
                ElectricalParameters eachElectricalParameter = new ElectricalParameters();
                eachElectricalParameter.Connector = Convert.ToString(connectorCombBox.Text);
                eachElectricalParameter.ConnectorCode = Convert.ToString(connectorCodeCombBox.Text);
                eachElectricalParameter.ConnectorType = Convert.ToString(connectorTypeCombBox.Text);
                eachElectricalParameter.Pins = Convert.ToString(numbOfPinsTxtBox.Text);

                List<ElectricalParametersInElectricalDataDataGridView> listofElectricalParametersFromElectricalDataDataGridView = new List<ElectricalParametersInElectricalDataDataGridView>();
                if (electricalDataDataGridView != null)
                {

                    int i = 0;
                    int j = electricalDataDataGridView.Rows.Count;
                    if (i <= 0)
                    {
                        while (i < j)
                        {
                            ElectricalParametersInElectricalDataDataGridView parametersFromElectricalDataDataGrid = new ElectricalParametersInElectricalDataDataGridView();
                            try
                            {
                                parametersFromElectricalDataDataGrid.ReferenceID = Convert.ToString(electricalDataDataGridView.Rows[i].Cells[0].Value);
                                parametersFromElectricalDataDataGrid.Attributes = Convert.ToString(electricalDataDataGridView.Rows[i].Cells[1].Value);
                                parametersFromElectricalDataDataGrid.Values = Convert.ToString(electricalDataDataGridView.Rows[i].Cells[2].Value);
                                parametersFromElectricalDataDataGrid.Units = Convert.ToString(electricalDataDataGridView.Rows[i].Cells[3].Value);
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }


                            listofElectricalParametersFromElectricalDataDataGridView.Add(parametersFromElectricalDataDataGrid);
                            i++;

                        }
                    }
                }
                eachElectricalParameter.listofElectricalDataDataGridViewParameters = listofElectricalParametersFromElectricalDataDataGridView;
                List<PinParametersInPinInfoDataGridView> listofPinFrompinInfoDataGridView = new List<PinParametersInPinInfoDataGridView>();
                if (pinInfoDataGridView != null)
                {

                    int i = 0;
                    int j = pinInfoDataGridView.Rows.Count;
                    if (i <= 0)
                    {
                        while (i < j)
                        {
                            PinParametersInPinInfoDataGridView parametersFrompinInfoDataGridView = new PinParametersInPinInfoDataGridView();
                            try
                            {
                                if (pinInfoDataGridView.Rows[i].Cells[0].Value != null)
                                {
                                    parametersFrompinInfoDataGridView.PinNumber = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[0].Value);
                                    parametersFrompinInfoDataGridView.ReferenceID = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[1].Value);
                                    parametersFrompinInfoDataGridView.Attributes = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[2].Value);
                                    parametersFrompinInfoDataGridView.Values = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[3].Value);
                                    parametersFrompinInfoDataGridView.Units = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[4].Value);
                                }

                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }


                            listofPinFrompinInfoDataGridView.Add(parametersFrompinInfoDataGridView);
                            i++;

                        }
                    }
                }
                eachElectricalParameter.listOfPinInfoDataGridViewParameters = listofPinFrompinInfoDataGridView;
                List<ElectricalParameters> listOfElectrialParameters = new List<ElectricalParameters>();

                listOfElectrialParameters.Add(eachElectricalParameter);


                if (electricalInterfacesComboBox.Items.Count <= Convert.ToInt32(electricalInterfacenumbTxtBox.Text) && electricalInterfacesComboBox.Items.Count != 15)
                {

                    switch (electricalInterfacesComboBox.Items.Count)
                    {
                        case 1:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 1 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                            break;
                        case 2:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 2 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 3:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 3 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 4:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 4 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 5:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 5 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 6:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 6 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 7:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 7 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 8:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 8 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 9:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 9 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 10:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 10 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 11:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 11 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 12:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 12 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 13:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 13 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 14:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 14 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 15:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 15 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        default:
                            break;
                    }



                }

                /*else
                {
                    MessageBox.Show("Specified Interfaces Limit has been reached, Change Number of Interfaces to add more Interfaces", " Interface Limit Reached",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }*/

            }




        }

        private void GenerateAML_Click(object sender, EventArgs e)
        {
            // Create a new Device
            var device = new MWDevice();

            // Read all the input fields and write them to the device data
            if (communicationTechnologyTxtBx.SelectedItem != null)
            {
                device.deviceType = communicationTechnologyTxtBx.SelectedItem.ToString();
            }
            else
            {
                device.deviceType = "";
            }

            // Check if there was an input in this field, if so: try to convert it to integer
            if (!String.IsNullOrWhiteSpace(vendorIDTxtBx.Text))
            {
                try { device.vendorID = Convert.ToInt32(vendorIDTxtBx.Text); } catch (Exception) { MessageBox.Show("Warning: Vendor ID must be number.\n please correct input"); }
            }
            // Check if there was an input in this field, if so: try to convert it to integer
            if (!String.IsNullOrWhiteSpace(productRangeTxtBx.Text))
            {
                try { device.deviceID = Convert.ToInt32(deviceIDTxtBx.Text); } catch (Exception) { MessageBox.Show("Device ID is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            device.vendorName = vendorNameTxtBx.Text;
            device.vendorHomepage = vendorHomepageTxtBx.Text;
            device.deviceName = deviceNameTxtBx.Text;
            device.productRange = productRangeTxtBx.Text;
            device.productName = productNumberTxtBx.Text;
            device.orderNumber = orderNumberTxtBx.Text;
            device.productText = producTxtBx.Text;
            device.harwareRelease = hardwareReleaseTxtBx.Text;
            device.softwareRelease = softwareReleaseTxtBx.Text;
            device.productFamily = productFamilyTxtBx.Text;
            device.productGroup = productGroupTxtBx.Text;
            device.semanticsystem = semanticSystemTextBox.Text;
            device.semanticSystemClassificationSystem = classificationSystemTxtBx.Text;
            device.semanticSystemVersion = semanticSystemVersionTxtBx.Text;


            device.ipProtection = ipProtectionTxtBx.Text;
            if (!String.IsNullOrWhiteSpace(opTempMaxTxtBx.Text))
            {
                try { device.minTemperature = Convert.ToDouble(opTempMinTxtBx.Text); } catch (Exception) { device.minTemperature = Double.NaN; MessageBox.Show("Min Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            if (!String.IsNullOrWhiteSpace(opTempMaxTxtBx.Text))
            {
                try { device.maxTemperature = Convert.ToDouble(opTempMaxTxtBx.Text); } catch (Exception) { device.maxTemperature = Double.NaN; MessageBox.Show("Max Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }


            device.dataGridParametersLists = new List<DataGridParameters>();

            if (identificationDataGridView != null)
            {

                int i = 0;
                int j = identificationDataGridView.Rows.Count;
                if (i <= 0 && j != i)
                {
                    while (i < j)
                    {
                        DataGridParameters parametersFromDataGrid = new DataGridParameters();
                        try
                        {
                            parametersFromDataGrid.RefSemantics = Convert.ToString(identificationDataGridView.Rows[i].Cells[0].Value);
                            parametersFromDataGrid.Attributes = Convert.ToString(identificationDataGridView.Rows[i].Cells[1].Value);
                            parametersFromDataGrid.Values = Convert.ToString(identificationDataGridView.Rows[i].Cells[2].Value);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning); }

                        device.dataGridParametersLists.Add(parametersFromDataGrid);
                        i++;
                    }

                }
            }

            device.dataGridProductDetailsParametersLists = new List<DataGridProductDetailsParameters>();
            if (dataGridViewProductDetails != null)
            {

                int i = 0;
                int j = dataGridViewProductDetails.Rows.Count;
                if (i <= 0)
                {
                    while (i < j)
                    {
                        DataGridProductDetailsParameters parametersFromProductDetailsDataGrid = new DataGridProductDetailsParameters();
                        try
                        {
                            parametersFromProductDetailsDataGrid.PDRefSemantics = Convert.ToString(dataGridViewProductDetails.Rows[i].Cells[0].Value);
                            parametersFromProductDetailsDataGrid.PDAttributes = Convert.ToString(dataGridViewProductDetails.Rows[i].Cells[1].Value);
                            parametersFromProductDetailsDataGrid.PDvalues = Convert.ToString(dataGridViewProductDetails.Rows[i].Cells[2].Value);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning); }

                        device.dataGridProductDetailsParametersLists.Add(parametersFromProductDetailsDataGrid);
                        i++;

                    }
                }

            }
            device.dataGridProductOrderDetailsParametersLists = new List<DataGridProductOrderDetailsParameters>();
            if (dataGridViewProductOrderDetails != null)
            {

                int i = 0;
                int j = dataGridViewProductOrderDetails.Rows.Count;
                if (i <= 0)
                {
                    while (i < j)
                    {
                        DataGridProductOrderDetailsParameters parametersFromProductOrderDetailsDataGrid = new DataGridProductOrderDetailsParameters();
                        try
                        {
                            parametersFromProductOrderDetailsDataGrid.PODRefSemantics = Convert.ToString(dataGridViewProductOrderDetails.Rows[i].Cells[0].Value);
                            parametersFromProductOrderDetailsDataGrid.PODAttributes = Convert.ToString(dataGridViewProductOrderDetails.Rows[i].Cells[1].Value);
                            parametersFromProductOrderDetailsDataGrid.PODvalues = Convert.ToString(dataGridViewProductOrderDetails.Rows[i].Cells[2].Value);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning); }

                        device.dataGridProductOrderDetailsParametersLists.Add(parametersFromProductOrderDetailsDataGrid);
                        i++;

                    }
                }


            }
            device.dataGridProductPriceDetailsParametersLists = new List<DataGridProductPriceDetailsParameters>();
            if (dataGridViewProductPriceDetails != null)
            {

                int i = 0;
                int j = dataGridViewProductPriceDetails.Rows.Count;
                if (i <= 0)
                {
                    while (i < j)
                    {
                        DataGridProductPriceDetailsParameters parametersFromProductPriceDetailsDataGrid = new DataGridProductPriceDetailsParameters();
                        try
                        {
                            parametersFromProductPriceDetailsDataGrid.PPDRefSemantics = Convert.ToString(dataGridViewProductPriceDetails.Rows[i].Cells[0].Value);
                            parametersFromProductPriceDetailsDataGrid.PPDAttributes = Convert.ToString(dataGridViewProductPriceDetails.Rows[i].Cells[1].Value);
                            parametersFromProductPriceDetailsDataGrid.PPDvalues = Convert.ToString(dataGridViewProductPriceDetails.Rows[i].Cells[2].Value);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning); }

                        device.dataGridProductPriceDetailsParametersLists.Add(parametersFromProductPriceDetailsDataGrid);
                        i++;

                    }
                }


            }
            device.dataGridManufacturerDetailsParametersLists = new List<DataGridManufacturerDetailsParameters>();
            if (dataGridViewManufacturerDetails != null)
            {

                int i = 0;
                int j = dataGridViewManufacturerDetails.Rows.Count;
                if (i <= 0)
                {
                    while (i < j)
                    {
                        DataGridManufacturerDetailsParameters parametersFromManufacturerDetailsDataGrid = new DataGridManufacturerDetailsParameters();
                        try
                        {
                            parametersFromManufacturerDetailsDataGrid.MDRefSemantics = Convert.ToString(dataGridViewManufacturerDetails.Rows[i].Cells[0].Value);
                            parametersFromManufacturerDetailsDataGrid.MDAttributes = Convert.ToString(dataGridViewManufacturerDetails.Rows[i].Cells[1].Value);
                            parametersFromManufacturerDetailsDataGrid.MDvalues = Convert.ToString(dataGridViewManufacturerDetails.Rows[i].Cells[2].Value);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning); }

                        device.dataGridManufacturerDetailsParametersLists.Add(parametersFromManufacturerDetailsDataGrid);
                        i++;

                    }
                }


            }
            device.ElectricalInterfaceInstances = electricalInterfaceslistoflists;

            // storing user defined values of Attachebles data grid view in to list 

            device.dataGridAttachablesParametrsList = new List<AttachablesDataGridViewParameters>();
            if (attachablesInfoDataGridView != null)
            {
                int i = 0;
                int j = attachablesInfoDataGridView.Rows.Count - 1;
                if (i <= 0)
                {
                    while (i < j)
                    {

                        AttachablesDataGridViewParameters parametersFromAttachablesDataGrid = new AttachablesDataGridViewParameters();

                        try
                        {
                            parametersFromAttachablesDataGrid.AutomationMlRole = Convert.ToString(attachablesInfoDataGridView.Rows[i].Cells[0].Value);
                            parametersFromAttachablesDataGrid.FileLocation = Convert.ToString(attachablesInfoDataGridView.Rows[i].Cells[1].Value);
                            parametersFromAttachablesDataGrid.FileName = Convert.ToString(attachablesInfoDataGridView.Rows[i].Cells[2].Value);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning); }

                        device.dataGridAttachablesParametrsList.Add(parametersFromAttachablesDataGrid);
                        i++;

                    }
                }
            }

            // Pass the device to the controller
            string result = mWController.CreateDeviceOnClick(device, isEditing);
            clear();
            // Display the result
            if (result != null)
            {
                // Display error Dialog
                MessageBox.Show(result);
            }
            // Assigning values and parameters in "Identification data grid" to properties given in class "DatatableParametersCarrier" in MWDevice


        }
        public void clear()
        {
            // vendorNameTxtBx.Text = "";
            productRangeTxtBx.Text = "";
            //vendorIDTxtBx.Text = "";
            hardwareReleaseTxtBx.Text = "";
            softwareReleaseTxtBx.Text = "";
            productNumberTxtBx.Text = "";
            orderNumberTxtBx.Text = "";
            // vendorHomepageTxtBx.Text = "";
            communicationTechnologyTxtBx.Text = "";
            opTempMaxTxtBx.Text = "";
            opTempMinTxtBx.Text = "";
            producTxtBx.Text = "";
            //productGroupTxtBx.Text = "";
            deviceNameTxtBx.Text = "";
            // productRangeTxtBx.Text = "";
            //productFamilyTxtBx.Text = "";



            // All "Data Grid Views" gets cleared
            dataGridViewManufacturerDetails.Rows.Clear();
            dataGridViewProductDetails.Rows.Clear();
            dataGridViewProductOrderDetails.Rows.Clear();
            dataGridViewProductPriceDetails.Rows.Clear();
            identificationDataGridView.Rows.Clear();
            semanticSystemTextBox.Text = "";

            // clear tree view
            dataHierarchyTreeView.Nodes.Clear();

            // reset switch case to case 1 again.
            //electricalInterfacesComboBox.Items.Clear();

        }



        private void IdentificationDataBtn_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(identificationDataPanel, identificationDataBtn);
        }



        private void CommercialDataBtn_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(commercialDataPanel, commercialDataBtn);
        }

        private void ClearSemanticSystemBtn_Click(object sender, EventArgs e)
        {
            dataGridViewManufacturerDetails.Rows.Clear();
            dataGridViewProductDetails.Rows.Clear();
            dataGridViewProductOrderDetails.Rows.Clear();
            dataGridViewProductPriceDetails.Rows.Clear();
            identificationDataGridView.Rows.Clear();
            semanticSystemTextBox.Text = "";
            dataHierarchyTreeView.Nodes.Clear();

            // hide ref semantic buttons 

        }

        private void VendorNameRefSemanticBtn_Click(object sender, EventArgs e)
        {
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                AMC.SemanticSystemOpener(vendorNameRefSemanticBtn.Text);
            }

        }

        private void VendorLogoDisplayBtn_Click(object sender, EventArgs e)
        {

        }

        private void DeviceNameRefSemanticBtn_Click(object sender, EventArgs e)
        {
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                AMC.SemanticSystemOpener(deviceNameRefSemanticBtn.Text);
            }
        }

        private void ProductFamilyRefSemanticBtn_Click(object sender, EventArgs e)
        {
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                AMC.SemanticSystemOpener(productFamilyRefSemanticBtn.Text);
            }
        }

        private void VendorHomepageRefSemanticBtn_Click(object sender, EventArgs e)
        {
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                AMC.SemanticSystemOpener(vendorHomepageRefSemanticBtn.Text);
            }
        }

        private void dataHierarchyTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (dataHierarchyTreeView.SelectedNode.Text == "Identification Data")
            {
                dataTabControl.SelectTab("genericDataTabPage");
                AMC.WindowSizeChanger(identificationDataPanel);
            }
            if (dataHierarchyTreeView.SelectedNode.Text == "Commercial Data")
            {
                dataTabControl.SelectTab("genericDataTabPage");
                AMC.WindowSizeChanger(commercialDataPanel);
            }

            if (dataHierarchyTreeView.SelectedNode.Text == "Electrical interface")
            {
                dataTabControl.SelectTab("InterfacesDataTabPage");
                AMC.WindowSizeChanger(electricalInterfacePanel);
            }
            if (dataHierarchyTreeView.SelectedNode.Text == "Add Docs")
            {
                dataTabControl.SelectTab("DocsTabPage");
                AMC.WindowSizeChanger(addPicturesandDocsPanel);
            }
            dataHierarchyTreeView.SelectedNode = null;
        }



        private void electricalInterfaceBtn_Click_1(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(electricalInterfacePanel, electricalInterfaceBtn);
        }

        private void addElectricalInterfacesBtn_Click(object sender, EventArgs e)
        {


        }

        private void addElectricalDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectorCombBox.SelectedItem != null && connectorTypeCombBox.SelectedItem != null && electricalDataDataGridView.Rows.Count <= 1)
                {
                    Datatables datatables = new Datatables();

                    DataTable datatableheadersIRDIED = datatables.Parametersdatatable();

                    DictionaryIRDI DIRDI = new DictionaryIRDI();

                    Dictionary<int, Parameters> IRDIED = DIRDI.IRDIElectricalData();

                    datatables.CreateDataTableWith4Columns(IRDIED, datatableheadersIRDIED, electricalDataDataGridView);


                }
                else
                {
                    MessageBox.Show(" 'Select Connector from Connector Combo Box' or 'Select Connector Type from Connector Type combo box' ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception)
            {


            }



        }
        private void clearElectricalDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (electricalDataDataGridView.CurrentCell != null)
                {
                    int rowIndex = electricalDataDataGridView.CurrentCell.RowIndex;
                    electricalDataDataGridView.Rows.RemoveAt(rowIndex);
                }

            }
            catch (Exception) { }
        }

        private void addPinsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (numbOfPinsTxtBox.Text != null && connectorCodeCombBox.SelectedItem != null && pinInfoDataGridView.Rows.Count <= 1)
                {

                    int countofpins = 0;
                    string enteredvalue = numbOfPinsTxtBox.Text;
                    int convertedtonumber = Convert.ToInt32(enteredvalue);
                    for (int i = 0; i < convertedtonumber; i++)
                    {
                        pinInfoDataGridView.Rows.Add();
                        pinInfoDataGridView.Rows[countofpins + i].Cells[0].Value = (1 + i).ToString();
                    }
                    numbOfPinsTxtBox.Text = null;
                }
                else
                {
                    MessageBox.Show("Missing Number of Pins or Connector Code", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            catch (Exception)
            {

            }
        }

        private void clearPinsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (pinInfoDataGridView.CurrentCell != null)
                {
                    int rowIndex = pinInfoDataGridView.CurrentCell.RowIndex;
                    pinInfoDataGridView.Rows.RemoveAt(rowIndex);
                }

            }
            catch (Exception) { }
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(addPicturesandDocsPanel, addBtn);
        }

        private void addRole_Click(object sender, EventArgs e)
        {

            if (automationMLRoleCmbBx.Text != null && attachablesInfoDataGridView.Rows.Count > 0)
            {

                string searchValue = automationMLRoleCmbBx.Text;
                string mid = "_";

                int result = 1;
                string end = Convert.ToString(result);
                string final = searchValue + mid + end;

                List<string> listofstrings = new List<string>();
                List<int> listofintegers = new List<int>();

                int i;
                int result3;
                string ultrafinal = String.Empty;
                attachablesInfoDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                foreach (DataGridViewRow row in attachablesInfoDataGridView.Rows)
                {
                    try
                    {
                        if (row.Cells[0].Value == null)
                        {

                            AMLfileLabel.Text = automationMLRoleCmbBx.Text;
                            AMLURLLabel.Text = automationMLRoleCmbBx.Text;

                        }
                    }
                    catch (Exception)
                    {


                    }

                }
                foreach (DataGridViewRow eachrow in attachablesInfoDataGridView.Rows)
                {
                    try
                    {
                        if (eachrow.Cells[0].Value.Equals(searchValue))
                        {

                            foreach (DataGridViewRow eachrow3 in attachablesInfoDataGridView.Rows)
                            {
                                try
                                {
                                    if (eachrow3.Cells[0].Value != null && eachrow3.Cells[0].Value.ToString().Contains(searchValue))
                                    {
                                        string eachstringindataGridView = eachrow3.Cells[0].Value.ToString();
                                        listofstrings.Add(eachstringindataGridView);
                                    }

                                }
                                catch (Exception)
                                {

                                    throw;
                                }


                            }
                            foreach (string eachstring in listofstrings)
                            {
                                bool success = int.TryParse(new string(eachstring.SkipWhile(x => !char.IsDigit(x)).TakeWhile(x => char.IsDigit(x)).ToArray()), out i);
                                if (success == false)
                                {
                                    i = 0;
                                }
                                listofintegers.Add(i);
                            }

                            result3 = listofintegers.Max();

                            ultrafinal = searchValue + mid + Convert.ToString(++result3);

                            AMLfileLabel.Text = ultrafinal;
                            AMLURLLabel.Text = ultrafinal;

                        }

                    }
                    catch (Exception)
                    {


                    }


                }

            }
            if (automationMLRoleCmbBx.SelectedItem == null)
            {
                MessageBox.Show("Select AutomationML Role type from the combo box", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void clearRole_Click(object sender, EventArgs e)
        {
            if (selectURLBtn.Text != null && selectFileBtn.Text != null)
            {
                AMLfileLabel.Text = "";
                AMLURLLabel.Text = "";
                automationMLRoleCmbBx.SelectedItem = null;
                selectedFileLocationTxtBx.Text = null;
                selectedFileURLTextBox.Text = null;

            }
        }

        private void selectFileBtn_Click(object sender, EventArgs e)
        {
            if (AMLfileLabel.Text != "")
            {

                string filename = AMC.OpenFileDialog(selectedFileLocationTxtBx);
                if (selectedFileLocationTxtBx.Text != "")
                {
                    if (MessageBox.Show("Add selected file to AMLX-Package", "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {
                        var index = attachablesInfoDataGridView.Rows.Add();
                        attachablesInfoDataGridView.Rows[index].Cells["AMLRole"].Value = AMLfileLabel.Text;
                        attachablesInfoDataGridView.Rows[index].Cells["FileLocation"].Value = selectedFileLocationTxtBx.Text;
                        attachablesInfoDataGridView.Rows[index].Cells["FileName"].Value = filename;
                        selectedFileLocationTxtBx.Text = "";
                        AMLfileLabel.Text = "";
                        AMLURLLabel.Text = "";
                    }

                }

            }
            else
            {
                MessageBox.Show("Select AutomationML Role type from the combo box and Click Add button.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void selectURLBtn_Click(object sender, EventArgs e)
        {
          
            if (AMLURLLabel.Text != "")
            {
                if (selectedFileURLTextBox.Text != "" && selectedFileURLTextBox.Text.Contains("https://"))
                {
                    if (MessageBox.Show("Add selected file to AML-File", "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {
                        var index = attachablesInfoDataGridView.Rows.Add();
                        attachablesInfoDataGridView.Rows[index].Cells["AMLRole"].Value = AMLURLLabel.Text;
                        attachablesInfoDataGridView.Rows[index].Cells["FileLocation"].Value = selectedFileURLTextBox.Text;
                        AMLURLLabel.Text = "";
                        selectedFileURLTextBox.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Enter URL of respective AutomationML Role or Enter secured (https://) URL of a reference", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {
                MessageBox.Show("Select AutomationML Role type from the combo box and Click Add button.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void clearSelectedRowBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (attachablesInfoDataGridView.CurrentCell != null)
                {
                    int rowIndex = attachablesInfoDataGridView.CurrentCell.RowIndex;
                    attachablesInfoDataGridView.Rows.RemoveAt(rowIndex);
                }

            }
            catch (Exception) { }

        }

        private void clearAllBtn_Click(object sender, EventArgs e)
        {
            attachablesInfoDataGridView.Rows.Clear();
        }

        private void clearAllElectricalDataBtn_Click(object sender, EventArgs e)
        {
            electricalDataDataGridView.Rows.Clear();
        }

        private void clearAllPinsInfoBtn_Click(object sender, EventArgs e)
        {
            pinInfoDataGridView.Rows.Clear();
            numbOfPinsTxtBox.Text = "";
            connectorCodeCombBox.SelectedItem = "";
        }

        private void deleteElectricalInterfaceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                electricalInterfaceslistoflists.RemoveAt(electricalInterfacesComboBox.Items.IndexOf(electricalInterfacesComboBox.SelectedItem));
            }
            catch (Exception)
            {


            }

            electricalInterfacesComboBox.Items.Remove(electricalInterfacesComboBox.SelectedItem);
            MessageBox.Show("Electrical Interface is removed", "Deleted Interface", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofInterfaceClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
        Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofRoleClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
        Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofExternalInterfaceattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
        private void selectAMLFileBtn_Click(object sender, EventArgs e)
        {
            dictionaryofInterfaceClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
            dictionaryofRoleClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
            dictionaryofExternalInterfaceattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();

            treeViewAttributeClassLib.Nodes.Clear();
            treeViewRoleClassLib.Nodes.Clear();
            treeViewInterfaceClassLib.Nodes.Clear();
            CAEXDocument document = null;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "AML Files(*.aml; *.amlx;*.xml )|*.aml; *.amlx;*.xml;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string file = open.FileName;
                    FileInfo fileInfo = new FileInfo(file);
                    string objectName = fileInfo.Name;
                    string filetype = null;
                    if (( filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".amlx")
                    {
                        // Load the amlx container from the given filepath
                        AutomationMLContainer amlx = new AutomationMLContainer(file);

                        // Get the root path -> main .aml file
                        IEnumerable<PackagePart> rootParts = amlx.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);

                        // We expect the aml to only have one root part
                        if (rootParts.First() != null)
                        {
                            PackagePart part = rootParts.First();

                            // load the aml file as an CAEX document
                            document = CAEXDocument.LoadFromStream(part.GetStream());


                            // Iterate over all SystemUnitClassLibs and SystemUnitClasses and scan if it matches our format
                            // since we expect only one device per aml(x) file, return after on is found
                        }
                    }
                    if ((filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".aml" || (filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".xml")
                    {
                        document = CAEXDocument.LoadFromFile(file);
                    }

                    foreach (var classLibType in document.CAEXFile.RoleClassLib)
                     {

                        TreeNode libNode = treeViewRoleClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(),0) ;
                         foreach (var classType in classLibType.RoleClass)
                         {
                            TreeNode roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(), 1);
                            CheckForAttributes(classType);
                            if (classType.ExternalInterface.Exists)
                            {
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                   TreeNode externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(),2);
                                    CheckForAttributes(externalinterface);
                                    PrintExternalInterfaceNodes(externalinterfacenode, externalinterface);
                                }
                                
                            }
                            PrintNodesRecursiveInRoleClassLib(roleNode, classType);
                         }

                     }
                   
                    foreach (var classLibType in document.CAEXFile.InterfaceClassLib)
                    {
                        TreeNode libNode = treeViewInterfaceClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(),0);
                        foreach (var classType in classLibType.InterfaceClass) 
                        {
                            TreeNode interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(),1);
                            CheckForAttributes(classType);

                            if (classType.ExternalInterface.Exists)
                            {
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                    TreeNode externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(),2);
                                    CheckForAttributes(externalinterface);
                                    PrintExternalInterfaceNodes(externalinterfacenode, externalinterface);
                                }
                            }
                            PrintNodesRecursiveInInterfaceClassLib(interfaceclassNode, classType);
                        }       
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }
        public void PrintNodesRecursiveInRoleClassLib(TreeNode oParentNode, RoleFamilyType classType)
        {
          
            foreach (var item in classType.RoleClass)
            {

                TreeNode newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(),1);
                CheckForAttributes(item);
                if (item.ExternalInterface.Exists)
                {
                    foreach (var externalinterfaces in item.ExternalInterface)
                    {
                       TreeNode externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString(),2);
                        PrintExternalInterfaceNodes(externalinterafcenode, externalinterfaces);
                    }
                }
                PrintNodesRecursiveInRoleClassLib(newnode,item);
            }
        }
        public void PrintNodesRecursiveInInterfaceClassLib(TreeNode oParentNode, InterfaceFamilyType classType)
        {
            
            foreach (var item in classType.InterfaceClass)
            {
              TreeNode newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(),1);
                CheckForAttributes(item);
                if (item.ExternalInterface.Exists)
                {
                    foreach (var externalinterfaces in item.ExternalInterface)
                    {
                       // CheckForAttributes(externalinterfaces);
                        TreeNode externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString(),2);
                        PrintExternalInterfaceNodes(externalinterafcenode, externalinterfaces);
                    } 
                }
                PrintNodesRecursiveInInterfaceClassLib(newnode, item);
            }
        }
        public void PrintExternalInterfaceNodes(TreeNode oParentNode, ExternalInterfaceType classType)
        {
            if (classType.ExternalInterface.Exists)
            {
                foreach (var item in classType.ExternalInterface)
                {
                    TreeNode newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(), 2) ;
                    CheckForAttributes(item);
                    PrintExternalInterfaceNodes(newnode, item);
                }
            }
           
        }


        // Atrributes checker is used to retrive each attributes and store them in a dictionary with classname+parentattributename+attributename as a key for the individual 
        //list of parameters in an attribute.
        // below classes are responsible to check for attributes in Interface classes and their individual attributes.
        
        public void CheckForAttributes (InterfaceFamilyType classType)
        {
          
            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType ,attribute);
                }
            } 
        }
        public void CheckForNestedAttributeinsideAttribute(InterfaceFamilyType classType,AttributeType attributeType)
        {
            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (attributeType.Attribute.Exists)
            {
               
                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attributeinattribute);
                    StoreEachAttributeValuesInList(attributelist, attributeinattribute, classType, attributeType) ;
                }

            }
        }
         public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> listname, AttributeType AttributeInAttribute, InterfaceFamilyType classType, AttributeType attributeType)
         {
              listname = new List<ClassOfListsFromReferencefile>();
             ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

             attributeparameters.Name = attributeType.Name;
             attributeparameters.Value = attributeType.Value;
             attributeparameters.Default = attributeType.DefaultValue;
             attributeparameters.Unit = attributeType.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
             attributeparameters.Description = attributeType.Description;
             attributeparameters.CopyRight = attributeType.Copyright;
             attributeparameters.Reference = attributeType.AttributePath;

             listname.Add(attributeparameters);
            dictionaryofInterfaceClassattributes.Add(classType.Name.ToString()+attributeType.Name.ToString()+ AttributeInAttribute.Name.ToString(), listname) ;
            // Limitation, attributes with identical names in one class type cannot be added.

         }

        /// Atrributes checker is used to retrive each attributes and store them in a dictionary with classname+parentattributename+attributename as a key for the individual 
        //list of parameters in an attribute.
        // below classes are responsible to check for attributes in Role classes and their individual attributes.
        public void CheckForAttributes(RoleFamilyType classType)
        {

            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attribute);
                }
            }
        }
        public void CheckForNestedAttributeinsideAttribute(RoleFamilyType classType, AttributeType attributeType)
        {
            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attributeinattribute);
                    StoreEachAttributeValuesInList(attributelist, attributeinattribute, classType, attributeType);
                }

            }
        }
        public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> listname, AttributeType AttributeInAttribute, RoleFamilyType classType, AttributeType attributeType)
        {
            listname = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.Reference = attributeType.AttributePath;

            listname.Add(attributeparameters);
            dictionaryofRoleClassattributes.Add(classType.Name.ToString() + attributeType.Name.ToString() + AttributeInAttribute.Name.ToString(), listname);
            // Limitation, attributes with identical names in one class type cannot be added.

        }
        /// Atrributes checker is used to retrive each attributes and store them in a dictionary with classname+parentattributename+attributename as a key for the individual 
        //list of parameters in an attribute.
        // below classes are responsible to check for attributes in ExternalInterfaces and their individual attributes.
        public void CheckForAttributes(ExternalInterfaceType classType)
        {

            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attribute);
                }
            }
        }
        public void CheckForNestedAttributeinsideAttribute(ExternalInterfaceType classType, AttributeType attributeType)
        {
            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attributeinattribute);
                    StoreEachAttributeValuesInList(attributelist, attributeinattribute, classType, attributeType);
                }

            }
        }
        public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> listname, AttributeType AttributeInAttribute, ExternalInterfaceType classType, AttributeType attributeType)
        {
            listname = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.Reference = attributeType.AttributePath;

            listname.Add(attributeparameters);
            dictionaryofExternalInterfaceattributes.Add(classType.Name.ToString() + attributeType.Name.ToString() + AttributeInAttribute.Name.ToString(), listname);
            // Limitation, attributes with identical names in one class type cannot be added.

        }
        /// <summary>
        /// Drag and drop events of "AutomationML Role Treeview" and "AutomationML role treeview" in Interfaces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void tree_MouseDown(object sender, MouseEventArgs e)
        {
           
            TreeView tree = (TreeView)sender;

            TreeNode node = tree.GetNodeAt(e.X, e.Y);
            tree.SelectedNode = node;
           

            if (node != null)
            {
                tree.DoDragDrop(node, DragDropEffects.Copy);
            }
        }
        private void tree_DragOver(object sender, DragEventArgs e)
        {
            TreeView tree = (TreeView)sender;

            e.Effect = DragDropEffects.None;

            TreeNode nodeSource = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (nodeSource != null)
            {
                if (nodeSource.TreeView != tree)
                {
                    Point pt = new Point(e.X, e.Y);
                    pt = tree.PointToClient(pt);
                    TreeNode nodeTarget = tree.GetNodeAt(pt);
                    if (nodeTarget != null)
                    {
                        e.Effect = DragDropEffects.Copy;
                        tree.SelectedNode = nodeTarget;
                    }
                }
            }
        }
        private void tree_DragDrop(object sender, DragEventArgs e)
        {
           
             TreeView tree = (TreeView)sender;
             Point pt = new Point(e.X, e.Y);

             pt = tree.PointToClient(pt);

             TreeNode nodeTarget = tree.GetNodeAt(pt);

             TreeNode nodeSource = (TreeNode)e.Data.GetData(typeof(TreeNode));
             nodeTarget.Nodes.Add((TreeNode)nodeSource.Clone());
             nodeTarget.Expand();

        }
       

        private void treeViewRoleClassLib_AfterSelect(object sender, TreeViewEventArgs e)
        {
          /*  this.treeViewRoleClassLib.DragDrop += new System.Windows.Forms.DragEventHandler(this.tree_DragDrop);
            this.treeViewRoleClassLib.DragOver += new System.Windows.Forms.DragEventHandler(this.tree_DragOver);
            this.treeViewRoleClassLib.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);*/
        }





        private void treeViewInterfaceClassLib_MouseDown(object sender, MouseEventArgs e)
        {
              
                this.treeViewInterfaceClassLib.MouseDown += new MouseEventHandler(this.tree_MouseDown);
              
        }

        private void treeViewInterfaceClassLib_DragOver(object sender, DragEventArgs e)
        {
            this.treeViewInterfaceClassLib.DragOver += new DragEventHandler(this.tree_DragOver);
        }

        private void treeViewInterfaceClassLib_DragDrop(object sender, DragEventArgs e)
        {
            this.treeViewInterfaceClassLib.DragDrop += new DragEventHandler(this.tree_DragDrop);
        }





       

       

        private void buttonElectricalInterface_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(panelElectricalInterface);
        }

        private void DeleteInterfaceBtn_Click(object sender, EventArgs e)
        {
            if (treeViewImportedInterfaceHierarchy.SelectedNode.Text != "AML")
            {
                treeViewImportedInterfaceHierarchy.Nodes.Remove(treeViewImportedInterfaceHierarchy.SelectedNode);
            }
           
        }

        private void treeViewImportedInterfaceHierarchy_MouseClick(object sender, MouseEventArgs e)
        {

        }






        private void treeViewImportedInterfaceHierarchy_MouseDown(object sender, MouseEventArgs e)
        {
            this.treeViewImportedInterfaceHierarchy.MouseDown += new MouseEventHandler(this.tree_MouseDown);
        }

        private void treeViewImportedInterfaceHierarchy_DragOver(object sender, DragEventArgs e)
        {
            this.treeViewImportedInterfaceHierarchy.DragOver += new DragEventHandler(this.tree_DragOver);
        }

        private void treeViewImportedInterfaceHierarchy_DragDrop(object sender, DragEventArgs e)
        {
            this.treeViewImportedInterfaceHierarchy.DragDrop += new DragEventHandler(this.tree_DragDrop);
        }
    }
}
