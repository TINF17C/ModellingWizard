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
        public Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofInterfaceClassattributes { get; set; }
        public Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofRoleClassattributes { get; set; }
        public Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofExternalInterfaceattributes { get; set; }
        public Dictionary<string,List<List<ClassOfListsFromReferencefile>>> dictionaryForInterfaceClassInstancesAttributes { get; set; }

        public string  referencedClassName { get; set; }

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
       
        public void SearchForReferencedClassName(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType/*InterfaceClassLibType classLibType*/)
        {
            if (classType.Attribute.Exists)
            {
                CheckForAttributesOfReferencedClassName(classType);
            }
            else
            {

            }
            
            string referencedClassNameofReferencedClassName = "";
            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.InterfaceClassLib)
            {
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.InterfaceClass)
                {

                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {

                        CheckForAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, classType);
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;
                            SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType/*, classLibType*/);
                        }
                    }

                }
            }
        }

        public void SearchForReferencedClassNameWithoutAttributes()
        {

        }

        public void CheckForExternalInterfacesUnderInterfaceClass()
        {

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
            attributeparameters.Reference = attributeType.AttributePath;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            try
            {
                if (dictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString()))
                {
                    dictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString()].AddRange(list);
                }
                else
                {
                    dictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString(), list);
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
            attributeparameters.Reference = AttributeInAttribute.AttributePath;


            sublist.Add(attributeparameters);
            list.Add(sublist);
            if (dictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString()))
            {
                dictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString()].AddRange(list);
            }
            else
            {
                dictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString(), list);
            }
            // Limitation, attributes with identical names in one class type cannot be added.
        }



        /// <summary>
        /// This method takes arguments "TreeNode" and "RoleFamilyType" to print tree nodes in "Role Class Library TreeView " in Plugin.
        /// </summary>
        /// <param name="oParentNode"></param>
        /// <param name="classType"></param>

        public void PrintNodesRecursiveInRoleClassLib(TreeNode oParentNode, RoleFamilyType classType)
        {
            
            foreach (var item in classType.RoleClass)
            {
                TreeNode newnode;
                if (item.ReferencedClassName != "")
                {
                    referencedClassName = item.ReferencedClassName;
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);
                }
                else
                {
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() , 1);
                }
               
                CheckForAttributes(item);
                if (item.ExternalInterface.Exists)
                {
                    foreach (var externalinterfaces in item.ExternalInterface)
                    {
                        TreeNode externalinterafcenode;
                        if (externalinterfaces.BaseClass.ToString() != "")
                        {
                             referencedClassName = externalinterfaces.BaseClass.ToString();
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);
                        }
                        else
                        {
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString(), 2);
                        }

                       
                        PrintExternalInterfaceNodes(externalinterafcenode, externalinterfaces);
                    }
                }
                PrintNodesRecursiveInRoleClassLib(newnode, item);
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
                    referencedClassName = item.ReferencedClassName;
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() + "{" + "Class:" +"  "+ referencedClassName + "}", 1);
                    /*CheckForAttributesOfReferencedClassName(item);*/
                    SearchForReferencedClassName(document, referencedclassName, item);
                }
                else
                {
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(), 1);
                }
               
                CheckForAttributes(item);
                if (item.ExternalInterface.Exists)
                {
                    foreach (var externalinterfaces in item.ExternalInterface)
                    {
                        TreeNode externalinterafcenode;
                        if (externalinterfaces.BaseClass.ToString() != "")
                        {
                             referencedClassName = externalinterfaces.BaseClass.ToString();
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString()+"{" + "Class:" + "  " + referencedClassName + "}", 2);
                        }
                        else
                        {
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString(), 2);
                        }
                        CheckForAttributes(externalinterfaces);
                        
                        PrintExternalInterfaceNodes(externalinterafcenode, externalinterfaces);
                    }
                }
              
                PrintNodesRecursiveInInterfaceClassLib(document,newnode, item, referencedclassName);
            }
        }

        /// <summary>
        /// This method is called to print "External Interfaces" in both "Role class Library and Interface Class Library" in the plugin.
        /// </summary>
        /// <param name="oParentNode"></param>
        /// <param name="classType"></param>
        public void PrintExternalInterfaceNodes(TreeNode oParentNode, ExternalInterfaceType classType)
        {
            if (classType.ExternalInterface.Exists)
            {
                foreach (var item in classType.ExternalInterface)
                {
                    CheckForAttributes(item);
                    TreeNode newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(), 2);

                    PrintExternalInterfaceNodes(newnode, item);
                }
            }

        }


        /// <summary>
        /// Atrributes checker is used to retrive each attributes and store them in a dictionary with classname+parentattributename+attributename 
        /// as a key for the individual list of parameters in an attribute. below classes are responsible to check for attributes
        /// in Interface classes and their individual attributes.
        /// </summary>
        /// <param name="classType"></param>
        

        public void CheckForAttributes(InterfaceFamilyType classType)
        {
            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attribute);
                    StoreEachAttributeValuesInList(attributelist, classType, attribute);
                }

            }

        }
        /// <summary>
        /// This check for Nested Atrributes of Attributes "InterfaceFamilyType".
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="attributeType"></param>
        public void CheckForNestedAttributeinsideAttribute(InterfaceFamilyType classType, AttributeType attributeType)
        {
            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attributeinattribute);
                    StoreEachAttributeValuesInList(attributelist, attributeinattribute, classType, attributeType);
                }

            }
        }
       /// <summary>
       /// This method is connected with the above method that check for attributes in "InterafceFamilyType", this method store individual attribute value to list 
       /// and store in a dictionary "dictionaryofInterfaceClassattributes" using a key
       /// </summary>
       /// <param name="list"></param>
       /// <param name="classType"></param>
       /// <param name="attributeType"></param>
        public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> list, InterfaceFamilyType classType, AttributeType attributeType)
        {
            list = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.Reference = attributeType.AttributePath;

            list.Add(attributeparameters);
           dictionaryofInterfaceClassattributes.Add(classType.Name.ToString() + attributeType.Name.ToString(), list);
            // Limitation, attributes with identical names in one class type cannot be added.

        }
        /// <summary>
        /// This method is linked with the above attribute checking method but holds more paramters than this previous method 
        /// because this method stores each nested  attribute values in to the respective dictionary.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="AttributeInAttribute"></param>
        /// <param name="classType"></param>
        /// <param name="attributeType"></param>
        public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> list, AttributeType AttributeInAttribute, InterfaceFamilyType classType, AttributeType attributeType)
        {
            list = new List<ClassOfListsFromReferencefile>();
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


            list.Add(attributeparameters);
            dictionaryofInterfaceClassattributes.Add(classType.Name.ToString() + attributeType.Name.ToString() + AttributeInAttribute.Name.ToString(), list);
            // Limitation, attributes with identical names in one class type cannot be added.

        }


        /// <summary>
        /// Atrributes checker is used to retrive each attributes and store them in a dictionary with classname+parentattributename+attributename
        /// as a key for the individual list of parameters in an attribute. below classes are responsible to check for attributes 
        /// in Role classes and their individual attributes.
        /// </summary>
        /// <param name="classType"></param>
       
        public void CheckForAttributes(RoleFamilyType classType)
        {

            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attribute);
                    StoreEachAttributeValuesInList(attributelist, classType, attribute);
                }
            }
        }

        /// <summary>
        /// This check for Nested Atrributes of Attributes in "RoleClassFamilyType".
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="attributeType"></param>
        public void CheckForNestedAttributeinsideAttribute(RoleFamilyType classType, AttributeType attributeType)
        {
            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attributeinattribute);
                    StoreEachAttributeValuesInList(attributelist, attributeinattribute, classType, attributeType);
                }

            }
        }
        /// <summary>
        /// This method is connected with the above method that check for attributes in "RoleFamilyType", this method store individual attribute value to list 
        /// and store in a dictionary "dictionaryofInterfaceClassattributes" using a key
        /// </summary>
        /// <param name="list"></param>
        /// <param name="classType"></param>
        /// <param name="attributeType"></param>
        public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> list, AttributeType AttributeInAttribute, RoleFamilyType classType, AttributeType attributeType)
        {
            list = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = AttributeInAttribute.Name;
            attributeparameters.Value = AttributeInAttribute.Value;
            attributeparameters.Default = AttributeInAttribute.DefaultValue;
            attributeparameters.Unit = AttributeInAttribute.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = AttributeInAttribute.Description;
            attributeparameters.CopyRight = AttributeInAttribute.Copyright;
            attributeparameters.Reference = AttributeInAttribute.AttributePath;

            list.Add(attributeparameters);
            dictionaryofRoleClassattributes.Add(classType.Name.ToString() + attributeType.Name.ToString() + AttributeInAttribute.Name.ToString(), list);
            // Limitation, attributes with identical names in one class type cannot be added.

        }

        /// <summary>
        /// This method is linked with the above attribute checking method but holds more paramters than this previous method 
        /// because this method stores each nested  attribute values in to the respective dictionary.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="AttributeInAttribute"></param>
        /// <param name="classType"></param>
        /// <param name="attributeType"></param>
        public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> list, RoleFamilyType classType, AttributeType attributeType)
        {
            list = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            //attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.Reference = attributeType.AttributePath;

            list.Add(attributeparameters);
            dictionaryofRoleClassattributes.Add(classType.Name.ToString() + attributeType.Name.ToString(), list);
            // Limitation, attributes with identical names in one class type cannot be added.

        }

        /// <summary>
        /// Atrributes checker is used to retrive each attributes and store them in a dictionary with classname+parentattributename+attributename
        /// as a key for the individual list of parameters in an attribute.  below classes are responsible to check for attributes 
        /// in ExternalInterfaces and their individual attributes.
        /// </summary>
        /// <param name="classType"></param>
        
        public void CheckForAttributes(ExternalInterfaceType classType)
        {
            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (classType.Attribute.Exists)
            {
                foreach (var attribute in classType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attribute);
                    StoreEachAttributeValuesInList(attributelist, classType, attribute);
                }
            }
        }
        /// <summary>
        /// This check for Nested Atrributes of Attributes in "ExternalInterfaceType".
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="attributeType"></param>
        public void CheckForNestedAttributeinsideAttribute(ExternalInterfaceType classType, AttributeType attributeType)
        {
            List<ClassOfListsFromReferencefile> attributelist = new List<ClassOfListsFromReferencefile>();
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    CheckForNestedAttributeinsideAttribute(classType, attributeinattribute);
                    StoreEachAttributeValuesInList(attributelist, attributeinattribute, classType, attributeType);
                }

            }
        }
        /// <summary>
        /// This method is connected with the above method that check for attributes in "ExternalInterfaceType", this method store individual attribute value to list 
        /// and store in a dictionary "dictionaryofExternalInterfaceattributes" using a key
        /// </summary>
        /// <param name="list"></param>
        /// <param name="classType"></param>
        /// <param name="attributeType"></param>
        public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> list, AttributeType AttributeInAttribute, ExternalInterfaceType classType, AttributeType attributeType)
        {
            list = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = AttributeInAttribute.Name;
            attributeparameters.Value = AttributeInAttribute.Value;
            attributeparameters.Default = AttributeInAttribute.DefaultValue;
            attributeparameters.Unit = AttributeInAttribute.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = AttributeInAttribute.Description;
            attributeparameters.CopyRight = AttributeInAttribute.Copyright;
            attributeparameters.Reference = AttributeInAttribute.AttributePath;

            list.Add(attributeparameters);
            dictionaryofExternalInterfaceattributes.Add(classType.CAEXParent.ToString() + classType.Name.ToString() + attributeType.Name.ToString() + AttributeInAttribute.Name.ToString(), list);
            // Limitation, attributes with identical names in one class type cannot be added.

        }

        /// <summary>
        /// This method is linked with the above attribute checking method but holds more paramters than this previous method 
        /// because this method stores each nested  attribute values in to the respective dictionary.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="AttributeInAttribute"></param>
        /// <param name="classType"></param>
        /// <param name="attributeType"></param>
        public void StoreEachAttributeValuesInList(List<ClassOfListsFromReferencefile> list, ExternalInterfaceType classType, AttributeType attributeType)
        {
            list = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.Reference = attributeType.AttributePath;

            list.Add(attributeparameters);
            dictionaryofExternalInterfaceattributes.Add(classType.CAEXParent.ToString() + classType.Name.ToString() + attributeType.Name.ToString(), list);
            // Limitation, attributes with identical names in one class type cannot be added.

        }
      
    }
}
