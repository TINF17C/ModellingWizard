using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            deviceIDTxtBx.Text = "";
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
            productRangeTxtBx.Text = "";
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

            // Tree view updates on the "dataHiereachyTreeView"
            TreeNode node;
            TreeNode node1;
            TreeNode node2;
            node = dataHierarchyTreeView.Nodes.Add("Generic Data");
            node.Nodes.Add("Identification Data");
            node.Nodes.Add("Commercial Data");
            node.Nodes.Add("Product Data");

            node1 = dataHierarchyTreeView.Nodes.Add("Interfaces");
            node1.Nodes.Add("Electrical interface");
            node1.Nodes.Add("Sensor interface");
            node1.Nodes.Add("Mechanical interface");

            node2 = dataHierarchyTreeView.Nodes.Add("Field Attachables");
            node2.Nodes.Add("Add logos");
            node2.Nodes.Add("Add Documents");

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
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                //Initializing "Pararmeter of data table" and assigning a name.
                DataTable datatableheaderIRDIID = dataTables.Parametersdatatable();
                DataTable datatableheaderIRDIProductDetails = dataTables.Parametersdatatable();

                // Calling "IRDI Dictionary class" and retreving required Dictionary from it in this case the Dictionary is "IRDIIdentificationdata"
                //Later values in the dictionary are assigned to respective "Data Grid Views"
                DictionaryIRDI DIRDI = new DictionaryIRDI();
                Dictionary<int, Parameters> IRDIID = DIRDI.IRDIIdentificationdata();
                
                dataTables.CreateDataTableWith3Columns(IRDIID, datatableheaderIRDIID, identificationDataGridView);

            }
            // Dispaly buttons
            
            AMC.DispalySemanticBtn(vendorNameRefSemanticBtn,identificationDataGridView, "Manufacturer Name");
            AMC.DispalySemanticBtn(deviceNameRefSemanticBtn, identificationDataGridView, "Product Name");
            AMC.DispalySemanticBtn(vendorHomepageRefSemanticBtn, identificationDataGridView, "Product Online Information URL");
            AMC.DispalySemanticBtn(productFamilyRefSemanticBtn, identificationDataGridView, "Product Family");
            
        }

        // This Save button method takes multiple electrical interfaces intances and store them in a list 

        List<List<ElectricalParameters>> electricalInterfaceslistoflists = new List<List<ElectricalParameters>>();

        public void saveElectricalInterfaceBtn_Click(object sender, EventArgs e)
        {
            electricalInterfacesComboBox.Items.Add(connectorCombBox.Text);
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

            List<ElectricalParameters> listOfElectrialParameters = new List<ElectricalParameters>();

            listOfElectrialParameters.Add(eachElectricalParameter);


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
            if (!String.IsNullOrWhiteSpace(deviceIDTxtBx.Text))
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
            


            device.ipProtection = ipProtectionTxtBx.Text;
            if (!String.IsNullOrWhiteSpace(opTempMaxTxtBx.Text))
            {
                try { device.minTemperature = Convert.ToDouble(opTempMinTxtBx.Text); } catch (Exception) { device.minTemperature = Double.NaN; MessageBox.Show("Min Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            if (!String.IsNullOrWhiteSpace(opTempMaxTxtBx.Text))
            {
                try { device.maxTemperature = Convert.ToDouble(opTempMaxTxtBx.Text); } catch (Exception) { device.maxTemperature = Double.NaN; MessageBox.Show("Max Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }

            //Assigning picture paths to propertie defined in "device"
            device.deviceIcon = deviceIconTextBox.Text;
            device.devicePicture = devicePictureTextBox.Text;
            device.vendorLogo = vendorLogoTextBox.Text;

            //Assigning Documents paths to properties defined in "device"
            device.billOfMaterialsDocument = billOfMaterialsTextBox.Text;
            device.shortGuideDocument = shortGuideTextBox.Text;
            device.decOfConfDocument = decOfConTextBox.Text;

            device.dataGridParametersLists = new List<DataGridParameters>();

           if (identificationDataGridView != null)
            {

                int i = 0;
                int j = 10;
                if (i <= 0 )
                {
                    while (i<j)
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
                int j = 8;
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
                        catch (Exception ex) { MessageBox.Show(ex.Message); }

                        device.dataGridProductDetailsParametersLists.Add(parametersFromProductDetailsDataGrid);
                        i ++;

                    }
                }
            
            }
            device.dataGridProductOrderDetailsParametersLists = new List<DataGridProductOrderDetailsParameters>();
            if (dataGridViewProductOrderDetails != null)
            {

                int i = 0;
                int j = 9;
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
                        catch (Exception ex) { MessageBox.Show(ex.Message); }

                        device.dataGridProductOrderDetailsParametersLists.Add(parametersFromProductOrderDetailsDataGrid);
                        i ++;

                    }
                }
               

            }
            device.dataGridProductPriceDetailsParametersLists = new List<DataGridProductPriceDetailsParameters>();
            if (dataGridViewProductPriceDetails != null)
            {

                int i = 0;
                int j = 7;
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
                        catch (Exception ex) { MessageBox.Show(ex.Message); }

                        device.dataGridProductPriceDetailsParametersLists.Add(parametersFromProductPriceDetailsDataGrid);
                        i ++;

                    }
                }
                

            }
            device.dataGridManufacturerDetailsParametersLists = new List<DataGridManufacturerDetailsParameters>();
            if (dataGridViewManufacturerDetails != null)
            {

                int i = 0;
                int j = 7;
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
                        catch (Exception ex) { MessageBox.Show(ex.Message); }

                        device.dataGridManufacturerDetailsParametersLists.Add(parametersFromManufacturerDetailsDataGrid);
                        i ++;

                    }
                }
              

            }
            device.ElectricalInterfaceInstances = electricalInterfaceslistoflists;
            
            
            
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
            deviceIDTxtBx.Text = "";
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

            // All fields in "Field Attachables" gets cleared
            vendorLogoTextBox.Text = "";
            vendorLogoDisplayBtn.Text = "";
            vendorLogoDisplayBtn.Visible = false;
            vendorLogoPicBox.Image = null;
            deviceIconTextBox.Text = "";
            deviceIconDisplayBtn.Text = "";
            deviceIconDisplayBtn.Visible = false;
            deviceIconPicBox.Image = null;
            devicePictureTextBox.Text = "";
            devicePictureDisplayBtn.Text = "";
            devicePictureDisplayBtn.Visible = false;
            devicePicturePicBox.Image = null;

            // All "Data Grid Views" gets cleared
            dataGridViewManufacturerDetails.Rows.Clear();
            dataGridViewProductDetails.Rows.Clear();
            dataGridViewProductOrderDetails.Rows.Clear();
            dataGridViewProductPriceDetails.Rows.Clear();
            identificationDataGridView.Rows.Clear();
            semanticSystemTextBox.Text = "";

            // clear tree view
            dataHierarchyTreeView.Nodes.Clear();

        }

        private void AddLogoBtn_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(addLogoPanel,addLogoBtn);
        }

        private void VendorLogoUploadBtn_Click(object sender, EventArgs e)
        {
            AMC.OpenFileDialog(vendorLogoTextBox, vendorLogoPicBox, vendorLogoDisplayBtn );
        }

        private void DeviceIconUploadBtn_Click(object sender, EventArgs e)
        {
            AMC.OpenFileDialog(deviceIconTextBox, deviceIconPicBox, deviceIconDisplayBtn);
        }

        private void DevicePictureUploadBtn_Click(object sender, EventArgs e)
        {
            AMC.OpenFileDialog(devicePictureTextBox, devicePicturePicBox, devicePictureDisplayBtn);
        }

        private void VendorLogoClearBtn_Click(object sender, EventArgs e)
        {
            vendorLogoTextBox.Text = "";
            vendorLogoDisplayBtn.Text = "";
            vendorLogoDisplayBtn.Visible = false;
            vendorLogoPicBox.Image = null;
        }

        private void DeviceIconClearBtn_Click(object sender, EventArgs e)
        {
            deviceIconTextBox.Text = "";
            deviceIconDisplayBtn.Text = "";
            deviceIconDisplayBtn.Visible = false;
            deviceIconPicBox.Image = null;

        }

        private void DevicePictureClearBtn_Click(object sender, EventArgs e)
        {
            devicePictureTextBox.Text = "";
            devicePictureDisplayBtn.Text = "";
            devicePictureDisplayBtn.Visible = false;
            devicePicturePicBox.Image = null;
        }

        private void IdentificationDataBtn_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(identificationDataPanel,identificationDataBtn);
        }

        private void DecOfConfUploadBtn_Click(object sender, EventArgs e)
        {
            AMC.OpenFileDialog(decOfConTextBox,decOfConfDisplayBtn);
        }

        private void ShortGuideUploadBtn_Click(object sender, EventArgs e)
        {
            AMC.OpenFileDialog(shortGuideTextBox, shortGuideDisplayBtn);
        }

        private void BillOfMaterialsUploadBtn_Click(object sender, EventArgs e)
        {
            AMC.OpenFileDialog(billOfMaterialsTextBox, billofMaterialsDisplayBtn);
        }

        private void AddDocumentsBtn_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(addDocumentsPanel,addDocumentsBtn);
        }

        private void DecOfConfClearBtn_Click(object sender, EventArgs e)
        {
            decOfConfDisplayBtn.Text = "";
            decOfConTextBox.Text = "";
            decOfConfDisplayBtn.Visible = false;
        }

        private void ShortGuideClearBtn_Click(object sender, EventArgs e)
        {
            shortGuideDisplayBtn.Text = "";
            shortGuideDisplayBtn.Visible = false;
            shortGuideTextBox.Text = "";
        }

        private void BillOfMaterialsClearBtn_Click(object sender, EventArgs e)
        {
            billofMaterialsDisplayBtn.Text = "";
            billofMaterialsDisplayBtn.Visible = false;
            billOfMaterialsTextBox.Text = "";
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
            if (dataHierarchyTreeView.SelectedNode.Text == "Add logos")
            {
                dataTabControl.SelectTab("fieldAttachablesTabPage");
                AMC.WindowSizeChanger(addLogoPanel);
            }
            if (dataHierarchyTreeView.SelectedNode.Text == "Add Documents")
            {
                dataTabControl.SelectTab("fieldAttachablesTabPage");
                AMC.WindowSizeChanger(addDocumentsPanel);
            }
            if (dataHierarchyTreeView.SelectedNode.Text == "Electrical interface")
            {
                dataTabControl.SelectTab("Interfaces");
                AMC.WindowSizeChanger(electricalInterfacePanel);
            }
            dataHierarchyTreeView.SelectedNode = null;
        }

        

        private void electricalInterfaceBtn_Click_1(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(electricalInterfacePanel, electricalInterfaceBtn);
        }

        private void addElectricalInterfacesBtn_Click(object sender, EventArgs e)
        {
           /* int numbOfConnectors = Convert.ToInt32(electricalInterfacenumbTxtBox.Text);

            for (int i = 0; i < numbOfConnectors; i++)
            {
                electricalInterfacesComboBox.Items.Add("Electrical Connector");
            }*/
           
        }
        
        
       
        private void addElectricalDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectorCombBox.SelectedItem != null && connectorTypeCombBox.SelectedItem != null)
                {
                    Datatables datatables = new Datatables();

                    DataTable datatableheadersIRDIED = datatables.Parametersdatatable();

                    DictionaryIRDI DIRDI = new DictionaryIRDI();

                    Dictionary<int, Parameters> IRDIED = DIRDI.IRDIElectricalData();

                    datatables.CreateDataTableWith4Columns(IRDIED, datatableheadersIRDIED, electricalDataDataGridView);
                    

                }

            }
            catch (Exception)
            {

                MessageBox.Show(" 'Select Connector from Connector Combo Box' or 'Select Connector Type from Connector Type combo box' ", "ERROR" , MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
           
           

        }
        private void clearElectricalDataBtn_Click(object sender, EventArgs e)
        {
            electricalDataDataGridView.Rows.Clear();
        }

        private void addPinsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (numbOfPinsTxtBox.Text != null && connectorCodeCombBox.SelectedItem != null)
                {

                    int countofpins = 0;
                    string enteredvalue = numbOfPinsTxtBox.Text;
                    int convertedtonumber = Convert.ToInt32(enteredvalue);
                    for (int i = 0; i < convertedtonumber; i++)
                    {
                        pinInfoDataGridView.Rows.Add();
                        pinInfoDataGridView.Rows[countofpins + i].Cells[0].Value = (1 + i).ToString();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Missing Number of Pins or Connector Code", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearPinsBtn_Click(object sender, EventArgs e)
        {
            pinInfoDataGridView.Rows.Clear();
            numbOfPinsTxtBox.Text = "";
            connectorCodeCombBox.SelectedItem = "";
        }

       
      
       
    }
}
