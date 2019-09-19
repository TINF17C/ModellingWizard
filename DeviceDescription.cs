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
        }

        private void AddSemanticSystemBtn_Click(object sender, EventArgs e)
        {
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
            
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                
                DataTable datatableheaderIRDIID = dataTables.Parametersdatatable();

                DictionaryIRDI DIRDI = new DictionaryIRDI();
                Dictionary<int, Parameters> IRDIID = DIRDI.IRDIIdentificationdata();

                dataTables.CreateDataTableWith3Columns(IRDIID, datatableheaderIRDIID, identificationDataGridView);
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
               
                int i = 8;
                while (i >= 0)
                {
                    DataGridParameters parametersFromDataGrid = new DataGridParameters();
                    try
                    {
                        parametersFromDataGrid.RefSemantic = Convert.ToString(identificationDataGridView.Rows[i].Cells[0].Value);
                        parametersFromDataGrid.Attributes = Convert.ToString(identificationDataGridView.Rows[i].Cells[1].Value);
                        parametersFromDataGrid._value = Convert.ToString(identificationDataGridView.Rows[i].Cells[2].Value);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                    device.dataGridParametersLists.Add(parametersFromDataGrid);
                    i += -1;

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

        }

        private void AddLogoBtn_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(addLogopanel,addLogoBtn);
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
    }
}
