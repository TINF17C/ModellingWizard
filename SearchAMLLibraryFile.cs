using System;
using System.Collections.Generic;
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
using System.IO.Compression;
using Aml.Editor.Plugin.Contracts;

namespace Aml.Editor.Plugin
{
    /// <summary>
    /// This class reads the library file loaded in to the plugin "Role Class Library TreeView" and "Interface Class Library TreeView"
    /// </summary>
    class SearchAMLLibraryFile
    {
        /// <summary>
        /// These are the properties iof this class i.e. dictionaries where all attribute values from AML file are strored and#
        /// further retrived in "Device Description Class" to edi values by user.
        /// </summary>
        /// 
        public Dictionary<string, string> DictioanryOfIDofInterfaceClassLibraryNodes { get; set; }
       

        public Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofRoleClassattributes { get; set; }

        public Dictionary<string, List<List<ClassOfListsFromReferencefile>>> DictionaryForInterfaceClassInstancesAttributes { get; set; }
        public Dictionary<string, List<List<ClassOfListsFromReferencefile>>> DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib { get; set; }

        public Dictionary<string, List<List<ClassOfListsFromReferencefile>>> DictionaryForRoleClassInstanceAttributes { get; set; }
        public Dictionary<string, List<List<ClassOfListsFromReferencefile>>> DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib { get; set; }

        public string referencedClassName { get; set; }

        /// <summary>
        /// This is the constructor of this class where all properties are intitialised to there early state
        /// </summary>
        public SearchAMLLibraryFile()

        {

        }



        /// <summary>
        /// This method is responsible to iterate over "Interafce Class Libraries & Interafce Classes in it" in InterfaceClassLib in AML file to find "Referenced Class Names" 
        /// to get Inherited Attributres of these Interface Classes. 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="referencedClassName"></param>
        /// <param name="classType"></param>

        public void SearchForReferencedClassName(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType)
        {
            string referencedClassNameofReferencedClassName = "";


            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.InterfaceClassLib)
            {
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.InterfaceClass)
                {

                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {
                        if (classTypeSearchForReferencedClassName.ExternalInterface.Exists)
                        {
                            foreach (var externalInterface in classTypeSearchForReferencedClassName.ExternalInterface)
                            {
                                if (externalInterface.BaseClass != null)
                                {
                                    referencedClassName = externalInterface.BaseClass.ToString();
                                    CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalInterface);
                                    SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalInterface);
                                }
                               
                            }
                        }


                        CheckForAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, classType);
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;

                            SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType);
                        }

                    }
                    if (classTypeSearchForReferencedClassName.InterfaceClass.Exists)
                    {
                        SearchForInterfaceClassesInsideInterfaceClass(doc, referencedClassName, classType, classTypeSearchForReferencedClassName);

                    }


                }
            }

        }



        public void SearchForInterfaceClassesInsideInterfaceClass(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType,
           InterfaceFamilyType classTypeSearchForReferencedClassName)
        {
            string referencedClassNameofReferencedClassName = "";
            foreach (var item in classTypeSearchForReferencedClassName.InterfaceClass)
            {
                if (item.Name == referencedClassName)
                {
                    if (item.ExternalInterface.Exists)
                    {
                        foreach (var externalInterface in item.ExternalInterface)
                        {
                            if (externalInterface.BaseClass != null)
                            {
                                referencedClassName = externalInterface.BaseClass.ToString();
                                CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalInterface);
                                SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalInterface);
                            }
                            
                        }
                    }

                    CheckForAttributesOfReferencedClassName(item, classType);
                    if (item.ReferencedClassName != "" && item.ReferencedClassName != item.Name)
                    {
                        referencedClassNameofReferencedClassName = item.ReferencedClassName;

                        SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType);
                    }

                }
                if (item.InterfaceClass.Exists)
                {
                    SearchForInterfaceClassesInsideInterfaceClass(doc, referencedClassName, classType, item);

                }
            }
        }



        public void CheckForAttributesOfReferencedClassName(InterfaceFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassName(attribute, classType);
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, classType, attribute);
                }

            }
            if (!classType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                try
                {
                    if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString()+"{"+ "Class:" + "  "  +classType.ReferencedClassName+"}"))
                    {
                        DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            

        }
        public void CkeckForNestedAttributesOfReferencedClassName(AttributeType attributeType, InterfaceFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassName(attributeinattribute, classType);
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, attributeinattribute, classType, attributeType);
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                }
            }
           
        }



        public void CheckForAttributesOfReferencedClassName(InterfaceFamilyType classTypeSearchForReferencedClassName, InterfaceFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                foreach (var attribute in classTypeSearchForReferencedClassName.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, attribute, classType);
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, classType, attribute);
                }

            }
            if (!classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
               
                list.Add(sublist);
                try
                {
                    if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                    {
                        DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public void CkeckForNestedAttributesOfReferencedClassName(InterfaceFamilyType classTypeSearchForReferencedClassName, AttributeType attributeType, InterfaceFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, attributeinattribute, classType);
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, attributeinattribute, classType, attributeType);
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

               
                list.Add(sublist);
                if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                }
            }
        }



        public void StoreEachAttributeValueInListOfReferencedClassName(List<List<ClassOfListsFromReferencefile>> list, InterfaceFamilyType classType, AttributeType attributeType)
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
            attributeparameters.ReferencedClassName = classType.ReferencedClassName;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;
            attributeparameters.ID = classType.ID;



            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void StoreEachAttributeValueInListOfReferencedClassName(List<List<ClassOfListsFromReferencefile>> list, AttributeType AttributeInAttribute, InterfaceFamilyType classType, AttributeType attributeType)
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
            attributeparameters.RefSemanticList = attributeType.RefSemantic;
            attributeparameters.ReferencedClassName = classType.ReferencedClassName;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;
            attributeparameters.ID = classType.ID;

            sublist.Add(attributeparameters);
            list.Add(sublist);
            if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
            {
                DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
            }
            else
            {
                DictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
            }

        }





        public void SearchForReferencedClassNameofExternalIterface(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType, ExternalInterfaceType externalInterface)
        {
            string referencedClassNameofReferencedClassName = "";


            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.InterfaceClassLib)
            {
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.InterfaceClass)
                {

                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {
                        CheckForAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, classType, externalInterface);
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;

                            SearchForReferencedClassNameofExternalIterface(doc, referencedClassNameofReferencedClassName, classType, externalInterface);
                        }

                    }
                    if (classTypeSearchForReferencedClassName.InterfaceClass.Exists)
                    {

                        SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(doc, referencedClassName, classType, classTypeSearchForReferencedClassName, externalInterface);

                    }


                }
            }

        }

        public void SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType,
           InterfaceFamilyType classTypeSearchForReferencedClassName, ExternalInterfaceType externalInterface)
        {
            string referencedClassNameofReferencedClassName = "";
            foreach (var item in classTypeSearchForReferencedClassName.InterfaceClass)
            {
                if (item.Name == referencedClassName)
                {
                    CheckForAttributesOfReferencedClassNameofExternalIterface(item, classType, externalInterface);
                    if (item.ReferencedClassName != "" && item.ReferencedClassName != item.Name)
                    {
                        referencedClassNameofReferencedClassName = item.ReferencedClassName;

                        SearchForReferencedClassNameofExternalIterface(doc, referencedClassNameofReferencedClassName, classType, externalInterface);
                    }

                }
                if (item.InterfaceClass.Exists)
                {
                    SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(doc, referencedClassName, classType, item, externalInterface);

                }

            }
        }



        public void CheckForAttributesOfReferencedClassNameofExternalIterface(InterfaceFamilyType classType, ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (externalInterface.Attribute.Exists)
            {
                foreach (var attribute in externalInterface.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(attribute, classType, externalInterface);
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, classType, attribute, externalInterface);
                }

            }
            if (!externalInterface.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
               
                list.Add(sublist);
                try
                {
                    if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
           

        }
        public void CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(AttributeType attributeType, InterfaceFamilyType classType, ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(attributeinattribute, classType, externalInterface);
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, attributeinattribute, classType, attributeType, externalInterface);
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

               
                list.Add(sublist);
                if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString() 
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }

            }

        }



        public void CheckForAttributesOfReferencedClassNameofExternalIterface(InterfaceFamilyType classTypeSearchForReferencedClassName, InterfaceFamilyType classType,
             ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                foreach (var attribute in classTypeSearchForReferencedClassName.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, attribute, classType, externalInterface);
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, classType, attribute, externalInterface);
                }

            }
            if (!classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
               
                list.Add(sublist);
                try
                {
                    if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public void CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(InterfaceFamilyType classTypeSearchForReferencedClassName, AttributeType attributeType, InterfaceFamilyType classType,
             ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, attributeinattribute, classType, externalInterface);
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, attributeinattribute, classType, attributeType, externalInterface);
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }
            }
        }



        public void StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(List<List<ClassOfListsFromReferencefile>> list,
            InterfaceFamilyType classType, AttributeType attributeType, ExternalInterfaceType externalInterface)
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
            attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;

            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(List<List<ClassOfListsFromReferencefile>> list,
            AttributeType AttributeInAttribute, InterfaceFamilyType classType, AttributeType attributeType, ExternalInterfaceType externalInterface)
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
            attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
            {
                DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
            }
            else
            {
                DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
            }

        }


        public void SearchForReferencedClassName(CAEXDocument doc, string referencedClassName, RoleFamilyType classType)
        {
            string referencedClassNameofReferencedClassName = "";


            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.RoleClassLib)
            {
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.RoleClass)
                {

                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {
                        if (classTypeSearchForReferencedClassName.ExternalInterface.Exists)
                        {
                            foreach (var externalInterface in classTypeSearchForReferencedClassName.ExternalInterface)
                            {
                                if (externalInterface.BaseClass != null)
                                {
                                    referencedClassName = externalInterface.BaseClass.ToString();
                                    CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalInterface);
                                    SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalInterface);
                                }
                            }
                        }


                        CheckForAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, classType);
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;

                            SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType);
                        }

                    }
                    if (classTypeSearchForReferencedClassName.RoleClass.Exists)
                    {
                        SearchForInterfaceClassesInsideInterfaceClass(doc, referencedClassName, classType, classTypeSearchForReferencedClassName);

                    }


                }
            }

        }
        public void CheckForAttributesOfReferencedClassName(RoleFamilyType classTypeSearchForReferencedClassName, RoleFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                foreach (var attribute in classTypeSearchForReferencedClassName.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, attribute, classType);
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, classType, attribute);
                }

            }
            if (!classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                try
                {
                    if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                    {
                        DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryForRoleClassInstanceAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }
        public void CkeckForNestedAttributesOfReferencedClassName(RoleFamilyType classTypeSearchForReferencedClassName, AttributeType attributeType, RoleFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, attributeinattribute, classType);
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, attributeinattribute, classType, attributeType);
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForRoleClassInstanceAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                }
            }
        }
        public void StoreEachAttributeValueInListOfReferencedClassName(List<List<ClassOfListsFromReferencefile>> list, RoleFamilyType classType, AttributeType attributeType)
        {
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = Convert.ToString(attributeType.Name);
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.Reference = attributeType.AttributePath;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForRoleClassInstanceAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void StoreEachAttributeValueInListOfReferencedClassName(List<List<ClassOfListsFromReferencefile>> list, AttributeType AttributeInAttribute, RoleFamilyType classType, AttributeType attributeType)
        {
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            // In the following parameters on right hand side "attributeType" has been changed to "AttributeInAttribute" this has been repeated to all 
            // methods of name "StoreEachAttributeValuesInList" with four parameters.
            
            attributeparameters.Name = Convert.ToString(AttributeInAttribute.Name) ;
            attributeparameters.Value = AttributeInAttribute.Value;
            attributeparameters.Default = AttributeInAttribute.DefaultValue;
            attributeparameters.Unit = AttributeInAttribute.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = AttributeInAttribute.Description;
            attributeparameters.CopyRight = AttributeInAttribute.Copyright;
            attributeparameters.Reference = AttributeInAttribute.AttributePath;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
            {
                DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
            }
            else
            {
                DictionaryForRoleClassInstanceAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
            }

        }

        public void CheckForAttributesOfReferencedClassNameofExternalIterface(RoleFamilyType classType, ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(attribute, classType, externalInterface);
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, classType, attribute, externalInterface);
                }

            }
            if (!externalInterface.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                try
                {
                    if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public void CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(AttributeType attributeType, RoleFamilyType classType, ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(attributeinattribute, classType, externalInterface);
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, attributeinattribute, classType, attributeType, externalInterface);
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }

            }
        }



        public void CheckForAttributesOfReferencedClassNameofExternalIterface(RoleFamilyType classTypeSearchForReferencedClassName, RoleFamilyType classType,
             ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                foreach (var attribute in classTypeSearchForReferencedClassName.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, attribute, classType, externalInterface);
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, classType, attribute, externalInterface);
                }

            }
            if (!classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);

                try
                {
                    if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }
        public void CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(RoleFamilyType classTypeSearchForReferencedClassName, AttributeType attributeType, RoleFamilyType classType,
             ExternalInterfaceType externalInterface)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, attributeinattribute, classType, externalInterface);
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, attributeinattribute, classType, attributeType, externalInterface);
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }
            }
        }



        public void StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(List<List<ClassOfListsFromReferencefile>> list,
            RoleFamilyType classType, AttributeType attributeType, ExternalInterfaceType externalInterface)
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
            attributeparameters.Reference = attributeType.AttributePath;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(List<List<ClassOfListsFromReferencefile>> list,
            AttributeType AttributeInAttribute, RoleFamilyType classType, AttributeType attributeType, ExternalInterfaceType externalInterface)
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
            attributeparameters.Reference = AttributeInAttribute.AttributePath;
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
            {
                DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
            }
            else
            {
                DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
            }

        }


        public void SearchForReferencedClassNameofExternalIterface(CAEXDocument doc, string referencedClassName, RoleFamilyType classType, ExternalInterfaceType externalInterface)
        {
            string referencedClassNameofReferencedClassName = "";


            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.RoleClassLib)
            {
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.RoleClass)
                {

                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {
                        CheckForAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, classType, externalInterface);
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;

                            SearchForReferencedClassNameofExternalIterface(doc, referencedClassNameofReferencedClassName, classType, externalInterface);
                        }

                    }
                    if (classTypeSearchForReferencedClassName.RoleClass.Exists)
                    {

                        SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(doc, referencedClassName, classType, classTypeSearchForReferencedClassName, externalInterface);

                    }


                }
            }

        }
        public void SearchForInterfaceClassesInsideInterfaceClass(CAEXDocument doc, string referencedClassName, RoleFamilyType classType,
          RoleFamilyType classTypeSearchForReferencedClassName)
        {
            string referencedClassNameofReferencedClassName = "";
            foreach (var item in classTypeSearchForReferencedClassName.RoleClass)
            {
                if (item.Name == referencedClassName)
                {
                    if (item.ExternalInterface.Exists)
                    {
                        foreach (var externalInterface in item.ExternalInterface)
                        {
                            if (externalInterface.BaseClass != null)
                            {
                                referencedClassName = externalInterface.BaseClass.ToString();
                                CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalInterface);
                                SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalInterface);
                            }
                        }
                    }

                    CheckForAttributesOfReferencedClassName(item, classType);
                    if (item.ReferencedClassName != "" && item.ReferencedClassName != item.Name)
                    {
                        referencedClassNameofReferencedClassName = item.ReferencedClassName;

                        SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType);
                    }

                }
                if (item.RoleClass.Exists)
                {
                    SearchForInterfaceClassesInsideInterfaceClass(doc, referencedClassName, classType, item);

                }
            }
        }
        public void SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(CAEXDocument doc, string referencedClassName, RoleFamilyType classType,
          RoleFamilyType classTypeSearchForReferencedClassName, ExternalInterfaceType externalInterface)
        {
            string referencedClassNameofReferencedClassName = "";
            foreach (var item in classTypeSearchForReferencedClassName.RoleClass)
            {
                if (item.Name == referencedClassName)
                {
                    CheckForAttributesOfReferencedClassNameofExternalIterface(item, classType, externalInterface);
                    if (item.ReferencedClassName != "" && item.ReferencedClassName != item.Name)
                    {
                        referencedClassNameofReferencedClassName = item.ReferencedClassName;

                        SearchForReferencedClassNameofExternalIterface(doc, referencedClassNameofReferencedClassName, classType, externalInterface);
                    }

                }
                if (item.RoleClass.Exists)
                {
                    SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(doc, referencedClassName, classType, item, externalInterface);

                }

            }
        }
        public void CheckForAttributesOfReferencedClassName(RoleFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassName(attribute, classType);
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, classType, attribute);
                }

            }
            if (!classType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                try
                {
                    if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                    {
                        DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                    }
                    else
                    {
                        DictionaryForRoleClassInstanceAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public void CkeckForNestedAttributesOfReferencedClassName(AttributeType attributeType, RoleFamilyType classType)
        {
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CkeckForNestedAttributesOfReferencedClassName(attributeinattribute, classType);
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, attributeinattribute, classType, attributeType);
                }

            }
            if (!attributeType.Attribute.Exists)
            {
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                else
                {
                    DictionaryForRoleClassInstanceAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                }
            }
        }


        /// <summary>
        /// This method takes arguments "TreeNode" and "RoleFamilyType" to print tree nodes in "Role Class Library TreeView " in Plugin.
        /// </summary>
        /// <param name="oParentNode"></param>
        /// <param name="classType"></param>

        public void PrintNodesRecursiveInRoleClassLib(CAEXDocument document, TreeNode oParentNode, RoleFamilyType classType, string referencedclassName)
        {

            foreach (var item in classType.RoleClass)
            {
                TreeNode newnode;
                if (item.ReferencedClassName != "")
                {
                    referencedClassName = item.ReferencedClassName;
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);
                    CheckForAttributesOfReferencedClassName(classType);
                    SearchForReferencedClassName(document, referencedClassName, classType);
                }
                else
                {
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(), 1);
                }

               
                if (item.ExternalInterface.Exists)
                {
                    foreach (var externalinterfaces in item.ExternalInterface)
                    {
                        TreeNode externalinterafcenode;
                        if (externalinterfaces.BaseClass != null && externalinterfaces.BaseClass.ToString() != externalinterfaces.Name.ToString() )
                        {
                            referencedClassName = externalinterfaces.BaseClass.ToString();
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);
                            CheckForAttributesOfReferencedClassNameofExternalIterface(item, externalinterfaces);
                            SearchForReferencedClassNameofExternalIterface(document, referencedclassName, item, externalinterfaces);
                        }
                        else
                        {
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString(), 2);
                        }


                        PrintExternalInterfaceNodes(document, externalinterafcenode, externalinterfaces, classType);
                    }
                }
                PrintNodesRecursiveInRoleClassLib(document, newnode, item, referencedclassName);
            }
        }

        /// <summary>
        /// This method Takes parameters "TreeNode" and "InterfaceFamilyType" to print tree nodes in "Interface Class Library TreeView " in Plugin.
        /// </summary>
        /// <param name="oParentNode"></param>
        /// <param name="classType"></param>
        public void PrintNodesRecursiveInInterfaceClassLib(CAEXDocument document, TreeNode oParentNode, InterfaceFamilyType classType, string referencedclassName)
        {

            foreach (var item in classType.InterfaceClass)
            {
                TreeNode newnode;
                if (item.ReferencedClassName != "")
                {
                   // DictioanryOfIDofInterfaceClassLibraryNodes.Add(item.Name.ToString(), item.ID.ToString());

                    referencedclassName = item.ReferencedClassName;
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() + "{" + "Class:" + "  " + referencedclassName + "}", 1);
                    CheckForAttributesOfReferencedClassName(item);

                    SearchForReferencedClassName(document, referencedclassName, item);
                }
                else
                {
                   // DictioanryOfIDofInterfaceClassLibraryNodes.Add(item.Name.ToString(), item.ID.ToString());

                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(), 1);
                }



                if (item.ExternalInterface.Exists)
                {
                    foreach (var externalinterfaces in item.ExternalInterface)
                    {
                        TreeNode externalinterafcenode;
                        if (externalinterfaces.BaseClass.ToString() != "")
                        {
                           // DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString() + externalinterfaces.ToString(), externalinterfaces.ID.ToString());

                            referencedclassName = externalinterfaces.BaseClass.ToString();
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString() + "{" + "Class:" + "  " + referencedclassName + "}", 2);
                            CheckForAttributesOfReferencedClassNameofExternalIterface(item, externalinterfaces);
                            SearchForReferencedClassNameofExternalIterface(document, referencedclassName, item, externalinterfaces);

                        }
                        else
                        {
                            //DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString() + externalinterfaces.ToString(), externalinterfaces.ID.ToString());

                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString(), 2);
                        }


                        PrintExternalInterfaceNodes(document,externalinterafcenode, externalinterfaces, classType);
                    }
                }

                PrintNodesRecursiveInInterfaceClassLib(document, newnode, item, referencedclassName);
            }
        }

        /// <summary>
        /// This method is called to print "External Interfaces" in both "Role class Library and Interface Class Library" in the plugin.
        /// </summary>
        /// <param name="oParentNode"></param>
        /// <param name="classType"></param>
        public void PrintExternalInterfaceNodes(CAEXDocument document,TreeNode oParentNode, ExternalInterfaceType classType, InterfaceFamilyType InterafceclassType)
        {
            if (classType.ExternalInterface.Exists)
            {
                
                foreach (var item in classType.ExternalInterface)
                {
                    TreeNode newnode;
                    if (item.BaseClass!= null)
                    {
                        referencedClassName = item.BaseClass.ToString();
                        newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);
                        CheckForAttributesOfReferencedClassNameofExternalIterface(InterafceclassType, item);
                        SearchForReferencedClassNameofExternalIterface(document, referencedClassName, InterafceclassType, item);
                    }
                    else
                    {
                        newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() , 2);
                    }
                   // DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString() + item.ToString(), item.ID.ToString());

                   

                    PrintExternalInterfaceNodes(document, newnode, item, InterafceclassType);
                }
            }

        }
        /// <summary>
        /// This method is called to print "External Interfaces" in both "Role class Library and Interface Class Library" in the plugin.
        /// </summary>
        /// <param name="oParentNode"></param>
        /// <param name="classType"></param>
        public void PrintExternalInterfaceNodes(CAEXDocument document, TreeNode oParentNode, ExternalInterfaceType classType, RoleFamilyType RoleclassType)
        {
            if (classType.ExternalInterface.Exists)
            {

                foreach (var item in classType.ExternalInterface)
                {
                    TreeNode newnode;
                    if (item.BaseClass != null)
                    {
                        referencedClassName = item.BaseClass.ToString();
                        newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);
                        CheckForAttributesOfReferencedClassNameofExternalIterface(RoleclassType, item);
                        SearchForReferencedClassNameofExternalIterface(document, referencedClassName, RoleclassType, item);
                    }
                    else
                    {
                        newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(), 2);
                    }
                    // DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString() + item.ToString(), item.ID.ToString());



                    PrintExternalInterfaceNodes(document, newnode, item, RoleclassType);
                }
            }

        }
    }

}
