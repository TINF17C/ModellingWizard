using Aml.Engine.AmlObjects;
using Aml.Engine.CAEX;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
            string vendorCompanyNameFilePath = "";
          
          

            string fileName = device.fileName;

            string amlFilePath = System.IO.Path.Combine(device.filepath, fileName + ".amlx");


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
            AutomationMLBaseRoleClassLibType.RoleClassLib(document) ;
            AutomationMLInterfaceClassLibType.InterfaceClassLib(document) ;

            var structureRoleFamilyType = AutomationMLBaseRoleClassLibType.RoleClassLib(document).Structure;
            

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
                 foreach (var pair in device.DictionaryForRoleClassofComponent)
                 {

                    SupportedRoleClassType supportedRoleClass = null;


                    Match numberfromElectricalConnectorType = Regex.Match(pair.Key.ToString(), @"\((\d+)\)");
                     string initialnumberbetweenparanthesisofElectricalConnectorType = numberfromElectricalConnectorType.Groups[1].Value;
                   // string stringinparanthesis = Regex.Match(pair.Key.ToString(), @"\{(\d+)\}").Groups[1].Value;

                    string supportedRoleClassFromDictionary = Regex.Replace(pair.Key.ToString(), @"\(.+?\)", "");
                     supportedRoleClassFromDictionary = Regex.Replace(supportedRoleClassFromDictionary, @"\{.+?\}", "");


                   
                    var SRC = systemUnitClass.SupportedRoleClass.Append();

                    

                    var attributesOfSystemUnitClass = systemUnitClass.Attribute;

                     foreach (var valueList in pair.Value)
                     {
                         foreach (var item in valueList)
                         {
                           
                            
                            if ( item.AttributePath.Contains("/") || item.AttributePath.Contains("."))
                            {
                                int count = 2;
                                int counter = 0;
                                Stack<char> stack = new Stack<char>();
                                string searchAttributeName = item.AttributePath.Substring(0, item.AttributePath.Length - item.Name.Length);

                                foreach (var character in searchAttributeName.Reverse())
                                {
                                   
                                    if (!char.IsLetterOrDigit(character))
                                    {
                                        counter++;
                                        if (counter == count)
                                        {
                                            break;
                                        }
                                       
                                    }
                                    if (char.IsLetterOrDigit(character))
                                    {
                                        stack.Push(character);
                                    }
                                    
                                }

                                string finalAttributeName = new string(stack.ToArray());

                                foreach (var attribute in systemUnitClass.Attribute)
                                {
                                    if (attribute.Name == finalAttributeName)
                                    {
                                        var eachattribute = attribute.Attribute.Append(item.Name.ToString());
                                        eachattribute.Value = item.Value;
                                        eachattribute.DefaultValue = item.Default;
                                        eachattribute.Unit = item.Unit;
                                        eachattribute.AttributeDataType = item.DataType;
                                        eachattribute.Description = item.Description;
                                        eachattribute.Copyright = item.CopyRight;

                                        eachattribute.ID = item.ID;

                                        foreach (var val in item.RefSemanticList.Elements)
                                        {
                                            var refsem = eachattribute.RefSemantic.Append();
                                            refsem.CorrespondingAttributePath = val.FirstAttribute.Value;
                                            
                                        }



                                        SRC.RefRoleClassPath = item.SupportesRoleClassType;

                                    }
                                    if (attribute.Attribute.Exists)
                                    {

                                        SearchForAttributesInsideAttributesofAutomationComponent(finalAttributeName, attribute, item,SRC);
                                    }
                                }
                               
                            }
                            else
                            {
                                var eachattribute = attributesOfSystemUnitClass.Append(item.Name.ToString());
                                eachattribute.Value = item.Value;
                                eachattribute.DefaultValue = item.Default;
                                eachattribute.Unit = item.Unit;
                                eachattribute.AttributeDataType = item.DataType;
                                eachattribute.Description = item.Description;
                                eachattribute.Copyright = item.CopyRight;
                                
                                eachattribute.ID = item.ID;

                               
                                foreach (var val in item.RefSemanticList.Elements)
                                {
                                    var refsem = eachattribute.RefSemantic.Append();
                                    refsem.CorrespondingAttributePath = val.FirstAttribute.Value;
                                }


                                SRC.RefRoleClassPath = item.SupportesRoleClassType;
                            }
                            

                        }
                     }
                   

                    foreach (var pairofList in device.DictionaryForExternalInterfacesUnderRoleClassofComponent)
                     {
                         Match numberfromElectricalConnectorPins = Regex.Match(pairofList.Key.ToString(), @"\((\d+)\)");
                         string initialnumberbetweenparanthesisElectricalConnectorPins = numberfromElectricalConnectorPins.Groups[1].Value;

                         string electricalConnectorPinName = Regex.Replace(pairofList.Key.ToString(), @"\(.*?\)", "");
                         electricalConnectorPinName = Regex.Replace(electricalConnectorPinName, @"\{.*?\}", "");
                         electricalConnectorPinName = electricalConnectorPinName.Replace(supportedRoleClassFromDictionary, "");




                         /*if (initialnumberbetweenparanthesisofElectricalConnectorType == initialnumberbetweenparanthesisElectricalConnectorPins)
                         {
                             supportedRoleClass.RoleReference = pairofList.Key.ToString();

                             systemUnitClass.SupportedRoleClass.Append(supportedRoleClass);
                             systemUnitClass.BaseClass.Name = supportedRoleClassFromDictionary;

                             var attributesOfSystemUnitClassattributes = systemUnitClass.Attribute;

                             foreach (var valueList in pairofList.Value)
                             {
                                 foreach (var item in valueList)
                                 {
                                     var eachattribute = attributesOfSystemUnitClassattributes.Append(item.Name.ToString());
                                     eachattribute.Value = item.Value;
                                     eachattribute.DefaultValue = item.Default;
                                     eachattribute.Unit = item.Unit;
                                     //eachattribute.AttributeDataType = 
                                     eachattribute.Description = item.Description;
                                     eachattribute.Copyright = item.CopyRight;

                                     eachattribute.ID = item.ID;



                                    // systemUnitClass.BaseClass.Name   = item.RefBaseClassPath;
                                 }
                             }
                         }*/
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


           

          

            // Create the internalElement Electrical Interfaces

            if (device.vendorName != null)
            {
                InternalElementType electricalInterface = null;
                RoleRequirementsType roleRequirements = null ;
                foreach (var internalElement in systemUnitClass.InternalElement)
                {
                    if (internalElement.Name.Equals("ElectricalInterfaces"))
                    {
                        electricalInterface = internalElement;
                        roleRequirements = electricalInterface.RoleRequirements.Append();
                        roleRequirements.RefBaseRoleClassPath = structureRoleFamilyType.CAEXPath();
                        break;
                    }
                }
                if (electricalInterface == null)
                    electricalInterface = systemUnitClass.InternalElement.Append("ElectricalInterfaces");
                   roleRequirements = electricalInterface.RoleRequirements.Append();

                    roleRequirements.RefBaseRoleClassPath = structureRoleFamilyType.CAEXPath();

                foreach (var pair in device.DictionaryForInterfaceClassesInElectricalInterfaces)
                {

                    InternalElementType internalElementofElectricalConnectorType = null;
                    ExternalInterfaceType electricalConnectorType = null;

                    ExternalInterfaceType electricalConnectorPins = null;

                    Match numberfromElectricalConnectorType = Regex.Match(pair.Key.ToString(), @"\((\d+)\)");
                    string initialnumberbetweenparanthesisofElectricalConnectorType = numberfromElectricalConnectorType.Groups[1].Value;


                    string electricalConnectorTypeName = Regex.Replace(pair.Key.ToString(), @"\(.+?\)", "");
                    electricalConnectorTypeName = Regex.Replace(electricalConnectorTypeName, @"\{.+?\}", "");

                    internalElementofElectricalConnectorType = electricalInterface.InternalElement.Append(electricalConnectorTypeName);

                    electricalConnectorType = internalElementofElectricalConnectorType.ExternalInterface.Append(electricalConnectorTypeName);

                    var attributesOfConnectorType = electricalConnectorType.Attribute;

                    foreach (var valueList in pair.Value)
                    {
                        foreach (var item in valueList)
                        {
                            if (item.AttributePath.Contains("/") || item.AttributePath.Contains("."))
                            {
                                int count = 2;
                                int counter = 0;
                                Stack<char> stack = new Stack<char>();
                                string searchAttributeName = item.AttributePath.Substring(0, item.AttributePath.Length - item.Name.Length);

                                foreach (var character in searchAttributeName.Reverse())
                                {

                                    if (!char.IsLetterOrDigit(character))
                                    {
                                        counter++;
                                        if (counter == count)
                                        {
                                            break;
                                        }

                                    }
                                    if (char.IsLetterOrDigit(character))
                                    {
                                        stack.Push(character);
                                    }

                                }

                                string finalAttributeName = new string(stack.ToArray());

                                foreach (var attribute in electricalConnectorType.Attribute)
                                {
                                    if (attribute.Name == finalAttributeName)
                                    {
                                        var eachattribute = attribute.Attribute.Append(item.Name.ToString());
                                        eachattribute.Value = item.Value;
                                        eachattribute.DefaultValue = item.Default;
                                        eachattribute.Unit = item.Unit;
                                        eachattribute.AttributeDataType = item.DataType;
                                        eachattribute.Description = item.Description;
                                        eachattribute.Copyright = item.CopyRight;

                                        eachattribute.ID = item.ID;

                                        foreach (var val in item.RefSemanticList.Elements)
                                        {
                                            var refsem = eachattribute.RefSemantic.Append();
                                            refsem.CorrespondingAttributePath = val.FirstAttribute.Value;

                                        }

                                        electricalConnectorType.RefBaseClassPath = item.RefBaseClassPath;

                                    }
                                    if (attribute.Attribute.Exists)
                                    {

                                        SearchAttributesInsideAttributesOFElectricConnectorType(finalAttributeName, attribute, item, electricalConnectorType);
                                    }
                                }

                            }
                            else
                            {
                                var eachattribute = attributesOfConnectorType.Append(item.Name.ToString());
                                eachattribute.Value = item.Value;
                                eachattribute.DefaultValue = item.Default;
                                eachattribute.Unit = item.Unit;
                                eachattribute.AttributeDataType = item.DataType;
                                eachattribute.Description = item.Description;
                                eachattribute.Copyright = item.CopyRight;

                                eachattribute.ID = item.ID;

                                foreach (var val in item.RefSemanticList.Elements)
                                {
                                    var refsem = eachattribute.RefSemantic.Append();
                                    refsem.CorrespondingAttributePath = val.FirstAttribute.Value;

                                }

                                electricalConnectorType.RefBaseClassPath = item.RefBaseClassPath;
                            }


                           
                        }
                    }


                    foreach (var pairofList in device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces)
                    {
                        Match numberfromElectricalConnectorPins = Regex.Match(pairofList.Key.ToString(), @"\((\d+)\)");
                        string initialnumberbetweenparanthesisElectricalConnectorPins = numberfromElectricalConnectorPins.Groups[1].Value;

                        string electricalConnectorPinName = Regex.Replace(pairofList.Key.ToString(), @"\(.*?\)", "");
                        electricalConnectorPinName = Regex.Replace(electricalConnectorPinName, @"\{.*?\}", "");
                        electricalConnectorPinName = electricalConnectorPinName.Replace(electricalConnectorTypeName,"");

                        


                        if (initialnumberbetweenparanthesisofElectricalConnectorType == initialnumberbetweenparanthesisElectricalConnectorPins)
                        {
                            electricalConnectorPins = electricalConnectorType.ExternalInterface.Append(electricalConnectorPinName);

                            var attributesOfConnectorPins = electricalConnectorPins.Attribute;

                            foreach (var valueList in pairofList.Value)
                            {
                                foreach (var item in valueList)
                                {
                                    if (item.AttributePath.Contains("/") || item.AttributePath.Contains("."))
                                    {
                                        int count = 2;
                                        int counter = 0;
                                        Stack<char> stack = new Stack<char>();
                                        string searchAttributeName = item.AttributePath.Substring(0, item.AttributePath.Length - item.Name.Length);

                                        foreach (var character in searchAttributeName.Reverse())
                                        {

                                            if (!char.IsLetterOrDigit(character))
                                            {
                                                counter++;
                                                if (counter == count)
                                                {
                                                    break;
                                                }

                                            }
                                            if (char.IsLetterOrDigit(character))
                                            {
                                                stack.Push(character);
                                            }

                                        }

                                        string finalAttributeName = new string(stack.ToArray());

                                        foreach (var attribute in electricalConnectorPins.Attribute)
                                        {
                                            if (attribute.Name == finalAttributeName)
                                            {
                                                var eachattribute = attribute.Attribute.Append(item.Name.ToString());
                                                eachattribute.Value = item.Value;
                                                eachattribute.DefaultValue = item.Default;
                                                eachattribute.Unit = item.Unit;
                                                eachattribute.AttributeDataType = item.DataType;
                                                eachattribute.Description = item.Description;
                                                eachattribute.Copyright = item.CopyRight;

                                                eachattribute.ID = item.ID;

                                                foreach (var val in item.RefSemanticList.Elements)
                                                {
                                                    var refsem = eachattribute.RefSemantic.Append();
                                                    refsem.CorrespondingAttributePath = val.FirstAttribute.Value;

                                                }

                                                electricalConnectorPins.RefBaseClassPath = item.RefBaseClassPath;

                                            }
                                            if (attribute.Attribute.Exists)
                                            {

                                                SearchAttributesInsideAttributesOFElectricConnectorType(finalAttributeName, attribute, item, electricalConnectorPins);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        var eachattribute = attributesOfConnectorPins.Append(item.Name.ToString());
                                        eachattribute.Value = item.Value;
                                        eachattribute.DefaultValue = item.Default;
                                        eachattribute.Unit = item.Unit;
                                        eachattribute.AttributeDataType = item.DataType;
                                        eachattribute.Description = item.Description;
                                        eachattribute.Copyright = item.CopyRight;

                                        eachattribute.ID = item.ID;

                                        foreach (var val in item.RefSemanticList.Elements)
                                        {
                                            var refsem = eachattribute.RefSemantic.Append();
                                            refsem.CorrespondingAttributePath = val.FirstAttribute.Value;

                                        }

                                        electricalConnectorPins.RefBaseClassPath = item.RefBaseClassPath;
                                    }

                                   
                                }
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

        public void SearchForAttributesInsideAttributesofAutomationComponent(string searchName, AttributeType attribute, ClassOfListsFromReferencefile item
            ,SupportedRoleClassType SRC)
        {
            foreach (var nestedAttribute in attribute.Attribute)
            {
                if (nestedAttribute.Name == searchName)
                {
                    var eachattribute = nestedAttribute.Attribute.Append(item.Name.ToString());
                    eachattribute.Value = item.Value;
                    eachattribute.DefaultValue = item.Default;
                    eachattribute.Unit = item.Unit;
                    eachattribute.AttributeDataType = item.DataType;
                    eachattribute.Description = item.Description;
                    eachattribute.Copyright = item.CopyRight;

                    eachattribute.ID = item.ID;

                    foreach (var val in item.RefSemanticList.Elements)
                    {
                        var refsem = eachattribute.RefSemantic.Append();
                        refsem.CorrespondingAttributePath = val.FirstAttribute.Value;
                    }


                    SRC.RefRoleClassPath = item.SupportesRoleClassType;
                }
                if (nestedAttribute.Attribute.Exists)
                {
                    SearchForAttributesInsideAttributesofAutomationComponent(searchName, nestedAttribute, item, SRC);
                }
            }
           
        }

        public void SearchAttributesInsideAttributesOFElectricConnectorType(string searchName, AttributeType attribute, ClassOfListsFromReferencefile item
            ,ExternalInterfaceType electricConnectorType)
        {
            foreach (var nestedAttribute in attribute.Attribute)
            {
                
                if (nestedAttribute.Name == searchName)
                {
                    var eachattribute = nestedAttribute.Attribute.Append(item.Name.ToString());
                    eachattribute.Value = item.Value;
                    eachattribute.DefaultValue = item.Default;
                    eachattribute.Unit = item.Unit;
                    eachattribute.AttributeDataType = item.DataType;
                    eachattribute.Description = item.Description;
                    eachattribute.Copyright = item.CopyRight;

                    eachattribute.ID = item.ID;
                    foreach (var val in item.RefSemanticList.Elements)
                    {
                        var refsem = eachattribute.RefSemantic.Append();
                        refsem.CorrespondingAttributePath = val.FirstAttribute.Value;

                    }


                    electricConnectorType.RefBaseClassPath = item.RefBaseClassPath;

                }
               
                if (nestedAttribute.Attribute.Exists)
                {
                    SearchAttributesInsideAttributesOFElectricConnectorType(searchName, nestedAttribute, item, electricConnectorType);
                }
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
