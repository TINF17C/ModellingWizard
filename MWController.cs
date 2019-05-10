using System;
using System.Collections.Generic;

namespace Aml.Editor.Plugin
{
    /// <summary>
    /// This class passes the inputs of the GUIs to <see cref="MWData"/> where needed and it is in controll of what is displayed at the screen
    /// </summary>
    public class MWController
    {
        // the (initialised) GUIs
        private CreateDevice createDeviceForm;
        private CreateInterface createInterfaceForm;
        private StartGUI startGUI;

        // the interface class to the AML Editor
        private ModellingWizard modellingWizard;

        // the MWData instance of the the plugin which handles all the AMLX stuff
        private MWData mWData;

        // the list of the currently loaded amlx devices / interfaces
        // these will be displayed on the start GUI
        private List<MWData.MWObject> devices = new List<MWData.MWObject>();

        /// <summary>
        /// Init the controller and reload all amlx devices
        /// </summary>
        /// <param name="modellingWizard"></param>
        public MWController(ModellingWizard modellingWizard)
        {
            this.modellingWizard = modellingWizard;
            mWData = new MWData(this);
            ReloadObjects();
        }

        /// <summary>
        /// Create the new CreateDevice GUI or return the previously created GUI
        /// </summary>
        /// <returns>the CreateDevice GUI for this session</returns>
        public CreateDevice GetCreateDeviceForm()
        {
            if (createDeviceForm == null)
            {
                createDeviceForm = new CreateDevice(this);
            }

            return createDeviceForm;
        }

        /// <summary>
        /// Create the new CreateInterface GUI or return the previously created GUI
        /// </summary>
        /// <returns>the CreateDevice GUI for this session</returns>
        public CreateInterface GetCreateInterfaceForm()
        {
            if (createInterfaceForm == null)
            {
                createInterfaceForm = new CreateInterface(this);
            }

            return createInterfaceForm;
        }

        /// <summary>
        /// Create the new Start GUI or return the previously created GUI
        /// </summary>
        /// <returns>the CreateDevice GUI for this session</returns>
        public StartGUI GetStartGUI()
        {
            if (startGUI == null)
            {
                startGUI = new StartGUI(this);
            }

            return startGUI;
        }


        // OnClickFunktion für CreateDevice
        // OnClickFunktion für CreateInterfac
        //      Daten aus allen GUI Input Feldern auslesen
        //      CreateDevice(/Interface) in MWModell mit diesen Daten als Parameter aufrufen#

        /// <summary>
        /// Pass the data of the newInterface to the MWData
        /// </summary>
        /// <param name="newInterface">the object holding the data</param>
        /// <param name="isEdit">true if the device was edited, false if the device is created</param>
        /// <returns>the result as a string</returns>
        public string CreateInterfaceOnClick(MWInterface newInterface, bool isEdit)
        {
            // create the interface
            string result = mWData.CreateInterface(newInterface, isEdit);

            // update the device list
            if (isEdit)
            {
                ReloadObjects();
            }
            else
            {
                devices.Add(newInterface);
                GetStartGUI().updateDeviceDropdown(devices);
            }

            // go to Start GUI
            ChangeGui(MWController.MWGUIType.Start);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDevice"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public String CreateDeviceOnClick(MWDevice newDevice, bool isEdit)
        {
            // Check if device Name is set
            if (newDevice.deviceName.Equals(""))
            {
                return "Error no device name set!";
            }

            // Check if vendor name is set
            if (newDevice.vendorName.Equals(""))
            {
                return "Error no vendor name set!";
            }

            // create the device
            string result = mWData.CreateDevice(newDevice, isEdit);


            // update the device list
            if (isEdit)
            {
                ReloadObjects();
            }
            else
            {
                devices.Add(newDevice);
                GetStartGUI().updateDeviceDropdown(devices);
            }

            ChangeGui(MWController.MWGUIType.Start);
            return result;

        }

        /// <summary>
        /// Show the correct GUI for the selected device
        /// </summary>
        /// <param name="selectedIndex">The index of the selected item in the dropdown</param>
        internal void showDevice(int selectedIndex)
        {
            if (devices.Count >= 1)
            {
                MWData.MWObject mWObject = devices[selectedIndex];
                // Display the corect GUI for the object type
                if (mWObject is MWDevice)
                {
                    GetCreateDeviceForm().prefill((MWDevice)mWObject);
                    ChangeGui(MWGUIType.CreateDevice);
                }
                else if (mWObject is MWInterface)
                {
                    GetCreateInterfaceForm().prefill((MWInterface)mWObject);
                    ChangeGui(MWGUIType.CreateInterface);
                }
            }
        }

        /// <summary>
        /// Load the AMLX file and display the loaded device
        /// </summary>
        /// <param name="fileName">the full path to an .amlx file</param>
        internal void showDevice(string fileName)
        {
            MWData.MWObject mWObject = mWData.loadObject(fileName);
            if (mWObject == null)
            {
                System.Windows.Forms.MessageBox.Show("The loaded device does not match the required format.\nThe ModellingWizard can not display this object");
                return;
            }
            devices.Add(mWObject);
            GetStartGUI().updateDeviceDropdown(devices);

            // show the most recently added device
            showDevice(devices.Count - 1);
        }

        /// <summary>
        /// Reload all .amlx files in ./modellingwizard/ and update the dropdown.
        /// </summary>
        internal void ReloadObjects()
        {
            devices = mWData.LoadMWObjects();
            GetStartGUI().updateDeviceDropdown(devices);
        }

        /// <summary>
        /// Switch the displayed 
        /// </summary>
        /// <param name="targetGUI">the GUI Type to display</param>
        public void ChangeGui(MWGUIType targetGUI)
        {
            switch (targetGUI)
            {
                case MWGUIType.CreateDevice:
                    modellingWizard.changeGUI(GetCreateDeviceForm());
                    break;
                case MWGUIType.CreateInterface:
                    modellingWizard.changeGUI(GetCreateInterfaceForm());
                    break;
                case MWGUIType.Start:
                    modellingWizard.changeGUI(GetStartGUI());
                    break;
            }
        }

        /// <summary>
        /// Enum to represent the GUI
        /// </summary>
        public enum MWGUIType { CreateDevice, CreateInterface, Start }

        /// <summary>
        /// Call the Converter with the given file
        /// </summary>
        /// <param name="filename">the full path to the file</param>
        /// <param name="filetype">whether the file is an IODD or an GSD file</param>
        /// <returns></returns>
        public string importFile(string filename, MWData.MWFileType filetype)
        {

            // call the correct import function for the file type
            string result = null;
            switch (filetype)
            {
                case MWData.MWFileType.IODD:
                    result = mWData.ImportIODD2AML(filename);
                    break;
                case MWData.MWFileType.GSD:
                    result = mWData.ImportGSD2AML(filename);
                    break;
                default:
                    result = "Invalid Filetype";
                    break;
            }
            ReloadObjects();
            return result;
        }
    }
}
