using Aml.Engine.AmlObjects;
using Aml.Engine.CAEX;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Aml.Editor.Plugin
{
    public class MWData : DeviceDescription
    {
        // holds the controller to report created devices to
        private readonly MWController mWController;
       

        /// <summary>
        /// Create the MWData Object
        /// </summary>
        /// <param name="mWController">the MWController to report to</param>
        public MWData(MWController mWController)
        {
            this.mWController = mWController;
        }

        /// <summary>
        /// Iterate over all .amlx Files in .\modellingwizard\ and try to load them as a device
        /// </summary>
        /// <returns>all loaded devices and interfaces</returns>
        public List<MWObject> LoadMWObjects()
        {
            List<MWObject> objects = new List<MWObject>();

            string amlFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\modellingwizard\\";
            // create Directory if it's not existing
            if (!Directory.Exists(amlFilePath))
            {
                Directory.CreateDirectory(amlFilePath);
            }

            // Get all .amlx Files in the directory
            string[] files = Directory.GetFiles(amlFilePath, "*.amlx");

            foreach (string file in files)
            {
                // try to load the object
                MWObject mWObject = loadObject(file);
                if (mWObject != null)
                {
                    objects.Add(mWObject);
                }
            }
            return objects;
        }

        /// <summary>
        /// Load the amlx container and try to load it as an <see cref="MWObject"/>
        /// </summary>
        /// <param name="file">The full path to the amlx file</param>
        /// <returns></returns>
        public MWObject loadObject(string file)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(file);
                string objectName = fileInfo.Name;

                // Load the amlx container from the given filepath
                AutomationMLContainer amlx = new AutomationMLContainer(file);

                // Get the root path -> main .aml file
                IEnumerable<PackagePart> rootParts = amlx.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);

                // We expect the aml to only have one root part
                if (rootParts.First() != null)
                {
                    PackagePart part = rootParts.First();

                    // load the aml file as an CAEX document
                    CAEXDocument document = CAEXDocument.LoadFromStream(part.GetStream());


                    // Iterate over all SystemUnitClassLibs and SystemUnitClasses and scan if it matches our format
                    // since we expect only one device per aml(x) file, return after on is found
                    foreach (SystemUnitClassLibType classLibType in document.CAEXFile.SystemUnitClassLib)
                    {
                        foreach (SystemUnitFamilyType classLib in classLibType.SystemUnitClass)
                        {
                            // check if it matches our format
                            foreach (InternalElementType internalElement in classLib.InternalElement)
                            {
                                // is the DeviceIdentification there?
                                if (internalElement.Name.Equals("DeviceIdentification"))
                                {
                                    // is it an interface or a device?
                                    if (internalElement.Attribute.GetCAEXAttribute("InterfaceNumber") != null)
                                    {

                                      

                                        amlx.Close();
                                       
                                    }
                                    else if (internalElement.Attribute.GetCAEXAttribute("DeviceName") != null)
                                    {
                                        MWDevice mWDevice = new MWDevice();

                                        // read the attributes and write them directly into the device
                                        fillDeviceWithData(mWDevice, internalElement.Attribute);

                                        // check if there are pictures provided
                                        foreach (InternalElementType ie in classLib.InternalElement)
                                        {
                                            switch (ie.Name)
                                            {
                                                case "ManufacturerIcon":
                                                    try
                                                    {
                                                        mWDevice.vendorLogo = ie.ExternalInterface.First().Attribute.GetCAEXAttribute("refURI").Value;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        // No vendorLogo
                                                    }
                                                    break;
                                                case "ComponentPicture":
                                                    try
                                                    {
                                                        mWDevice.devicePicture = ie.ExternalInterface.First().Attribute.GetCAEXAttribute("refURI").Value;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        // No vendorLogo
                                                    }
                                                    break;
                                                case "ComponentIcon":
                                                    try
                                                    {
                                                        mWDevice.deviceIcon = ie.ExternalInterface.First().Attribute.GetCAEXAttribute("refURI").Value;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        // No vendorLogo
                                                    }
                                                    break;
                                            }
                                        }
                                        amlx.Close();
                                        return mWDevice;
                                    }
                                }
                            }
                        }

                    }

                }
                amlx.Close();
                return null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error while loading the AMLX-File", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Read the all attributes in <paramref name="attributes"/> and write the values into <paramref name="mWInterface"/>
        /// </summary>
        /// <param name="mWInterface">the object to write the data into</param>
        /// <param name="attributes">the list of attributes</param>
       

        /// <summary>
        /// Read the all attributes in <paramref name="attributes"/> and write the values into <paramref name="device"/>
        /// </summary>
        /// <param name="device">the object to write the data into</param>
        /// <param name="attributes">the list of attributes</param>
        private void fillDeviceWithData(MWDevice device, AttributeSequence attributes)
        {
            // iterate over all atttributes
            foreach (AttributeType attribute in attributes)
            {
                // apply the value of the attribute to the correct interface parameter
                switch (attribute.Name)
                {
                    case "CommunicationTechonolgy":
                        device.deviceType = attribute.Value;
                        break;
                    case "VendorId":
                        try
                        {
                            device.vendorID = Int32.Parse(attribute.Value);
                        }
                        catch (Exception)
                        {
                            // let the value be null
                        }
                        break;
                    case "VendorName":
                        device.vendorName = attribute.Value;
                        break;
                    case "DeviceId":
                        try
                        {
                            device.deviceID = Int32.Parse(attribute.Value);
                        }
                        catch (Exception)
                        {
                            // let the value be null
                        }
                        break;
                    case "DeviceName":
                        device.deviceName = attribute.Value;
                        break;
                    case "ProductRange":
                        device.productRange = attribute.Value;
                        break;
                    case "ProductName":
                        device.productNumber = attribute.Value;
                        break;
                    case "OrderNumber":
                        device.orderNumber = attribute.Value;
                        break;
                    case "ProductText":
                        device.productText = attribute.Value;
                        break;
                    case "IPProtection":
                        device.ipProtection = attribute.Value;
                        break;
                    case "OperatingTemperatureMin":
                        try
                        {
                            device.minTemperature = Double.Parse(attribute.Value);
                        }
                        catch (Exception)
                        {
                            device.minTemperature = Double.NaN;
                        }
                        break;
                    case "OperatingTemperatureMax":
                        try
                        {
                            device.maxTemperature = Double.Parse(attribute.Value);
                        }
                        catch (Exception)
                        {
                            device.maxTemperature = Double.NaN;
                        }
                        break;
                    case "VendorUrl":
                        device.vendorHomepage = attribute.Value;
                        break;
                    case "HardwareRelease":
                        device.harwareRelease = attribute.Value;
                        break;
                    case "SoftwareRelease":
                        device.softwareRelease = attribute.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Create the AMLX File with the correct AML File and optional pictures
        /// </summary>
        /// <param name="device">The device which will be created</param>
        /// <param name="isEdit">true if an amlx file get update, false if a new file will be created</param>
        /// <returns></returns>
        public string CreateDevice(MWDevice device, bool isEdit)
        {
           
            CAEXDocument document = null;
            AutomationMLContainer amlx = null;

            // Init final .amlx Filepath
            //first of all create a folder on "Vendor Name"
            string vendorCompanyName = device.vendorName;
            string vendorCompanyNameFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\modellingwizard\\" + vendorCompanyName;
            
            if (!Directory.Exists(vendorCompanyNameFilePath))
            {
                Directory.CreateDirectory(vendorCompanyNameFilePath);
            }

            // Cretae a folder inside "Vendor Name"  file defining "Product Range"

            string deviceFamilyNamePath = System.IO.Path.Combine(vendorCompanyNameFilePath, device.productRange);
            System.IO.Directory.CreateDirectory(deviceFamilyNamePath);

            // Create a folder inside the "Product Range" file defining "Product Group"
           
            string productGroupNamePath = System.IO.Path.Combine(deviceFamilyNamePath, device.productGroup);
            System.IO.Directory.CreateDirectory(productGroupNamePath);

            // Create a folder inside the "Product Group" file defining "Product Families"
           
            string productFamilyNamePath = System.IO.Path.Combine(productGroupNamePath, device.productFamily);
            System.IO.Directory.CreateDirectory(productFamilyNamePath);

            // Create a folder inside the "Product Families" file for individual products
            string productNamePath = System.IO.Path.Combine(productFamilyNamePath, device.deviceName);
            System.IO.Directory.CreateDirectory(productNamePath);

          
            string fileName = device.vendorName + "-" + device.deviceName + "-V.1.0-" + DateTime.Now.Date.ToShortDateString();
          
            string amlFilePath = System.IO.Path.Combine(productNamePath, fileName + ".amlx");

            
            FileInfo file = new FileInfo(amlFilePath);
           
           

            // Create directory if it's not existing
            file.Directory.Create();
           

            // Init CAEX Document
            if (isEdit)
            {
                // Load the amlx file
                amlx = new AutomationMLContainer(amlFilePath, FileMode.Open);

                IEnumerable<PackagePart> rootParts = amlx.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);

                // We expect the aml to only have one root part
                if (rootParts.First() != null)
                {
                    PackagePart part = rootParts.First();

                    // load the aml file as an CAEX document
                    document = CAEXDocument.LoadFromStream(part.GetStream());
                }
                else
                {
                    // the amlx contains no aml file
                    document = CAEXDocument.New_CAEXDocument();
                }
            }
            else
            {
                // create a new CAEX document
                document = CAEXDocument.New_CAEXDocument();
                try
                {amlx = new AutomationMLContainer(amlFilePath, FileMode.Create); } catch (Exception){}
                 

            }

           

            // Init the default Libs
            AutomationMLBaseRoleClassLibType.RoleClassLib(document);
            AutomationMLInterfaceClassLibType.InterfaceClassLib(document);

            SystemUnitFamilyType systemUnitClass = null;
            // Create the SystemUnitClass for our device
           if (!isEdit)
            {
                systemUnitClass = document.CAEXFile.SystemUnitClassLib.Append("ComponentSystemUnitClassLib").SystemUnitClass.Append(device.deviceName);


                device.listWithURIConvertedToString = new List<AttachablesDataGridViewParameters>();
                foreach (AttachablesDataGridViewParameters eachparameter in device.dataGridAttachablesParametrsList)
                {
                    if (eachparameter.FilePath.Contains("https://") || eachparameter.FilePath.Contains("http://") || eachparameter.FilePath.Contains("www") || eachparameter.FilePath.Contains("WWW"))
                    {
                        interneturl(eachparameter.FilePath, eachparameter.ElementName.ToString(), "ExternalDataConnector", systemUnitClass);
                    }
                    else
                    {
                       
                        Boolean myBool;
                        Boolean.TryParse(eachparameter.AddToFile, out myBool);
                        
                        if (myBool == true)
                        {
                          
                        }

                        Uri eachUri = null;
                        AttachablesDataGridViewParameters par = new AttachablesDataGridViewParameters();
                        eachUri = createPictureRef(eachparameter.FilePath, eachparameter.ElementName.ToString(), "ExternalDataConnector", systemUnitClass);
                        par.ElementName = eachUri.ToString();
                        par.FilePath = eachparameter.FilePath;

                        device.listWithURIConvertedToString.Add(par);

                    }
                   
                }

            }
           else
            {
                // check if our format is given in the amlx file if not: create it
                bool foundSysClassLib = false;
                foreach (var sysclasslib in document.CAEXFile.SystemUnitClassLib)
                {
                    if (sysclasslib.Name.Equals("ComponentSystemUnitClassLib"))
                    {
                        bool foundSysClass = false;
                        foreach (var sysclass in sysclasslib.SystemUnitClass)
                        {
                            if (sysclass.Name.Equals(device.deviceName))
                            {
                                foundSysClass = true;
                                systemUnitClass = sysclass;
                                break;
                            }
                        }
                        if (!foundSysClass)
                            systemUnitClass = sysclasslib.SystemUnitClass.Append(device.deviceName);
                        foundSysClassLib = true;
                    }
                }
                if (!foundSysClassLib)
                    systemUnitClass = document.CAEXFile.SystemUnitClassLib.Append("ComponentSystemUnitClassLib").SystemUnitClass.Append(device.deviceName);
            }


           

            InternalElementType ie = null;
            foreach (var internalelement in systemUnitClass.InternalElement)
            {
                if (internalelement.Name.Equals("DeviceIdentification"))
                {
                    ie = internalelement;
                    break;
                }
               
            }
            if (ie == null)
                ie = systemUnitClass.InternalElement.Append("DeviceIdentification");

            // Init the Attributes for our format and set the correct DataTypes
            initCAEXattributes(ie);

            // Set the correct values for the Attributes
            setCAEXattribute(ie, device);

            // Create the internalElement Electrical Interfaces

            if (device.vendorName != null)
            {
                InternalElementType electricalInterface = null;
                foreach (var internalElement in systemUnitClass.InternalElement)
                {
                    if (internalElement.Name.Equals("ElectricalInterfaces"))
                    {
                        electricalInterface = internalElement;
                        break;
                    }
                }
                if (electricalInterface == null)
                    electricalInterface = systemUnitClass.InternalElement.Append("ElectricalInterfaces");

                foreach (var pair in device.DictionaryForInterfaceClassesInElectricalInterfaces)
                {
                    ExternalInterfaceType electricalConnectorType = null;

                    ExternalInterfaceType electricalConnectorPins = null;

                    Match numberfromElectricalConnectorType = Regex.Match(pair.Key.ToString(), @"\((\d+)\)");
                    string initialnumberbetweenparanthesisofElectricalConnectorType = numberfromElectricalConnectorType.Groups[1].Value;


                    string electricalConnectorTypeName = Regex.Replace(pair.Key.ToString(), @"\(.+?\)", "");
                    electricalConnectorTypeName = Regex.Replace(electricalConnectorTypeName, @"\{.+?\}", "");

                    electricalConnectorType = electricalInterface.ExternalInterface.Append(electricalConnectorTypeName);

                    var attributesOfConnectorType = electricalConnectorType.Attribute;

                    foreach (var valueList in pair.Value)
                    {
                        foreach (var item in valueList)
                        {
                            var eachattribute = attributesOfConnectorType.Append(item.Name.ToString());
                            eachattribute.Value = item.Value;
                            eachattribute.DefaultValue = item.Default;
                            eachattribute.Unit = item.Unit;
                            //eachattribute.AttributeDataType = 
                            eachattribute.Description = item.Description;
                            eachattribute.Copyright = item.CopyRight;
                            
                            eachattribute.ID = item.ID;



                            electricalConnectorType.RefBaseClassPath = item.RefBaseClassPath;
                        }
                    }


                    foreach (var pairofList in device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces)
                    {
                        Match numberfromElectricalConnectorPins = Regex.Match(pairofList.Key.ToString(), @"\((\d+)\)");
                        string initialnumberbetweenparanthesisElectricalConnectorPins = numberfromElectricalConnectorPins.Groups[1].Value;

                        string electricalConnectorPinName = Regex.Replace(pairofList.Key.ToString(), @"[^a-zA-Z]", "");
                        electricalConnectorPinName = Regex.Replace(electricalConnectorPinName, @"\{.+?\}", "");
                        electricalConnectorPinName = electricalConnectorPinName.Replace(electricalConnectorTypeName,"");

                        


                        if (initialnumberbetweenparanthesisofElectricalConnectorType == initialnumberbetweenparanthesisElectricalConnectorPins)
                        {
                            electricalConnectorPins = electricalConnectorType.ExternalInterface.Append(electricalConnectorPinName);

                            var attributesOfConnectorPins = electricalConnectorPins.Attribute;

                            foreach (var valueList in pairofList.Value)
                            {
                                foreach (var item in valueList)
                                {
                                    var eachattribute = attributesOfConnectorPins.Append(item.Name.ToString());
                                    eachattribute.Value = item.Value;
                                    eachattribute.DefaultValue = item.Default;
                                    eachattribute.Unit = item.Unit;
                                    //eachattribute.AttributeDataType = 
                                    eachattribute.Description = item.Description;
                                    eachattribute.Copyright = item.CopyRight;

                                    eachattribute.ID = item.ID;



                                    electricalConnectorPins.RefBaseClassPath = item.RefBaseClassPath;
                                }
                            }
                        }
                    }


                }

                
                for (int i = 0; i < device.ElectricalInterfaceInstances.Count; i++)
                {
                    List<ElectricalParameters> electricalInterfacenames = device.ElectricalInterfaceInstances[i];
                    InternalElementType electricalconnector = null;
                    InternalElementType electricalconnectorCode = null;
                    
                    InternalElementType electricalconnectorType = null;


                    foreach (var variable in electricalInterfacenames)
                    {
                        electricalconnector = electricalInterface.InternalElement.Append(variable.Connector.ToString());

                        var attributeofepconnector = electricalconnector.Attribute;
                        
                        foreach (var parametersofElectricalDataDataGridView in variable.listofElectricalDataDataGridViewParameters)
                        {
                           var eachAttribute =  attributeofepconnector.Append(parametersofElectricalDataDataGridView.Attributes.ToString());
                            eachAttribute.Unit = parametersofElectricalDataDataGridView.Units.ToString();
                            eachAttribute.Value = parametersofElectricalDataDataGridView.Values.ToString();
                            var refsemanticsOfEachAttribute = eachAttribute.RefSemantic.Append();
                            refsemanticsOfEachAttribute.Node.Add(parametersofElectricalDataDataGridView.ReferenceID.ToString());
                           
                        }


                        if (electricalconnectorCode == null)
                        {
                            electricalconnectorCode = electricalconnector.InternalElement.Append(variable.Connector.ToString()+variable.ConnectorCode.ToString()+variable.Pins+"Pins");

                           
                            foreach (var item in variable.listOfPinInfoDataGridViewParameters)
                            {
                                if (item.PinNumber != null)
                                {
                                    var eachpin = electricalconnectorCode.ExternalInterface.Append(item.PinNumber);
                                    var attributeofeachpin = eachpin.Attribute.Append(item.Attributes.ToString());
                                    attributeofeachpin.Unit = item.Units;
                                    attributeofeachpin.Value = item.Values;
                                    var refSemanticOfEachAttribute = attributeofeachpin.RefSemantic.Append();
                                    refSemanticOfEachAttribute.Node.Add(item.ReferenceID.ToString());

                                }
                                
                            }
                            
                            if (electricalconnectorType == null)
                            {
                                electricalconnectorType = electricalconnectorCode.InternalElement.Append(variable.ConnectorType.ToString());
                                
                            }
                        }
                     
                    }
                }
 
            }

           

            // create the PackageUri for the root aml file
            Uri partUri = PackUriHelper.CreatePartUri(new Uri("/" + fileName + "-root.aml", UriKind.Relative));


            // tcreate the aml file as a temporary file
            string path = Path.GetTempFileName();
            document.SaveToFile(path, true);

            if (isEdit)
            {
                // delete the old aml file
                amlx.Package.DeletePart(partUri);
                
                // delete all files in the amlx package.
               // Directory.Delete(Path.GetFullPath(amlx.ContainerFilename), true);

            }
           
            // write the new aml file into the package
            PackagePart root = amlx.AddRoot(path, partUri);
            
            
           if (!isEdit)
            {

                foreach (AttachablesDataGridViewParameters listWithUri in device.listWithURIConvertedToString)
                {
                    
                    if (listWithUri.ElementName != null)
                    {
                        Uri newuri = null;
                        newuri = new Uri(listWithUri.ElementName, UriKind.Relative);
                        amlx.AddAnyContent(root, listWithUri.FilePath.ToString(), newuri);
                       
                    }
                   

                }
            }
             DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(amlFilePath));
             foreach (FileInfo fileInfos in directory.GetFiles())
             {
                 if (fileInfos.Extension != ".amlx")
                 {
                     fileInfos.Delete();
                 }
                 

             }
           

            amlx.Save();
            amlx.Close();
            if (isEdit)
            {
                return "Sucessfully updated device!\nFilepath " + amlFilePath;
            }
            else
            {
                return "Device description file created!\nFilepath " + amlFilePath;
            }
        }
      
        /// <summary>
        /// Takes the url of the picture and setup in the value attribute of the corresponding internal element <paramref name="pic"/>.
        /// </summary>
        /// <param name="url">the absolut path to the picture or document in the internet</param>
        /// <param name="urltype">Picturetyp like 'DevicePicture' or 'DeviceIcon' and document type like "Short guide" or "Bill of materials" etc</param>
        /// <param name="externalname">The name of the externalElement</param>
        /// <param name="systemUnitClass">the systemUnitClass to insert the structure into</param>
        /// <returns></returns>
        public void interneturl(string url, string urltype, string externalname, SystemUnitClassType systemUnitClass)
        {

            // Create the InternalElement which refers to the picture
            InternalElementType urlIE = null;
            foreach (var internalElement in systemUnitClass.InternalElement)
            {
                if (internalElement.Name.Equals(urltype))
                {
                    urlIE = internalElement;
                    break;
                }
            }
            if (urlIE == null)
                urlIE = systemUnitClass.InternalElement.Append(urltype);

            // create the externalelement
            ExternalInterfaceType urlEI = null;
            foreach (var externalinterface in urlIE.ExternalInterface)
            {
                if (externalinterface.Name.Equals(externalname))
                {
                    urlEI = externalinterface;
                    break;
                }
            }
            if (urlEI == null)
                urlEI = urlIE.ExternalInterface.Append(externalname);

            urlEI.RefBaseClassPath = AutomationMLInterfaceClassLib.ExternalDataConnector;

            // create the refURI Attribute with the value of the path

            AttributeType urlAtt = null;
            if (urlEI.Attribute.GetCAEXAttribute("refURI") == null)
            {
                urlAtt = urlEI.Attribute.Append("refURI");
            }
            urlAtt.AttributeDataType = "xs:anyURI";
            urlAtt.Value = url.ToString();
            
        }

        /// <summary>
        /// Creates the Structur to reference a picture and set the correct value <paramref name="pic"/>.
        /// If the structur is already there, it will only update the value.
        /// </summary>
        /// <param name="pic">the absolut path to the picture</param>
        /// <param name="pictype">Picturetyp like 'DevicePicture' or 'DeviceIcon'</param>
        /// <param name="externalname">The name of the externalElement</param>
        /// <param name="systemUnitClass">the systemUnitClass to insert the structure into</param>
        /// <returns></returns>
        public Uri createPictureRef(string pic, string pictype, string externalname, SystemUnitClassType systemUnitClass)
        {
            // create the package paths
            FileInfo pictureInfo = new FileInfo(pic);
            Uri picturePath = new Uri(pictureInfo.Name, UriKind.Relative);
            Uri picturePart = PackUriHelper.CreatePartUri(picturePath);

            // Create the InternalElement which refers to the picture
            InternalElementType pictureIE = null;
            foreach (var internalElement in systemUnitClass.InternalElement)
            {
                if (internalElement.Name.Equals(pictype))
                {
                    pictureIE = internalElement;
                    break;
                }
            }
            if (pictureIE == null)
                pictureIE = systemUnitClass.InternalElement.Append(pictype);

            // create the externalelement
            ExternalInterfaceType pictureEI = null;
            foreach (var externalinterface in pictureIE.ExternalInterface)
            {
                if (externalinterface.Name.Equals(externalname))
                {
                    pictureEI = externalinterface;
                    break;
                }
            }
            if (pictureEI == null)
                pictureEI = pictureIE.ExternalInterface.Append(externalname);

            pictureEI.RefBaseClassPath = AutomationMLInterfaceClassLib.ExternalDataConnector;

            // create the refURI Attribute with the value of the path

            AttributeType pictureAtt = null;
            if (pictureEI.Attribute.GetCAEXAttribute("refURI") == null)
            {
                pictureAtt = pictureEI.Attribute.Append("refURI");
            }
            pictureAtt.AttributeDataType = "xs:anyURI";
            pictureAtt.Value = picturePart.ToString();
            

            return picturePart;
        }

        /// <summary>
        /// Creates the Structur to reference a document and set the correct value <paramref name="doc"/>.
        /// If the structur is already there, it will only update the value.
        /// </summary>
        /// <param name="doc">the absolut path to the document</param>
        /// <param name="doctype">Documenttype like 'Short Guide' or 'Bill of Materials'</param>
        /// <param name="externalname">The name of the externalElement</param>
        /// <param name="systemUnitClass">the systemUnitClass to insert the structure into</param>
        /// <returns></returns>
        public Uri createDocumentRef(string doc, string doctype, string externalname, SystemUnitClassType systemUnitClass)
        {
            // create the package paths
            FileInfo documentInfo = new FileInfo(doc);
            Uri documentPath = new Uri(documentInfo.Name, UriKind.Relative);
            Uri documentPart = PackUriHelper.CreatePartUri(documentPath);

            // Create the InternalElement which refers to the document
            InternalElementType documentIE = null;
            foreach (var internalElement in systemUnitClass.InternalElement)
            {
                if (internalElement.Name.Equals(doctype))
                {
                    documentIE = internalElement;
                    break;
                }
            }
            if (documentIE == null)
                documentIE = systemUnitClass.InternalElement.Append(doctype);

            // create the externalelement
            ExternalInterfaceType documentEI = null;
            foreach (var externalinterface in documentIE.ExternalInterface)
            {
                if (externalinterface.Name.Equals(externalname))
                {
                    documentEI = externalinterface;
                    break;
                }
            }
            if (documentEI == null)
                documentEI = documentIE.ExternalInterface.Append(externalname);

            documentEI.RefBaseClassPath = AutomationMLInterfaceClassLib.ExternalDataConnector;

            // create the refURI Attribute with the value of the path

            AttributeType pictureAtt = null;
            if (documentEI.Attribute.GetCAEXAttribute("refURI") == null)
            {
                pictureAtt = documentEI.Attribute.Append("refURI");
            }
            pictureAtt.AttributeDataType = "xs:anyURI";
            pictureAtt.Value = documentPart.ToString();

            return documentPart;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ie"></param>
        private void initCAEXattributes(InternalElementType ie)
        {
            initCAEXAttribute("CommunicationTechnology", "xs:string", ie);
            initCAEXAttribute("VendorName", "xs:string", ie);
            initCAEXAttribute("DeviceName", "xs:string", ie);
            initCAEXAttribute("ProductRange", "xs:string", ie);
            initCAEXAttribute("ProductGroup", "xs:string", ie);
            initCAEXAttribute("ProductFamily", "xs:string", ie);
            initCAEXAttribute("OrderNumber", "xs:string", ie);
            initCAEXAttribute("ProductNumber", "xs:string", ie);
            initCAEXAttribute("ProductText", "xs:string", ie);
            initCAEXAttribute("IPProtection", "xs:string", ie);
            initCAEXAttribute("VendorHomepage", "xs:string", ie);
            initCAEXAttribute("HardwareRelease", "xs:string", ie);
            initCAEXAttribute("SoftwareRelease", "xs:string", ie);
            initCAEXAttribute("OperatingTemperatureMin", "xs:double", ie);
            initCAEXAttribute("OperatingTemperatureMax", "xs:double", ie);
            initCAEXAttribute("VendorId", "xs:integer", ie);
            initCAEXAttribute("DeviceId", "xs:integer", ie);
        }

        /// <summary>
        /// Create a attribute <paramref name="attribute"/> with the given datatype <paramref name="datatype"/> in the <paramref name="ie"/>
        /// </summary>
        /// <param name="attribute">the name of the attribute</param>
        /// <param name="datatype">the xs datatype</param>
        /// <param name="ie">the internalelement for these attributes</param>
        /// <returns></returns>
        private AttributeType initCAEXAttribute(string attribute, string datatype, InternalElementType ie)
        {
            AttributeType attributeType = null;
            // check if the attribute exists, if not create it
            if (ie.Attribute.GetCAEXAttribute(attribute) == null)
            {
                attributeType = ie.Attribute.Append(attribute);
                attributeType.AttributeDataType = datatype;
            }
            else
            {
                ie.Attribute.GetCAEXAttribute(attribute).AttributeDataType = datatype;
            }
            return attributeType;
        }

        /// <summary>
        /// assign the values of the <paramref name="device"/> to the corresponding attributes
        /// </summary>
        /// <param name="ie">the DeviceIdentification InternalElement</param>
        /// <param name="device">the device for this aml</param>
        private void setCAEXattribute(InternalElementType ie, MWDevice device)
        {
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("CommunicationTechnology"), device.deviceType);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("VendorId"), device.vendorID);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("VendorName"), device.vendorName);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("DeviceId"), device.deviceID);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("DeviceName"), device.deviceName);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("ProductRange"), device.productRange);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("ProductGroup"), device.productGroup);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("ProductFamily"), device.productFamily);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("OrderNumber"), device.orderNumber);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("ProductNumber"), device.productNumber);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("ProductText"), device.productText);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("IPProtection"), device.ipProtection);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("OperatingTemperatureMin"), device.minTemperature);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("OperatingTemperatureMax"), device.maxTemperature);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("VendorHomepage"), device.vendorHomepage);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("HardwareRelease"), device.harwareRelease);
            writeIfNotNull(ie.Attribute.GetCAEXAttribute("SoftwareRelease"), device.softwareRelease);
        }

        /// <summary>
        /// Write the Value if it's not null / empty / NaN
        /// </summary>
        /// <param name="attribute">the attribute</param>
        /// <param name="value">the value. Expected types: string, int, double</param>
        private void writeIfNotNull(AttributeType attribute, object value)
        {
            if (value is string)
            {
                if (!String.IsNullOrEmpty((string)value))
                    attribute.Value = (string)value;
            }
            else if (value is double)
            {
                if (!Double.IsNaN((double)value) && value != null)
                {
                    attribute.Value = ((double)value).ToString();
                }
            }
            else if (value is int)
            {
                if (value != null)
                {
                    attribute.Value = value.ToString();
                }
            }
        }

      

        /// <summary>
        /// Calls the iodd2aml Converter using <see cref="System.Reflection"/>
        /// the converted iodd will be saved in an amlx
        /// </summary>
        /// <param name="filename">the path to the iodd file</param>
        /// <returns>the result message as a string</returns>
        public string ImportIODD2AML(string filename)
        {

            // This methode using Reflection to check if the libary is available at runtime
            // If it is, then it's calling the Convert Function
            // Iodd2AmlConverter.Libary.ConversionHandler.Convert(String ioddFileData, string amlFileName);

            // Read file and create one string with the content
            FileInfo fileInfo = new FileInfo(filename);

            FileStream fs = new FileStream(filename, FileMode.Open);
            StreamReader fileReader = new StreamReader(fs);
            String fileinput = fileReader.ReadToEnd();
            fileReader.Close();
            fs.Close();


            // Load Libary .dll
            Assembly assembly = Assembly.Load("Iodd2AmlConverter.Library");
            Type conversionHandler = null;
            // Iterate over all Types in the Libary and get the ConversionHandler Type
            foreach (Type type in assembly.ExportedTypes)
            {
                if (type.Name.Equals("ConversionHandler"))
                {
                    conversionHandler = type;
                    break;
                }
            }

            // Check if Class is available
            if (conversionHandler == null)
            {
                return "Couldn't find the IODD2AML parser libary (AMLRider)";
            }

            // Check if Methode is available
            MethodInfo convertMethode = conversionHandler.GetMethod("Convert");
            if (convertMethode == null)
            {
                return "Couldn't find the correct parse method. Try downgrading your Version of AMLRider";
            }

            // Call the static Methode
            object result = convertMethode.Invoke(null, new object[] { fileinput, Path.GetFileNameWithoutExtension(filename) + ".aml" });

            string converted = (string)result;

            if (converted == null)
            {
                System.Windows.Forms.MessageBox.Show("Maybe the file is invalid", "Error while converting the file!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return "The device was not imported because an error occured during convertion!";
            }

            return createAMLXFromString(converted, filename);
        }

        /// <summary>
        /// Calls the gsd2aml Converter using <see cref="System.Reflection"/>
        /// the converted gsdml will be saved in an amlx
        /// </summary>
        /// <param name="filename">the path to the gsdml file</param>
        /// <returns>the result message as a string</returns>
        public string ImportGSD2AML(string filename)
        {
            // This methode using Reflection to check if the libary is available at runtime
            // If it is, then it's calling the Convert Function
            // Gsd2Aml.Lib.Converter.Convert(string inputFilepath, bool strictValidation)

            Assembly assembly = Assembly.Load("Gsd2Aml.Lib");
            Type conversionHandler = null;
            // Iterate over all Types in the Libary and get the ConversionHandler Type
            foreach (Type type in assembly.ExportedTypes)
            {
                if (type.Name.Equals("Converter"))
                {
                    conversionHandler = type;
                    break;
                }
            }

            // Check if the Method is available
            MethodInfo convertMethod = conversionHandler.GetMethod("Convert", new[] { typeof(string), typeof(bool) });
            if (convertMethod == null)
            {
                return "Couldn't find the correct parse method. Try downgrading your Version of Gsd2Aml";
            }

            object result = null;
            try
            {
                // Call the static method
                result = convertMethod.Invoke(null, new object[] { filename, false });
            }
            catch (Exception ex)
            {
                return "There was an error converting the file!\n" + ex.Message;
            }

            string converted = (string)result;

            if (converted == null)
            {
                System.Windows.Forms.MessageBox.Show("Maybe the file is invalid", "Error while converting the file!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return "The device was not imported because an error occured during convertion!";
            }

            // Convert string to stream for the CAEXDocument
            return createAMLXFromString(converted, filename);
        }

        /// <summary>
        /// Create an AMLX file with the aml file as string input
        /// </summary>
        /// <param name="caex">the complete aml file as a string</param>
        /// <param name="filename">the path to the original gsdml/iodd file</param>
        /// <returns>the result message as a string</returns>
        private string createAMLXFromString(string caex, string filename)
        {
            // create the CAEXDocument from byte string
            byte[] bytearray = System.Text.Encoding.Unicode.GetBytes(caex);
            CAEXDocument document = CAEXDocument.LoadFromBinary(bytearray);

            // create the amlx file
            string name = Path.GetFileNameWithoutExtension(filename);
            AutomationMLContainer amlx = new AutomationMLContainer(".\\modellingwizard\\" + name + ".amlx", FileMode.Create);

            // create the aml package path
            Uri partUri = PackUriHelper.CreatePartUri(new Uri("/" + name + "-root.aml", UriKind.Relative));

            // create a temp aml file
            string path = Path.GetTempFileName();
            document.SaveToFile(path, true);

            // copy the new aml into the package
            PackagePart root = amlx.AddRoot(path, partUri);

            // copy the original file into the package
            Uri gsdURI = new Uri(new FileInfo(filename).Name, UriKind.Relative);
            Uri gsdPartURI = PackUriHelper.CreatePartUri(gsdURI);
            amlx.AddAnyContent(root, filename, gsdPartURI);

            amlx.Save();
            amlx.Close();

            return "Sucessfully imported device!\nCreated File " + Path.GetFullPath(".\\modellingwizard\\" + name + ".amlx");
        }

        public enum MWFileType
        {
            IODD, GSD
        }

        public class MWObject
        {
            // Just as an interface
        }
        public void copyFiles(string sourceFilePath, string destinationFilePath )
        {
            string sourFile = Path.GetFileName(sourceFilePath);
            string destFile = Path.Combine(destinationFilePath, sourFile);
            File.Copy(sourceFilePath, destFile, true);
        }
    }
}
