using System;
using System.Windows.Forms;

namespace Aml.Editor.Plugin
{
    public partial class CreateDevice
        : UserControl
    {
        // Controller class to pass the data to
        MWController mWController;

        // flag if we are creating a new device or editing one
        bool isEditing = false;

        // holders for the selected paths. These will be set by the FileOKs of the openFileDialogs
        string vendorPicturePath = "";
        string devicePicturePath = "";
        string deviceIconPath = "";

        #region Public Constructors
        /// <summary>
        /// Create and initialize the GUI
        /// </summary>
        /// <param name="mWController">The <see cref="MWController"/>MWController Object to which the data will be passed to</param>
        public CreateDevice(MWController mWController)
        {
            this.mWController = mWController;
            InitializeComponent();

        }

        #endregion Public Constructors

        /// <summary>
        /// OnClick Methode of the Create Device Button.
        /// This creates a new <see cref="MWDevice"/>Device which will be passed to the <see cref="mWController"/>
        /// </summary>
        /// <param name="sender">Event Argument, not used</param>
        /// <param name="e">Event Argument, not used</param>
        private void createDeviceBtn_Click(object sender, System.EventArgs e)
        {

            // Create a new Device
            var device = new MWDevice();

            // Read all the input fields and write them to the device data
            if (deviceTypeListBox.SelectedItem != null)
            {
                device.deviceType = deviceTypeListBox.SelectedItem.ToString();
            }
            else
            {
                device.deviceType = "";
            }

            // Check if there was an input in this field, if so: try to convert it to integer
            if (!String.IsNullOrWhiteSpace(txtVendorId.Text))
            {
                try { device.vendorID = Convert.ToInt32(txtVendorId.Text); } catch (Exception) { MessageBox.Show("Warning: Vendor ID must be number.\n please correct input"); }
            }
            // Check if there was an input in this field, if so: try to convert it to integer
            if (!String.IsNullOrWhiteSpace(txtDeviceId.Text))
            {
                try { device.deviceID = Convert.ToInt32(txtDeviceId.Text); } catch (Exception) { MessageBox.Show("Device ID is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            device.vendorName = txtVendorName.Text;
            device.vendorHomepage = txtVendorHomepage.Text;
            device.deviceName = txtDeviceName.Text;
            device.deviceFamily = txtDeviceFamily.Text;
            device.productName = txtProductName.Text;
            device.orderNumber = txtOrderNumber.Text;
            device.productText = productTxtBox.Text;
            device.harwareRelease = hwRelTxt.Text;
            device.softwareRelease = swRelTxt.Text;


            device.ipProtection = txtIpProduction.Text;
            if (!String.IsNullOrWhiteSpace(txtMaxTemp.Text))
            {
                try { device.minTemperature = Convert.ToDouble(txtMinTemp.Text); } catch (Exception) { device.minTemperature = Double.NaN; MessageBox.Show("Min Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            if (!String.IsNullOrWhiteSpace(txtMaxTemp.Text))
            {
                try { device.maxTemperature = Convert.ToDouble(txtMaxTemp.Text); } catch (Exception) { device.maxTemperature = Double.NaN; MessageBox.Show("Max Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }

            device.deviceIcon = deviceIconPath;
            device.devicePicture = devicePicturePath;
            device.vendorLogo = vendorPicturePath;

            // Pass the device to the controller
            string result = mWController.CreateDeviceOnClick(device, isEditing);
            clear();
            // Display the result
            if (result != null)
            {
                // Display error Dialog
                MessageBox.Show(result);
            }
        }

        /// <summary>
        /// OnClick of the Back Button
        /// Call the <see cref="mWController"/> to display the Start GUI
        /// If <see cref="isEditing"/> was set, clear all input fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backBtn_Click(object sender, EventArgs e)
        {
            mWController.ChangeGui(MWController.MWGUIType.Start);
            if (isEditing)
            {
                clear();
            }
        }

        /// <summary>
        /// Clears all input fields and reset the GUI
        /// </summary>
        internal void clear()
        {
            deviceTypeListBox.SelectedItem = -1;
            txtVendorId.Text = "";
            txtVendorName.Text = "";
            txtVendorHomepage.Text = "";
            txtDeviceId.Text = "";
            txtDeviceName.Text = "";

            txtDeviceFamily.Text = "";
            txtProductName.Text = "";
            txtOrderNumber.Text = "";
            productTxtBox.Clear();
            txtIpProduction.Text = "";
            hwRelTxt.Text = "";
            swRelTxt.Text = "";
            txtMinTemp.Text = "";
            txtMaxTemp.Text = "";

            createDeviceBtn.Text = "Create Device";
            txtDeviceName.Enabled = true;
            txtVendorName.Enabled = true;
            isEditing = false;
        }

        /// <summary>
        /// Fill the input fields with data from the device
        /// </summary>
        /// <param name="device">The device with the data</param>
        internal void prefill(MWDevice device)
        {
            deviceTypeListBox.SelectedItem = device.deviceType;
            txtVendorId.Text = device.vendorID.ToString();
            txtVendorName.Text = device.vendorName;
            txtVendorHomepage.Text = device.vendorHomepage;
            txtDeviceId.Text = device.deviceID.ToString();
            txtDeviceName.Text = device.deviceName;

            txtDeviceFamily.Text = device.deviceFamily;
            txtProductName.Text = device.productName;
            txtOrderNumber.Text = device.orderNumber;
            productTxtBox.Text = device.productText;
            txtIpProduction.Text = device.ipProtection;
            hwRelTxt.Text = device.harwareRelease;
            swRelTxt.Text = device.softwareRelease;

            if (!Double.IsNaN(device.minTemperature))
            {
                txtMinTemp.Text = device.minTemperature.ToString();
            }
            else
            {
                txtMinTemp.Text = "";
            }

            if (!Double.IsNaN(device.maxTemperature))
            {
                txtMaxTemp.Text = device.maxTemperature.ToString();
            }
            else
            {
                txtMaxTemp.Text = "";
            }

            createDeviceBtn.Text = "Update Device";
            txtDeviceName.Enabled = false;
            txtVendorName.Enabled = false;

            vendorPicturePath = device.vendorLogo;
            deviceIconPath = device.deviceIcon;
            devicePicturePath = device.devicePicture;

            isEditing = true;
        }

        /// <summary>
        /// Open the VendorLogo File Dialog
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openVendorLogoBtn_Click(object sender, EventArgs e)
        {
            openVendorLogoDialog.ShowDialog();
        }

        /// <summary>
        /// Open DeviceIcon File Dialog
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openDeviceIconBtn_Click(object sender, EventArgs e)
        {
            openDeviceIconDialog.ShowDialog();
        }

        /// <summary>
        /// Open the openDevicePicture File Dialog
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openDevicePicture_Click(object sender, EventArgs e)
        {
            openDevicePictureDialog.ShowDialog();
        }

        /// <summary>
        /// update the <see cref="vendorPicturePath"/> with the selected file
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openVendorLogoDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vendorPicturePath = openVendorLogoDialog.FileName;
        }

        /// <summary>
        /// update the <see cref="deviceIconPath"/> with the selected file
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openDeviceIconDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            deviceIconPath = openDeviceIconDialog.FileName;
        }

        /// <summary>
        /// update the <see cref="devicePicturePath"/> with the selected file
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openDevicePictureDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            devicePicturePath = openDevicePictureDialog.FileName;
        }

    }
}