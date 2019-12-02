﻿using System;
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
                    CkeckForNestedAttributesOfExternalIterface(i,attribute,  externalInterface);
                    StoreEachAttributeValueInListOfExternalIterface(i,attributelist,  attribute, externalInterface);
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
                    CkeckForNestedAttributesOfExternalIterface(i,attributeinattribute,  externalInterface);
                    StoreEachAttributeValueInListOfExternalIterface(i,attributelist, attributeinattribute,  attributeType, externalInterface);
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
            // attributeparameters.Semantic = attributeType.RefSemantic;
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
            // attributeparameters.Semantic = attributeType.RefSemantic;
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
                    CkeckForNestedAttributesOfElectricalConnectorPins(i, attribute, externalInterface, classType);
                    StoreEachAttributeValueInListOfElectricalConnectorPins(i, attributelist, attribute, externalInterface, classType);
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
                    CkeckForNestedAttributesOfElectricalConnectorPins(i, attributeinattribute, externalInterface, classType);
                    StoreEachAttributeValueInListOfElectricalConnectorPins(i, attributelist, attributeinattribute, attributeType, externalInterface, classType);
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
            // attributeparameters.Semantic = attributeType.RefSemantic;
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
            // attributeparameters.Semantic = attributeType.RefSemantic;
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


    }
}
