using Aml.Engine.CAEX;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aml.Editor.Plugin
{
    /// <summary>
    /// This is a window forms UI control, containing a tree view. The Tree view is updated, ever when an InternalElement is selected
    /// in the editor which has an Instance Class relation to a SystemUnitClass. The Tree view is populated with the ExternalInterface
    /// objects and InternalElement objects of the referenced SystemUnitClass.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class StartGUI : UserControl
    {
        private MWController mWController;
        private MWData.MWFileType filetype;
        #region Public Constructors

        public StartGUI(MWController mWController)
        {
            this.mWController = mWController;
            InitializeComponent();
        }

        #endregion Public Constructors 

        private void createDeviceBtn_Click(object sender, EventArgs e)
        {
            mWController.ChangeGui(MWController.MWGUIType.CreateDevice);
        }

        private void createInterfaceBtn_Click(object sender, EventArgs e)
        {
            mWController.ChangeGui(MWController.MWGUIType.CreateInterface);
        }

        private void importIODDFileBtn_Click(object sender, EventArgs e)
        {
            filetype = MWData.MWFileType.IODD;
            openFileDialog.Filter = "IODD Files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.ShowDialog();
        }

        private void importGSDFileBtn_Click(object sender, EventArgs e)
        {
            filetype = MWData.MWFileType.GSD;
            openFileDialog.Filter = "GSDML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            foreach (String filename in openFileDialog.FileNames)
            {
                String error = mWController.importFile(filename, filetype);
                if (error != null)
                {
                    MessageBox.Show(error);
                }

            }
            
        }

        internal void updateDeviceDropdown(List<MWData.MWObject> devices)
        {
            devicesComboBox.Items.Clear();
            if (devices.Count > 0)
            {
                foreach (MWData.MWObject device in devices)
                {
                    
                    string name = "";
                    if (device is MWDevice)
                    {
                       name = ((MWDevice) device).deviceName;
                    }
                    else
                    {
                        name = ((MWInterface)device).numberOfInterface.ToString();
                    }
                    devicesComboBox.Items.Add(name);
                }
            }
            else
            {
                devicesComboBox.Items.Add("<No created Objects>");
            }
        }

        private void devicesComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mWController.showDevice(devicesComboBox.SelectedIndex);
        }

        private void openEditDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mWController.showDevice(openEditDialog.FileName);
        }

        private void editFileBtn_Click(object sender, EventArgs e)
        {
            openEditDialog.ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
           
        }

        private void DeviceDescriptionBtn_Click(object sender, EventArgs e)
        {
            mWController.ChangeGui(MWController.MWGUIType.DeviceDescription);
        }
    }
}