using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aml.Engine.CAEX;

namespace Aml.Editor.Plugin
{
    class SearchAMLComponentFile
    {
        public Dictionary<string, List<List<ClassOfListsFromReferencefile>>> DictionaryofElectricalConnectorType { get; set; }
        public Dictionary<string, List<List<ClassOfListsFromReferencefile>>> DictioanryofElectricalConnectorPinType { get; set; }


        public Dictionary<string, List<List<ClassOfListsFromReferencefile>>> DictionaryofRolesforAutomationComponenet { get; set; }
        public Dictionary<string, List<List<ClassOfListsFromReferencefile>>> DictionaryofRoles { get; set; }


        public SearchAMLComponentFile()
        {
            
        }


        public void CheckForAttributesOfExternalIterface(int i, ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (externalInterface.Attribute.Exists)
            {
                foreach (var attribute in externalInterface.Attribute)
                {
                    StoreEachAttributeValueInListOfExternalIterface(i, attributelist, attribute, externalInterface);
                    CkeckForNestedAttributesOfExternalIterface(i,attribute,  externalInterface);
                    
                }

            }
            if (!externalInterface.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                try
                {
                    if (DictionaryofElectricalConnectorType.ContainsKey( "("+i+")"+ externalInterface.Name.ToString()+ "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictionaryofElectricalConnectorType["(" + i + ")" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryofElectricalConnectorType.Add("(" + i + ")" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }
        public void CkeckForNestedAttributesOfExternalIterface(int i, AttributeType attributeType,  ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    StoreEachAttributeValueInListOfExternalIterface(i, attributelist, attributeinattribute, attributeType, externalInterface);
                    CkeckForNestedAttributesOfExternalIterface(i,attributeinattribute,  externalInterface);
                   
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                if (DictionaryofElectricalConnectorType.ContainsKey("(" + i + ")" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryofElectricalConnectorType["(" + i + ")" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictionaryofElectricalConnectorType.Add("(" + i + ")" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }

            }

        }

        public void StoreEachAttributeValueInListOfExternalIterface(int i, List<List<ClassOfListsFromReferencefile>> list,
           AttributeType attributeType, ExternalInterfaceType externalInterface)
        {
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            attributeparameters.DataType = attributeType.AttributeDataType;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.AttributePath = attributeType.AttributePath;
            attributeparameters.RefSemanticList = attributeType.RefSemantic;
           // attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;

            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (DictionaryofElectricalConnectorType.ContainsKey("(" + i + ")" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryofElectricalConnectorType["(" + i + ")" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictionaryofElectricalConnectorType.Add("(" + i + ")" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void StoreEachAttributeValueInListOfExternalIterface(int i, List<List<ClassOfListsFromReferencefile>> list,
            AttributeType AttributeInAttribute, AttributeType attributeType, ExternalInterfaceType externalInterface)
        {
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            // In the following parameters on right hand side "attributeType" has been changed to "AttributeInAttribute" this has been repeated to all 
            // methods of name "StoreEachAttributeValuesInList" with four parameters.
            attributeparameters.Name = AttributeInAttribute.Name;
            attributeparameters.Value = AttributeInAttribute.Value;
            attributeparameters.Default = AttributeInAttribute.DefaultValue;
            attributeparameters.Unit = AttributeInAttribute.Unit;
            attributeparameters.DataType = AttributeInAttribute.AttributeDataType;
            attributeparameters.Description = AttributeInAttribute.Description;
            attributeparameters.CopyRight = AttributeInAttribute.Copyright;
            attributeparameters.AttributePath = AttributeInAttribute.AttributePath;
            attributeparameters.RefSemanticList = AttributeInAttribute.RefSemantic;
           // attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            if (DictionaryofElectricalConnectorType.ContainsKey("(" + i + ")" + externalInterface.Name.ToString()
                + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
            {
                DictionaryofElectricalConnectorType["(" + i + ")" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
            }
            else
            {
                DictionaryofElectricalConnectorType.Add("(" + i + ")" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
            }

        }






        public void CheckForAttributesOfEclectricalConnectorPins(int i, ExternalInterfaceType externalInterface, ExternalInterfaceType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (externalInterface.Attribute.Exists)
            {
                foreach (var attribute in externalInterface.Attribute)
                {
                    StoreEachAttributeValueInListOfElectricalConnectorPins(i, attributelist, attribute, externalInterface, classType);
                    CkeckForNestedAttributesOfElectricalConnectorPins(i, attribute, externalInterface, classType);
                   
                }

            }
            if (!externalInterface.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                try
                {
                    if (DictioanryofElectricalConnectorPinType.ContainsKey("(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}"
                       + externalInterface.Name.ToString() + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictioanryofElectricalConnectorPinType["(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    else
                    {
                        DictioanryofElectricalConnectorPinType.Add("(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }
        public void CkeckForNestedAttributesOfElectricalConnectorPins(int i, AttributeType attributeType, ExternalInterfaceType externalInterface, 
            ExternalInterfaceType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    StoreEachAttributeValueInListOfElectricalConnectorPins(i, attributelist, attributeinattribute, attributeType, externalInterface, classType);
                    CkeckForNestedAttributesOfElectricalConnectorPins(i, attributeinattribute, externalInterface, classType);
                    
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                if (DictioanryofElectricalConnectorPinType.ContainsKey("(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictioanryofElectricalConnectorPinType["(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictioanryofElectricalConnectorPinType.Add("(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }

            }

        }

        public void StoreEachAttributeValueInListOfElectricalConnectorPins(int i, List<List<ClassOfListsFromReferencefile>> list,
           AttributeType attributeType, ExternalInterfaceType externalInterface, ExternalInterfaceType classType)
        {
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            attributeparameters.DataType = attributeType.AttributeDataType;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.AttributePath = attributeType.AttributePath;
            attributeparameters.RefSemanticList = attributeType.RefSemantic;
           // attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;

            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (DictioanryofElectricalConnectorPinType.ContainsKey("(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictioanryofElectricalConnectorPinType["(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictioanryofElectricalConnectorPinType.Add("(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void StoreEachAttributeValueInListOfElectricalConnectorPins(int i, List<List<ClassOfListsFromReferencefile>> list,
            AttributeType AttributeInAttribute, AttributeType attributeType, ExternalInterfaceType externalInterface, ExternalInterfaceType classType)
        {
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            // In the following parameters on right hand side "attributeType" has been changed to "AttributeInAttribute" this has been repeated to all 
            // methods of name "StoreEachAttributeValuesInList" with four parameters.
            attributeparameters.Name = AttributeInAttribute.Name;
            attributeparameters.Value = AttributeInAttribute.Value;
            attributeparameters.Default = AttributeInAttribute.DefaultValue;
            attributeparameters.Unit = AttributeInAttribute.Unit;
            attributeparameters.DataType = AttributeInAttribute.AttributeDataType;
            attributeparameters.Description = AttributeInAttribute.Description;
            attributeparameters.CopyRight = AttributeInAttribute.Copyright;
            attributeparameters.AttributePath = AttributeInAttribute.AttributePath;
            attributeparameters.RefSemanticList = AttributeInAttribute.RefSemantic;
            //attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            if (DictioanryofElectricalConnectorPinType.ContainsKey("(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
            {
                DictioanryofElectricalConnectorPinType["(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
            }
            else
            {
                DictioanryofElectricalConnectorPinType.Add("(" + i + ")" + classType.Name.ToString() + "{" + "Class:" + "  " + classType.BaseClass + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
            }

        }






        // read supported role class attributes of System Unit Class i.e. "Component Attributes "

        public void CheckForAttributesOfComponent(int i, SupportedRoleClassType supportedRoleClass, SystemUnitFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    StoreEachAttributeValueInListOfComponent(i, attributelist, attribute, supportedRoleClass, classType);
                    CkeckForNestedAttributesOfComponent(i, attribute, supportedRoleClass, classType);
                    
                }

            }
            if (!classType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                try
                {
                    if (DictionaryofRolesforAutomationComponenet.ContainsKey("(" + i + ")" + supportedRoleClass.RoleReference.ToString() ))
                    {
                        DictionaryofRolesforAutomationComponenet["(" + i + ")" + supportedRoleClass.RoleReference.ToString()].AddRange(list);
                    }
                    else
                    {
                        DictionaryofRolesforAutomationComponenet.Add("(" + i + ")" + supportedRoleClass.RoleReference.ToString(), list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }
        public void CkeckForNestedAttributesOfComponent(int i, AttributeType attributeType, SupportedRoleClassType supportedRoleClass, SystemUnitFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    StoreEachAttributeValueInListOfComponent(i, attributelist, attributeinattribute, attributeType, supportedRoleClass, classType);
                    CkeckForNestedAttributesOfComponent(i, attributeinattribute, supportedRoleClass, classType);
                    
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                try
                {
                    if (DictionaryofRolesforAutomationComponenet.ContainsKey("(" + i + ")" + supportedRoleClass.RoleReference.ToString()))
                    {
                        DictionaryofRolesforAutomationComponenet["(" + i + ")" + supportedRoleClass.RoleReference.ToString()].AddRange(list);
                    }
                    else
                    {
                        DictionaryofRolesforAutomationComponenet.Add("(" + i + ")" + supportedRoleClass.RoleReference.ToString(), list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }

        public void StoreEachAttributeValueInListOfComponent(int i, List<List<ClassOfListsFromReferencefile>> list,
           AttributeType attributeType, SupportedRoleClassType supportedRoleClass, SystemUnitFamilyType classType)
        {
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            attributeparameters.DataType = attributeType.AttributeDataType;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.AttributePath = attributeType.AttributePath;
            attributeparameters.RefSemanticList = attributeType.RefSemantic;
            // attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.SupportesRoleClassType = supportedRoleClass.RefRoleClassPath.ToString();
            //attributeparameters.ID = supportedRoleClass.ID;

            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (DictionaryofRolesforAutomationComponenet.ContainsKey("(" + i + ")" + supportedRoleClass.RoleReference.ToString()))
                {
                    DictionaryofRolesforAutomationComponenet["(" + i + ")" + supportedRoleClass.RoleReference.ToString()].AddRange(list);
                }
                else
                {
                    DictionaryofRolesforAutomationComponenet.Add("(" + i + ")" + supportedRoleClass.RoleReference.ToString(), list);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void StoreEachAttributeValueInListOfComponent(int i, List<List<ClassOfListsFromReferencefile>> list,
            AttributeType AttributeInAttribute, AttributeType attributeType, SupportedRoleClassType supportedRoleClass, SystemUnitFamilyType classType)
        {
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            // In the following parameters on right hand side "attributeType" has been changed to "AttributeInAttribute" this has been repeated to all 
            // methods of name "StoreEachAttributeValuesInList" with four parameters.
            attributeparameters.Name = AttributeInAttribute.Name;
            attributeparameters.Value = AttributeInAttribute.Value;
            attributeparameters.Default = AttributeInAttribute.DefaultValue;
            attributeparameters.Unit = AttributeInAttribute.Unit;
            attributeparameters.DataType = AttributeInAttribute.AttributeDataType;
            attributeparameters.Description = AttributeInAttribute.Description;
            attributeparameters.CopyRight = AttributeInAttribute.Copyright;
            attributeparameters.AttributePath = AttributeInAttribute.AttributePath;
            attributeparameters.RefSemanticList = AttributeInAttribute.RefSemantic;
            // attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.SupportesRoleClassType = supportedRoleClass.RefRoleClassPath.ToString();
            /*attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;*/


            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (DictionaryofRolesforAutomationComponenet.ContainsKey("(" + i + ")" + supportedRoleClass.RoleReference.ToString()))
                {
                    DictionaryofRolesforAutomationComponenet["(" + i + ")" + supportedRoleClass.RoleReference.ToString()].AddRange(list);
                }
                else
                {
                    DictionaryofRolesforAutomationComponenet.Add("(" + i + ")" + supportedRoleClass.RoleReference.ToString(), list);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
