using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
        /// These are the properties of this class i.e. dictionaries where all attribute values from AML file are strored and#
        /// further retrived in "Device Description Class" to edit values by user.
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
        /// This method is responsible to iterate over "Interafce Class Libraries & Interafce Classes in it", and strore attributes of "Referenced Class Name" in 
        /// the dictionary.
        /// </summary>
        /// <param name="doc">This is the "CAEXDocument, where the search has to be done "</param>
        /// <param name="referencedClassName">This is "String" variable, that stores the name of the "Referenced Class Name"</param>
        /// <param name="classType">This "InterfaceFamilyType", which is a"Ground Class".</param>

        public void SearchForReferencedClassName(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType)
        {
            //This is the "String" variable, where the "Refernced Class Name" of the "Referenced class Name" has to be stored
            string referencedClassNameofReferencedClassName = "";

            // foreach "Interface Class Lib".....
            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.InterfaceClassLib)
            {
                //Foreach "Interface Class".......
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.InterfaceClass)
                {
                    //If "referenced Class Name" is found....
                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {
                        //IF "Referenced Class Name" is having "External Interface"....
                        if (classTypeSearchForReferencedClassName.ExternalInterface.Exists)
                        {
                            //Foreach "ExternalInterface"......
                            foreach (var externalInterface in classTypeSearchForReferencedClassName.ExternalInterface)
                            {
                                //If "Referenced Class Name" is existing (here:- Base class == refernced Class Name)
                                if (externalInterface.BaseClass != null)
                                {

                                    referencedClassName = externalInterface.BaseClass.ToString();

                                    //This method check attributes of "referenced Class name"
                                    CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalInterface);
                                    //This method search for "Referenced Class" of "External Interface"
                                    SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalInterface);
                                }
                               
                            }
                        }

                        //This method check attributes of "Referenced Class Name"
                        CheckForAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, classType);
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;
                            //This method is recursion of itself...
                            SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType);
                        }

                    }
                    if (classTypeSearchForReferencedClassName.InterfaceClass.Exists)
                    {
                        //This class is responsible to search for interface classes ´nested inside Interface classes and recursion of 
                        // this interface classes.
                        SearchForInterfaceClassesInsideInterfaceClass(doc, referencedClassName, classType, classTypeSearchForReferencedClassName);

                    }

                    
                }
            }

        }


        /// <summary>
        /// This class is responsible to search for interface classes ´nested inside Interface classes and recursion of 
        /// this interface classes.
        /// </summary>
        /// <param name="doc">This is the "CAEXDocument, where the search has to be done "</param>
        /// <param name="referencedClassName">This is "String" variable, that stores the name of the "Referenced Class Name"</param>
        /// <param name="classType">This "InterfaceFamilyType", which is a"Ground Class"</param>
        /// <param name="classTypeSearchForReferencedClassName">This is "InterfaceFamilyType", where the "referenced Class Name" is presented</param>
        public void SearchForInterfaceClassesInsideInterfaceClass(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType,
           InterfaceFamilyType classTypeSearchForReferencedClassName)
        {
            //This is the "String" variable, where the "Refernced Class Name" of the "Referenced class Name" has to be stored
            string referencedClassNameofReferencedClassName = "";
            //Foreach "Interface Class" inside "Interface Class"
            foreach (var item in classTypeSearchForReferencedClassName.InterfaceClass)
            {
                //If "Refrenced Class Name" is existing...
                 if (item.Name == referencedClassName)
                {
                    //If external Interface is existing....
                    if (item.ExternalInterface.Exists)
                    {
                        foreach (var externalInterface in item.ExternalInterface)
                        {
                            if (externalInterface.BaseClass != null)
                            {
                                referencedClassName = externalInterface.BaseClass.ToString();
                                //This method is responsible to Check "attributes" of "Referenced Class Name"
                                CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalInterface);
                                //This method search for "Referenced Class" of "External Interface"
                                SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalInterface);
                            }
                            
                        }
                    }

                    //This method search for "Attributes" of "referenced Class name".
                    CheckForAttributesOfReferencedClassName(item, classType);
                    if (item.ReferencedClassName != "" && item.ReferencedClassName != item.Name)
                    {
                        referencedClassNameofReferencedClassName = item.ReferencedClassName;
                        //This method is responsible to iterate over "Interafce Class Libraries & Interafce Classes in it", and strore attributes of "Referenced Class Name" in 
                        // the dictionary.
                        SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType);
                    }

                }
                 //If "Interface Class" inside "Interface Class" is existng....
                if (item.InterfaceClass.Exists)
                {
                    //This class is responsible to search for interface classes ´nested inside Interface classes and recursion of 
                    // this interface classes.
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
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, classType, attribute);
                    CkeckForNestedAttributesOfReferencedClassName(attribute, classType);
                   
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
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, attributeinattribute, classType, attributeType);
                    CkeckForNestedAttributesOfReferencedClassName(attributeinattribute, classType);
                   
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


        /// <summary>
        /// This method search for "Attributes" of "referenced Class name".
        /// </summary>
        /// <param name="classTypeSearchForReferencedClassName">This is "InterfaceFamilyType", for which attributes has to be stored in dictionary.</param>
        /// <param name="classType">This is "InterfaceFamilyType", to which this "referenced Class name" belongs to </param>
        public void CheckForAttributesOfReferencedClassName(InterfaceFamilyType classTypeSearchForReferencedClassName, InterfaceFamilyType classType)
        {
            //Initiate new list of attributes. 
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributes are existing...
            if (classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                //Foreach attribute.....
                foreach (var attribute in classTypeSearchForReferencedClassName.Attribute)
                {
                    //This the method that stores Attribute values of "Referened Class name" of "Interface Class" in the dictionary. 
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, classType, attribute);
                    //This method is responsible to check nested attributes of "Referenced Class Name" 
                    CkeckForNestedAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, attribute, classType);
                   
                }

            }
            //If attributes exists....
            if (!classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                //Initiate new list of attribute values....
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                //Add sublist to list 
                list.Add(sublist);
                try
                {
                    //If dictioanry contains key , update the values under the key
                    if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                    {
                        DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                    }
                    //Else create the key with values...
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
        /// <summary>
        /// This method is responsible to check nested attributes of "Referenced Class Name" 
        /// </summary>
        /// <param name="classTypeSearchForReferencedClassName">This is "InterfaceFamilyType", for which attributes has to be stored in dictionary.</param>
        /// <param name="attributeType">This is "AttributeType", where the attributes values has to be stored</param>
        /// <param name="classType">This is "InterfaceFamilyType", to which this "referenced Class name" belongs to</param>
        public void CkeckForNestedAttributesOfReferencedClassName(InterfaceFamilyType classTypeSearchForReferencedClassName, AttributeType attributeType, InterfaceFamilyType classType)
        {
            //Initiate new list of attributes.
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributes are existing...
            if (attributeType.Attribute.Exists)
            {
                //Foreach attribute.....
                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    // This method store nested attributes of each attribute of "Referenced CLASS name" in the dictionary.
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, attributeinattribute, classType, attributeType);
                    //This method is recursion of it self.
                    CkeckForNestedAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, attributeinattribute, classType);
                   
                }

            }
            //If attributes exists....
            if (!attributeType.Attribute.Exists)
            {
                //Initiate new list of attribute values....
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

               //Add sublist to list 
                list.Add(sublist);
                //If dictioanry contains key , update the values under the key
                if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                //Else create the key with values...
                else
                {
                    DictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                }
            }
        }


        /// <summary>
        /// This the method that stores Attribute values of "Referened Class name" of "Interface Class" in the dictionary. 
        /// </summary>
        /// <param name="list">This is list of attribute values .</param>
        /// <param name="classType"> This is "Interafce Class", which is having "Referenced Class Name "</param>
        /// <param name="attributeType">This is "AttributeType", where the values has to be stored.</param>
        public void StoreEachAttributeValueInListOfReferencedClassName(List<List<ClassOfListsFromReferencefile>> list, InterfaceFamilyType classType, AttributeType attributeType)
        {
            // Initiate new list of attributes 
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;
            attributeparameters.DataType = attributeType.AttributeDataType;
            // attributeparameters.Semantic = attributeType.RefSemantic;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.AttributePath = attributeType.AttributePath;
            attributeparameters.RefSemanticList = attributeType.RefSemantic;
            attributeparameters.ReferencedClassName = classType.ReferencedClassName;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;
            attributeparameters.ID = classType.ID;


            //Add attrbutes to sublist
            sublist.Add(attributeparameters);
            //Ass sublit to list.
            list.Add(sublist);
            try
            {
                //If dictionary conatains key, the update values of the key...
                if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                //Else create new key with values...
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
        /// <summary>
        /// This method store nested attributes of each attribute of "Referenced CLASS name" in the dictionary.
        /// </summary>
        /// <param name="list">This is list of attribute values .</param>
        /// <param name="AttributeInAttribute">This is "AttributeType ", where the values has to be stored.</param>
        /// <param name="classType">This is "Interafce Class", which is having "Referenced Class Name "</param>
        /// <param name="attributeType">This is "AttributeType", </param>
        public void StoreEachAttributeValueInListOfReferencedClassName(List<List<ClassOfListsFromReferencefile>> list, AttributeType AttributeInAttribute, InterfaceFamilyType classType, AttributeType attributeType)
        {
            // Initiate new list of attributes 
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
            attributeparameters.RefSemanticList = attributeType.RefSemantic;
            attributeparameters.ReferencedClassName = classType.ReferencedClassName;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;
            attributeparameters.ID = classType.ID;

            //Add attrbutes to sublist
            sublist.Add(attributeparameters);
            //Ass sublit to list.
            list.Add(sublist);

            //If dictionary conatains key, the update values of the key...
            if (DictionaryForInterfaceClassInstancesAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
            {
                DictionaryForInterfaceClassInstancesAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
            }
            //Else create new key with values...
            else
            {
                DictionaryForInterfaceClassInstancesAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
            }

        }



        /// <summary>
        /// //This method search for "Referenced Class" of "External Interface"
        /// </summary>
        /// <param name="doc">This is "CAEXDocuemt" in which the search has to be done.</param>
        /// <param name="referencedClassName">This is "String variable" in which "Refrenced Class name" is stored.</param>
        /// <param name="classType">This is "InterfaceFamilyType", which is a "Ground Class"´used to generate key name in dictionary.</param>
        /// <param name="externalInterface">This is the "ExternalInterfaceType", for which refernced name attributes has to be stored.</param>

        public void SearchForReferencedClassNameofExternalIterface(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType, ExternalInterfaceType externalInterface)
        {
            //This is "String variable" in which "Refrenced Class name" is stored
            string referencedClassNameofReferencedClassName = "";

            //Foreach "InterfaceClassLib" in CAEXDocument....
            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.InterfaceClassLib)
            {
                //Foreach "Interface Class" in "InterfaceClassLib".........
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.InterfaceClass)
                {
                    //If "Refernced Class Name" is existing........
                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {
                        //This method check for "attributes" of "referenced Class name" of "External Interface"
                        CheckForAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, classType, externalInterface);
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;
                            //THis method is recursion of itself...
                            SearchForReferencedClassNameofExternalIterface(doc, referencedClassNameofReferencedClassName, classType, externalInterface);
                        }

                    }
                    if (classTypeSearchForReferencedClassName.InterfaceClass.Exists)
                    {
                        //This method search for "Interface Classes" inside "Interface classes"
                        SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(doc, referencedClassName, classType, classTypeSearchForReferencedClassName, externalInterface);

                    }


                }
            }

        }

        /// <summary>
        /// This method search for "Interface Classes" inside "Interface Classes"
        /// </summary>
        /// <param name="doc">This is "CAEXDocuemt" in which the search has to be done:</param>
        /// <param name="referencedClassName">This is "String variable" in which "Refrenced Class name" is stored</param>
        /// <param name="classType">This is "InterfaceFamilyType", which is a "Ground Class"´used to generate key name in dictionary.</param>
        /// <param name="classTypeSearchForReferencedClassName">This is "Interface Class TYPE"; where "Interfac Classes" inside has to be searched.</param>
        /// <param name="externalInterface">This is the "ExternalInterfaceType", for which refernced name attributes has to be stored.</param>
        public void SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(CAEXDocument doc, string referencedClassName, InterfaceFamilyType classType,
           InterfaceFamilyType classTypeSearchForReferencedClassName, ExternalInterfaceType externalInterface)
        {
            //This is "String variable" in which "Refrenced Class name" is stored
            string referencedClassNameofReferencedClassName = "";
            //Foreach "InterfaceClassLib" in classTypeSearchForReferencedClassName
            foreach (var item in classTypeSearchForReferencedClassName.InterfaceClass)
            {
                //If referenced Class Name is Existing ........
                if (item.Name == referencedClassName)
                {
                    //This method is responsible to check "attributes" OF "Refrenced Class Name" of "ExternalInterface"
                    CheckForAttributesOfReferencedClassNameofExternalIterface(item, classType, externalInterface);
                    if (item.ReferencedClassName != "" && item.ReferencedClassName != item.Name)
                    {
                        referencedClassNameofReferencedClassName = item.ReferencedClassName;
                        //This method search for "Referenced Class" of "External Interface"
                        SearchForReferencedClassNameofExternalIterface(doc, referencedClassNameofReferencedClassName, classType, externalInterface);
                    }

                }
                if (item.InterfaceClass.Exists)
                {
                    //This method search for "Interface Classes" inside "Interface Classes"
                    SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(doc, referencedClassName, classType, item, externalInterface);

                }

            }
        }


        /// <summary>
        /// This method is responsible to Check "attributes" of "Referenced Class Name"
        /// </summary>
        /// <param name="classType">This is "InterfaceFamilyType", which is a "Ground Class".</param>
        /// <param name="externalInterface">This is "ExternalInterfaceType", which attributes has to be stored in a dictionary</param>
        public void CheckForAttributesOfReferencedClassNameofExternalIterface(InterfaceFamilyType classType, ExternalInterfaceType externalInterface)
        {
            //Initaite the list of atrributes
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //IF "External Interface" is having "Attributes"......
            if (externalInterface.Attribute.Exists)
            {
                //Foreach "Attribute".....
                foreach (var attribute in externalInterface.Attribute)
                {
                    //This method store each "Attribute" value in the dictionary...
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, classType, attribute, externalInterface);

                    //This method check for the "Nested Attributes" inside the "Attribute" of "External Interface".
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(attribute, classType, externalInterface);
                   
                }

            }
            //IF there is no attributes......
            if (!externalInterface.Attribute.Exists)
            {
                //Initiate empty list of "Attributes values".....
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
               
                list.Add(sublist);
                try
                {
                    //IF dictionary is having the key, the update the values for the key.
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
                        //Else create the key with values.
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
        /// <summary>
        /// This method is responsible to check "Nested ATtributes" of "External Interface's Attribute "
        /// </summary>
        /// <param name="attributeType">This is "AttributeType" , for which nested attributes has to be checked.</param>
        /// <param name="classType">This is "InterfaceFamilyType, which is "Ground Class" used while creating "keyname" in dictionary</param>
        /// <param name="externalInterface">This "ExternalInterfaceType for which attributes are checked."</param>
        public void CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(AttributeType attributeType, InterfaceFamilyType classType, ExternalInterfaceType externalInterface)
        {
            //Initiate new list of attributes...
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributes are existing ....
            if (attributeType.Attribute.Exists)
            {
                //Foreach "Attribute"...
                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    //This method stores attribute values of "referenced Class's External Interface"
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, attributeinattribute, classType, attributeType, externalInterface);
                    //This method is recursion of itself.
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(attributeinattribute, classType, externalInterface);
                   
                }

            }
            //IF there is no attributes......
            if (!attributeType.Attribute.Exists)
            {
                //Initiate empty list of "Attributes values".....
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

               //Add sub list to list
                list.Add(sublist);

                //IF dictionary is having the key, the update the values for the key.
                if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString() 
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                //Else create the key with values.
                else
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }

            }

        }


        /// <summary>
        /// This method is responsible to check "attributes" OF "Refrenced Class Name" of "ExternalInterface"
        /// </summary>
        /// <param name="classTypeSearchForReferencedClassName">This is "InterfaceFamilyType", for which attributes are checked.</param>
        /// <param name="classType">This is "Interface Family Type", which is a "Ground Class" used for creating Key to the dictionary.</param>
        /// <param name="externalInterface"></param>
        public void CheckForAttributesOfReferencedClassNameofExternalIterface(InterfaceFamilyType classTypeSearchForReferencedClassName, InterfaceFamilyType classType,
             ExternalInterfaceType externalInterface)
        {
            //Initiate new list of "Attributes" values.
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //IF Attributes for "Referenced Class" is Existing....
            if (classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                //for each "Attribute"..........
                foreach (var attribute in classTypeSearchForReferencedClassName.Attribute)
                {
                    //This method store each "Attribute value" into respective dictionary
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, classType, attribute, externalInterface);
                    // This method is responsible to check the "Nested Attributes" of "Attributes" of"Interface Class's External interafce"
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, attribute, classType, externalInterface);
                   
                }

            }
            //If attributes are not existing....
            if (!classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                //Initiate new list of "Attribute " values
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                //Add sublist to list.
                list.Add(sublist);
                try
                {
                    //If dictionary contains key, the update the key.
                    if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    //Else Create new key with value.
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
        /// <summary>
        /// This method is responsible to check the "Nested Attributes" of "Attributes" of"Interface Class's External interafce"
        /// </summary>
        /// <param name="classTypeSearchForReferencedClassName">This is "InterfaceFamilyType", for which attributes are checked.</param>
        /// <param name="attributeType">This is "AttributeType", which attribtes has to be stored.</param>
        /// <param name="classType">This is "InterfceClassType", which is a "Ground Class" used for creating a key in dictionary. </param>
        /// <param name="externalInterface"></param>
        public void CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(InterfaceFamilyType classTypeSearchForReferencedClassName, AttributeType attributeType, InterfaceFamilyType classType,
             ExternalInterfaceType externalInterface)
        {
            //Initiate new list of "Attribute " values
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If "Attributes" Exist...........
            if (attributeType.Attribute.Exists)
            {
                //Foreach Attribute.........
                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    //This method stores "Attributes" of "Referenced Class's External Interface"
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, attributeinattribute, classType, attributeType, externalInterface);
                    //This method is the recursion of itself....
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, attributeinattribute, classType, externalInterface);
                    
                }

            }
            //If attributes are not existing....
            if (!attributeType.Attribute.Exists)
            {
                //Initiate new list of "Attribute " values
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
                //Add sublist to list.
                list.Add(sublist);
                //If dictionary contains key, the update the key.
                if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                else
                //Else Create new key with value.
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }
            }
        }


        /// <summary>
        /// This method store each "Attribute value" into respective dictionary
        /// </summary>
        /// <param name="list">This is the list of attribute values </param>
        /// <param name="classType">This is "InterfaceFamilyType", which is "ground class" used while creating "key name" .</param>
        /// <param name="attributeType">This is "AttributeType", whose values are going to be stored.</param>
        /// <param name="externalInterface">This "ExternalInterfaceType", to which these attributes belong to.</param>
        public void StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(List<List<ClassOfListsFromReferencefile>> list,
            InterfaceFamilyType classType, AttributeType attributeType, ExternalInterfaceType externalInterface)
        {
            //Initiate the list of "Attribiute values".
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
            attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;

            //Add attributes to sublist
            sublist.Add(attributeparameters);
            //Add Sublist to list.
            list.Add(sublist);
            try
            {
                //If the dictionary is already contains key, then add values end to the existing values.
                if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                //Else create a key to the values and store them in dictionary.
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
        /// <summary>
        /// This method stores "Attributes" of "Referenced Class's External Interface"
        /// </summary>
        /// <param name="list">This is list of "attribute values", that a"ttribute values" of "External Interfacs" has to be stored in.</param>
        /// <param name="AttributeInAttribute">This is "AttributeType, for which "Nested Attributes" has to be stored.</param>
        /// <param name="classType">This is "InterfaceFamilyType", which is "Ground Class" used while naming a key in dictioonary.</param>
        /// <param name="attributeType">This "AttributeType" is  nested attribute of parent attribute. </param>
        /// <param name="externalInterface"></param>
        public void StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(List<List<ClassOfListsFromReferencefile>> list,
            AttributeType AttributeInAttribute, InterfaceFamilyType classType, AttributeType attributeType, ExternalInterfaceType externalInterface)
        {
            //Initiate ne list of "Attribute Values"
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
            attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;

            //Add each attribute in to sublist
            sublist.Add(attributeparameters);
            //Add sublist to list.
            list.Add(sublist);

            //If the dictionary is already having the keyname, the update the key values.
            if (DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.ContainsKey(classType.Name.ToString()
                + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
            {
                DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib[classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
            }
            //Else create the key with ne values.
            else
            {
                DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib.Add(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
            }

        }

        /// <summary>
        /// This function is responsible for checking whole document for the "Referenced Class Name":
        /// Note:- This function is a recursion function, which conduct many iterations in the document.
        /// </summary>
        /// <param name="doc">This is the loaded document. This might be loaded by user or can be from the plugin as well.</param>
        /// <param name="referencedClassName">This is the "Role Class Name" in the documentm, which the function is checking for </param>
        /// <param name="classType">This is just the Class Type that we are asking this function to check in the document. i.e. (RoleFamilyType)</param>
        public void SearchForReferencedClassName(CAEXDocument doc, string referencedClassName, RoleFamilyType classType)
        {
            // There can be a "Referenced Class Náme" for the "Referenced Class Name", which we are looking for.
            // This name can be stored in this string.
            string referencedClassNameofReferencedClassName = "";

            //Searches "Role Class Libraires"
            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.RoleClassLib)
            {
                //Searches "Role Classes" inside "Role Class Libs"
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.RoleClass)
                {
                    // If loop checks for the "Refernced Class Name"
                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {
                        // If loop checks for "External Interface"
                        if (classTypeSearchForReferencedClassName.ExternalInterface.Exists)
                        {
                            // for each "external interfaces" inside  "Role class"
                            foreach (var externalInterface in classTypeSearchForReferencedClassName.ExternalInterface)
                            {
                                // Here "BaseClass" is nothing but a "Referenced Name" for "External Interface"
                                if (externalInterface.BaseClass != null)
                                {

                                    referencedClassName = externalInterface.BaseClass.ToString();

                                    // this function is responsible to search for "Referenced Class" for "ExternalInterface"
                                    SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalInterface);
                                    // This function is responsible to search for "Attributes" inside the "Referencd Class Name" of "eXTERNAL iNTERFACE"
                                    CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalInterface);
                                   
                                }
                            }
                        }

                        //This method is responsible for checking attributes under "Referenced Class Name"
                        CheckForAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, classType);
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;
                            //This function is recursion function......
                            SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType);
                        }

                    }
                    //If the referenced class is having "Role Class"
                    if (classTypeSearchForReferencedClassName.RoleClass.Exists)
                    {
                        //This mthod is responsible to Check the Role Class under "Referenced Class Name"
                        SearchForInterfaceClassesInsideInterfaceClass(doc, referencedClassName, classType, classTypeSearchForReferencedClassName);

                    }


                }
            }

        }
        /// <summary>
        /// This method is responsible for checking "Attributes" under "Referenced Class Name"
        /// </summary>
        /// <param name="classTypeSearchForReferencedClassName">this is "RoleFamilyType", which we are checking attributes for.</param>
        /// <param name="classType">This is the "Ground Class"</param>
        public void CheckForAttributesOfReferencedClassName(RoleFamilyType classTypeSearchForReferencedClassName, RoleFamilyType classType)
        {
            //Initiate new list of attributes
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributes are existing .....
            if (classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                //foreach attribute.....
                foreach (var attribute in classTypeSearchForReferencedClassName.Attribute)
                {
                    //This method stores each attribute value in the dicionary.
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, classType, attribute);
                    //This function check for nested attributes in the attributes of "Refernced Class Name"
                    CkeckForNestedAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, attribute, classType);
                    
                }

            }
            //If attributes are not existing....
            if (!classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                // Iniiate new list of attributes
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                try
                {
                    // if keyname is existing already in that dictionary, then add the value to the end og existing values 
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
        /// <summary>
        /// This method check for nested attributes under attributes of "Referenced Class Name"
        /// </summary>
        /// <param name="classTypeSearchForReferencedClassName">This is "RoleFamilyType", which we are checking attributes </param>
        /// <param name="attributeType">This is the "AttributeType", which is having "Nested Attributes"</param>
        /// <param name="classType">This id the "Ground Class"</param>
        public void CkeckForNestedAttributesOfReferencedClassName(RoleFamilyType classTypeSearchForReferencedClassName, AttributeType attributeType, RoleFamilyType classType)
        {
            //Initiate new list of attributes
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributes are existing .....
            if (attributeType.Attribute.Exists)
            {
                //foreach attribute.....
                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    //This method stores nested attributes in the respective dictionary
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, attributeinattribute, classType, attributeType);
                    //This method is recursion of itself
                    CkeckForNestedAttributesOfReferencedClassName(classTypeSearchForReferencedClassName, attributeinattribute, classType);
                    
                }

            }
            //If attributes are not existing....
            if (!attributeType.Attribute.Exists)
            {
                // Iniiate new list of attributes
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);
                // if keyname is existing already in that dictionary, then add the value to the end og existing values 
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
        /// This fuunction stores "Attributes" of "Referenced Class Name" in the dictionary "DictionaryForRoleClassInstanceAttributes"
        /// </summary>
        /// <param name="list">This is the list of attributes, which has to be initiated and stores values</param>
        /// <param name="classType">This is the "Ground Class"</param>
        /// <param name="attributeType">This is "AttributeType", Which can give access to every value in the "Attribute"</param>
        public void StoreEachAttributeValueInListOfReferencedClassName(List<List<ClassOfListsFromReferencefile>> list, RoleFamilyType classType, AttributeType attributeType)
        {
            //Initate list in to empty list.
            list = new List<List<ClassOfListsFromReferencefile>>();
            List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();
            ClassOfListsFromReferencefile attributeparameters = new ClassOfListsFromReferencefile();

          //Store every parameter value....

            attributeparameters.Name = attributeType.Name;
            attributeparameters.Value = attributeType.Value;
            attributeparameters.Default = attributeType.DefaultValue;
            attributeparameters.Unit = attributeType.Unit;

            attributeparameters.DataType = attributeType.AttributeDataType;
            attributeparameters.Description = attributeType.Description;
            attributeparameters.CopyRight = attributeType.Copyright;
            attributeparameters.AttributePath = attributeType.AttributePath;
            attributeparameters.RefSemanticList = attributeType.RefSemantic;
            attributeparameters.ReferencedClassName = classType.ReferencedClassName;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;
            attributeparameters.ID = classType.ID;
            attributeparameters.SupportesRoleClassType = classType.CAEXPath();

            //Store attributes in  Sublist 
            sublist.Add(attributeparameters);
            //Store Sublist in list.
            list.Add(sublist);
            try
            {
                // if keyname is existing already in that dictionary, then add the value to the end og existing values  
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

        /// <summary>
        /// This method is responsible to store "Nested Attributes" under "Attributes" of "Referenced Class Name"
        /// </summary>
        /// <param name="list">This is the list of attributes which has to be intantiaed as an empty list .</param>
        /// <param name="AttributeInAttribute">This is "AttributeType", which represents "nested attributes" of an "attribute"</param>
        /// <param name="classType">This is the "Ground Class"</param>
        /// <param name="attributeType">This is "AttributeType", which is the main attribute.</param>
        public void StoreEachAttributeValueInListOfReferencedClassName(List<List<ClassOfListsFromReferencefile>> list, AttributeType AttributeInAttribute, RoleFamilyType classType, AttributeType attributeType)
        {
            //Initate list in to empty list.
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
            attributeparameters.ReferencedClassName = classType.ReferencedClassName;
            attributeparameters.RefBaseClassPath = classType.RefBaseClassPath;
            attributeparameters.ID = classType.ID;
            attributeparameters.SupportesRoleClassType = classType.CAEXPath();

            //Store attributes in  Sublist 
            sublist.Add(attributeparameters);
            //Store Sublist in list.
            list.Add(sublist);

            // if keyname is existing already in that dictionary, then add the value to the end og existing values 
            if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
            {
                DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
            }
            else
            {
                DictionaryForRoleClassInstanceAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
            }

        }

        /// <summary>
        /// This Function is responsible to search attributes under the "Referenced Classs Name" i.e. in this part "RoleFamilyType"
        /// </summary>
        /// <param name="classType">This is "Ground Class we are checking attributes for." </param>
        /// <param name="externalInterface">This is "ExternalInterfaceType", for which the attributes has to be checked and stored</param>
        public void CheckForAttributesOfReferencedClassNameofExternalIterface(RoleFamilyType classType, ExternalInterfaceType externalInterface)
        {
            //Initate new attribute list.
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributes under "External Interface" are existing
            if (externalInterface.Attribute.Exists)
            {
                //For each attribute......
                foreach (var attribute in externalInterface.Attribute)
                {
                    //Store each attribute value in the dictionary using following method.
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, classType, attribute, externalInterface);

                    // This method look for nested attributes.
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(attribute, classType, externalInterface);
                   
                }

            }
            //If "External Interface" is not having attributes
            if (!externalInterface.Attribute.Exists)
            {
                //Create new list of attributes 
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                //Add sublit to list.....
                list.Add(sublist);
                try
                {
                    //If key already exists in dictionary, then add values to the already existing values.
                    if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    //Else create new key -.....
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
        /// <summary>
        /// This function is responsible for checking "nested attributes" under "attributes" of the "External Interface"
        /// </summary>
        /// <param name="attributeType">This is the "AttributeType", which is having nested attributes.</param>
        /// <param name="classType">This is the "Ground Class" we are Searching in.</param>
        /// <param name="externalInterface">This is the "ExternalInterfaceType", which is having attributes.</param>
        public void CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(AttributeType attributeType, RoleFamilyType classType, ExternalInterfaceType externalInterface)
        {
            //Initate new list of attributes
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributres are existing..........
            if (attributeType.Attribute.Exists)
            {
                // FOR EACH ATTRIBUTE.............
                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    // This method allows to store the "attribute values" in the designated "Dictionary"
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, attributeinattribute, classType, attributeType, externalInterface);
                    // This method allows tocheck for nested attribute inside attributes i.e. recursion of this own method.
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(attributeinattribute, classType, externalInterface);
                   
                }

            }
            //If attributes are not existing 
            if (!attributeType.Attribute.Exists)
            {
                //Initiate new list of attributes
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                //Ads sublist to list .
                list.Add(sublist);
                //If Dictionary already contains the key, then add values to the existing values
                if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                //Else Create new key .....
                else
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }

            }
        }


        /// <summary>
        /// This function is responsible to check and store "Attributes" under "External Interface" of "Referenced Class Name" of "Role Class"
        /// </summary>
        /// <param name="classTypeSearchForReferencedClassName">This is the "Referenced Class Name" of Role Classe's "External Interface".</param>
        /// <param name="classType">This is the "Role Class"</param>
        /// <param name="externalInterface">Thi is the "External Interface"  we are looking for.</param>
        public void CheckForAttributesOfReferencedClassNameofExternalIterface(RoleFamilyType classTypeSearchForReferencedClassName, RoleFamilyType classType,
             ExternalInterfaceType externalInterface)
        {
            // Initiate new list of attributes
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();

            //If attributes are existing 
            if (classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                //foreach loop for each "attribute"
                foreach (var attribute in classTypeSearchForReferencedClassName.Attribute)
                {
                    // This function is responsible for storing each attribute of External Interface
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, classType, attribute, externalInterface);

                    // This method looks for "nested attributes" under each "attribute"
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, attribute, classType, externalInterface);
                   
                }

            }
            //If attributes are not existing 
            if (!classTypeSearchForReferencedClassName.Attribute.Exists)
            {
                //Initiate lists as empty lists
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();


                list.Add(sublist);

                try
                {
                    // Dictionary is containing specific key already, the add attributes to the end of the existing values.
                    if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                    {
                        DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                            + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                            + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                    }
                    //Else create new keyname as specified below and add to dictionary
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

        /// <summary>
        /// This function is responsible to check "attribute" under "attributes" i.e. nested attributes
        /// This function is a recursive function, which check nested attributes untill they were ended.
        /// </summary>
        /// <param name="classTypeSearchForReferencedClassName">This if "RoleFamilyType", which we are storing attributes for </param>
        /// <param name="attributeType">This is "AttributeType ", which retrives attribute values </param>
        /// <param name="classType">This is the "Ground Class", which we are looking in</param>
        /// <param name="externalInterface">This is the "external interface", under which the attributes has to be stored. </param>
        public void CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(RoleFamilyType classTypeSearchForReferencedClassName, AttributeType attributeType, RoleFamilyType classType,
             ExternalInterfaceType externalInterface)
        {
            //Initiate new attribute list.
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();

            // if attributes are existing as nested attributes 
            if (attributeType.Attribute.Exists)
            {

                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    // Using this method "attributes" are stored in the respective dictionary
                    StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(attributelist, attributeinattribute, classType, attributeType, externalInterface);

                    //Do recursion of this method untill the nested attributes were ended
                    CkeckForNestedAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, attributeinattribute, classType, externalInterface);
                   
                }

            }
            //If attributes are not existing
            if (!attributeType.Attribute.Exists)
            {
                //Initiate new list of attributes
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                //Add sublist to list.
                list.Add(sublist);

                //If specified keyname is already existing in the dioctionary, then add them to the end of the list of values under specified key
                if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                //Else create nes keyname with below specified syntax.
                else
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
                }
            }
        }


        /// <summary>
        /// This method is responsible to store each attribute value of "Referenced Class Name" of "External Interface" of Role Class.
        /// This method stores each value in attribute into a dictionary (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib)
        /// </summary>
        /// <param name="list">This is list of lists that can hold attribute values</param>
        /// <param name="classType">RoleFamilyType is the "ground Class" We are storing this "Attributes" for</param>
        /// <param name="attributeType">This is each attribute</param>
        /// <param name="externalInterface">This is "external interface" we are storing "Attributes" for</param>
        public void StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(List<List<ClassOfListsFromReferencefile>> list,
            RoleFamilyType classType, AttributeType attributeType, ExternalInterfaceType externalInterface)
        {
            //Initiate list as new list.
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
            attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;
            attributeparameters.SupportesRoleClassType = externalInterface.CAEXPath();

            //Add each parameter to sublist 
            sublist.Add(attributeparameters);
            //Add sublist to main list.
            list.Add(sublist);
            try
            {
                // If the dictionary is already containing the key with specific syntax the add to the already existing attributes.
                if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
                {
                    DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                        + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                        + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
                }
                // else normally add "attribute ´values" withe the "specific key syntax" as stated below
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
        /// <summary>
        /// This method stores the nested attributes in "DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib"
        /// </summary>
        /// <param name="list">This the list of attributes </param>
        /// <param name="AttributeInAttribute">This is "AttributeType" i.e. nested attribute </param>
        /// <param name="classType">This is the "RoleFamilyType" used to define the "keyname" of the dictionary </param>
        /// <param name="attributeType">This is "AttributeType" under which "Nested Attributes" are present</param>
        /// <param name="externalInterface">This is the "ExternalInterfaceType" used to define the "keyname" of the dictionary and also the attribute holder "External Interface"</param>
        public void StoreEachAttributeValueInListOfReferencedClassNameofExternalIterface(List<List<ClassOfListsFromReferencefile>> list,
            AttributeType AttributeInAttribute, RoleFamilyType classType, AttributeType attributeType, ExternalInterfaceType externalInterface)
        {
            //Initate new list of attributes.
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
            attributeparameters.ReferencedClassName = externalInterface.BaseClass.ToString();
            attributeparameters.RefBaseClassPath = externalInterface.RefBaseClassPath;
            attributeparameters.ID = externalInterface.ID;
            attributeparameters.SupportesRoleClassType = externalInterface.CAEXPath();

            //Add "Each Attribute Values" to sub list
            sublist.Add(attributeparameters);
            //Add sublist to main list.
            list.Add(sublist);
            // If Dictionary is already having key withe specified key name syntax, then add to the end of the list.
            if (DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.ContainsKey(classType.Name.ToString()
                + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"))
            {
                DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib[classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}"].AddRange(list);
            }
            //Else add new keyname to the dictionary and store the values
            else
            {
                DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib.Add(classType.Name.ToString()
                    + "{" + "Class:" + "  " + classType.ReferencedClassName + "}" + externalInterface.Name.ToString()
                    + "{" + "Class:" + "  " + externalInterface.BaseClass + "}", list);
            }

        }


        /// <summary>
        /// This method is responsible to check for "Referenced Class Name" of "External Interfaces" under the "Role Class"
        /// This method is a recursive method, that looks for the "Referenced Class Name" of the "Extenal Interface" under "Role Class"
        /// </summary>
        /// <param name="doc">This is the document loaded from the plugin or loaded by user from his local machine.</param>
        /// <param name="referencedClassName">This is the string that we are looking for inside the document.</param>
        /// <param name="classType">This is the Class Type i.e. "RoleFamliyType" i.e. under "Role Classes" we are looking for.</param>
        /// <param name="externalInterface">Don't forget we are looking for thev"ExtenalInterface"  </param>
        public void SearchForReferencedClassNameofExternalIterface(CAEXDocument doc, string referencedClassName, RoleFamilyType classType, ExternalInterfaceType externalInterface)
        {
            //String that store "referenced clas name" of the "referenced class name"
            string referencedClassNameofReferencedClassName = "";

            // Search for Role Class Lib in the document
            foreach (var classLibTypeSearchForReferencedClassName in doc.CAEXFile.RoleClassLib)
            {
                // Search for the "Role Class"
                foreach (var classTypeSearchForReferencedClassName in classLibTypeSearchForReferencedClassName.RoleClass)
                {
                    // If "referenced class name" we are looking for is found 
                    if (classTypeSearchForReferencedClassName.Name == referencedClassName)
                    {
                        // This method Checks "Attributes" inside the "External Interface and store in the dictionary"
                        CheckForAttributesOfReferencedClassNameofExternalIterface(classTypeSearchForReferencedClassName, classType, externalInterface);

                        //If the referenced name is not equal to null, and not equal to the name we found, the start recursion of this method itself....
                        //....untill we found it.
                        if (classTypeSearchForReferencedClassName.ReferencedClassName != "" && classTypeSearchForReferencedClassName.ReferencedClassName != classTypeSearchForReferencedClassName.Name)
                        {
                            referencedClassNameofReferencedClassName = classTypeSearchForReferencedClassName.ReferencedClassName;

                            //Recursion of this method itself start here.
                            SearchForReferencedClassNameofExternalIterface(doc, referencedClassNameofReferencedClassName, classType, externalInterface);
                        }

                    }
                    //If there are "Role Classes", under "Referenced Class Name"
                    if (classTypeSearchForReferencedClassName.RoleClass.Exists)
                    {
                        //This function ignites new search in "Role Classes" under "Referenced Class Name"
                        // CAUTION:- Though the function Name States Interface Class, It serves for "Role Class" in this part.
                        //CAUTION:- This function naming has to be changed.
                        SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(doc, referencedClassName, classType, classTypeSearchForReferencedClassName, externalInterface);

                    }


                }
            }

        }

        /// <summary>
        /// This method searches "Role Classes" under "Referenced Class Name" 
        /// </summary>
        /// <param name="doc">This is the document, where we are searching for "Referenced Class Name" </param>
        /// <param name="referencedClassName">This is the "Referenced Class Name" i.e. "String" used to search for the name of thE  "Referenced Class Name"</param>
        /// <param name="classType">This is "RoleFamilyType" i.e. "Ground Class"</param>
        /// <param name="classTypeSearchForReferencedClassName">This is "RoleFamilyType", under which the role clasees are searched.</param>
        public void SearchForInterfaceClassesInsideInterfaceClass(CAEXDocument doc, string referencedClassName, RoleFamilyType classType,
          RoleFamilyType classTypeSearchForReferencedClassName)
        {
            //This is a "String" variable, where the "Referenced class Name" is stroed...
            string referencedClassNameofReferencedClassName = "";
            //Foreach "role class" in the "Referenced Class name"
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
                                //This method is responsible to check for "Referenced Class Name" of "External Interfaces" under the "Role Class" 
                                SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalInterface);
                                //This Function is responsible to search attributes under the "Referenced Classs Name" i.e. in this part "RoleFamilyType"
                                CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalInterface);
                                
                            }
                        }
                    }

                    //This method is responsible for checking "Attributes" under "Referenced Class Name"
                    CheckForAttributesOfReferencedClassName(item, classType);
                    //If referenced Class Name is not null ......
                    if (item.ReferencedClassName != "" && item.ReferencedClassName != item.Name)
                    {
                        referencedClassNameofReferencedClassName = item.ReferencedClassName;
                        //This function is responsible for checking whole document for the "Referenced Class Name":
                        SearchForReferencedClassName(doc, referencedClassNameofReferencedClassName, classType);
                    }

                }
                //If "Role Class" exists
                if (item.RoleClass.Exists)
                {
                    //This method is recursion of itself
                    SearchForInterfaceClassesInsideInterfaceClass(doc, referencedClassName, classType, item);

                }
            }
        }

        /// <summary>
        /// This method is a recursive method, which checks for "Refernced Class Name" under role class under role class
        /// 
        /// </summary>
        /// <param name="doc">This is the document or file, where the search has to be done</param>
        /// <param name="referencedClassName">This is the "Referenced ClassName", that we are looking i the document.</param>
        /// <param name="classType">This is the "Ground Class" we stared search at.</param>
        /// <param name="classTypeSearchForReferencedClassName"> This is "RoleFamilType" to search, if "Role Classes" are present. </param>
        /// <param name="externalInterface"></param>
        public void SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(CAEXDocument doc, string referencedClassName, RoleFamilyType classType,
          RoleFamilyType classTypeSearchForReferencedClassName, ExternalInterfaceType externalInterface)
        {
            // This string represents the "Refernced Class Name" of the "Referenced Class Name"
            string referencedClassNameofReferencedClassName = "";

            //For each "Role Class" existing under the "Role Class".
            foreach (var item in classTypeSearchForReferencedClassName.RoleClass)
            {

                if (item.Name == referencedClassName)
                {
                    // This method check for attributes under "Referenced name"
                    CheckForAttributesOfReferencedClassNameofExternalIterface(item, classType, externalInterface);

                    //If the referenced name is not equal to null, the search for "Referenced Class Name"
                    if (item.ReferencedClassName != "" && item.ReferencedClassName != item.Name)
                    {
                        referencedClassNameofReferencedClassName = item.ReferencedClassName;
                        //This method search for the "Refernced Class Name"
                        SearchForReferencedClassNameofExternalIterface(doc, referencedClassNameofReferencedClassName, classType, externalInterface);
                    }

                }
                //IF this "Role Class" is having further "Role Classes" the do recursion
                if (item.RoleClass.Exists)
                {
                    SearchForInterfaceClassesInsideInterfaceClassofExternalIterface(doc, referencedClassName, classType, item, externalInterface);

                }

            }
        }
        /// <summary>
        /// This method is responsible for checking attributes under "Referenced Class Name"
        /// </summary>
        /// <param name="classType">This is "RoleFamilyType", which is "GroudClass"</param>
        public void CheckForAttributesOfReferencedClassName(RoleFamilyType classType)
        {
            //Initiate New list of attributes.
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributes are existing ...
            if (classType.Attribute.Exists)
            {
                //for each attribute .....
                foreach (var attribute in classType.Attribute)
                {
                    //This fuunction stores "Attributes" of "Referenced Class Name" in the dictionary "DictionaryForRoleClassInstanceAttributes"
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, classType, attribute);
                    //This function check for nested attributes of referenced Class Name.
                    CkeckForNestedAttributesOfReferencedClassName(attribute, classType);
                   
                }

            }
            //If attributes are not existing...
            if (!classType.Attribute.Exists)
            {
                //Initaitae new list of attributes...
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);

                try
                {
                    //If Dictionary contains the key , then update the values of the key.
                    if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                    {
                        DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                    }
                    //Else create new key and add values to it.
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
        /// <summary>
        /// This method check for "nested attributes" of "referenced class name"
        /// </summary>
        /// <param name="attributeType">This is "AttributeType", for which nest attributes has to be checked.</param>
        /// <param name="classType">This "RoleFamilyType", which is "Ground class"</param>
        public void CkeckForNestedAttributesOfReferencedClassName(AttributeType attributeType, RoleFamilyType classType)
        {
            //Initiate new list of attributes
            List<List<ClassOfListsFromReferencefile>> attributelist = new List<List<ClassOfListsFromReferencefile>>();
            //If attributes are existing.....
            if (attributeType.Attribute.Exists)
            {
                ///Foreach attribute.....
                foreach (var attributeinattribute in attributeType.Attribute)
                {
                    // This method is responsible to store "Nested Attributes" under "Attributes" of "Referenced Class Name"
                    StoreEachAttributeValueInListOfReferencedClassName(attributelist, attributeinattribute, classType, attributeType);
                    //This method is recursion of itself...
                    CkeckForNestedAttributesOfReferencedClassName(attributeinattribute, classType);
                   
                }

            }
            //If attributes are not existing...
            if (!attributeType.Attribute.Exists)
            {
                //Initiate new list of attributes 
                List<List<ClassOfListsFromReferencefile>> list = new List<List<ClassOfListsFromReferencefile>>();
                List<ClassOfListsFromReferencefile> sublist = new List<ClassOfListsFromReferencefile>();

                list.Add(sublist);
                //If Dictionary contains key the update key values
                if (DictionaryForRoleClassInstanceAttributes.ContainsKey(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"))
                {
                    DictionaryForRoleClassInstanceAttributes[classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}"].AddRange(list);
                }
                //Else create new key with the list of attributes.
                else
                {
                    DictionaryForRoleClassInstanceAttributes.Add(classType.Name.ToString() + "{" + "Class:" + "  " + classType.ReferencedClassName + "}", list);
                }
            }
        }


        /// <summary>
        /// This method takes arguments "TreeNode" and "RoleFamilyType" to print tree nodes in "Role Class Library TreeView " in Plugin.
        /// </summary>
        /// <param name="oParentNode">This is a "TreeNode", which is parent node for the new node that is going to be created using this method. </param>
        /// <param name="classType">This is "RoleFamilyType", which is a "Ground Class"</param>

        public void PrintNodesRecursiveInRoleClassLib(CAEXDocument document, TreeNode oParentNode, RoleFamilyType classType, string referencedclassName)
        {

            foreach (var item in classType.RoleClass)
            {
                TreeNode newnode;
                if (item.ReferencedClassName != "")
                {
                    referencedClassName = item.ReferencedClassName;
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);
                    //This function is responsible for checking whole document for the "Referenced Class Name":
                    SearchForReferencedClassName(document, referencedClassName, classType);

                    //This method is responsible for checking attributes under "Referenced Class Name"
                    CheckForAttributesOfReferencedClassName(classType);
                    
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
                            externalinterafcenode.ForeColor = SystemColors.GrayText;

                            //This method is responsible to check for "Referenced Class Name" of "External Interfaces" under the "Role Class"
                            SearchForReferencedClassNameofExternalIterface(document, referencedclassName, item, externalinterfaces);

                            //This Function is responsible to search attributes under the "Referenced Classs Name" i.e. in this part "RoleFamilyType"
                            CheckForAttributesOfReferencedClassNameofExternalIterface(item, externalinterfaces);
                           
                        }
                        else
                        {
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString(), 2);
                            externalinterafcenode.ForeColor = SystemColors.GrayText;
                        }

                        //This method is called to print "External Interfaces" in both "Role class Library and Interface Class Library" in the plugin.
                        PrintExternalInterfaceNodes(document, externalinterafcenode, externalinterfaces, classType);
                    }
                }
                //This method is recursion of itself.
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
                   
                    referencedclassName = item.ReferencedClassName;
                    newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() + "{" + "Class:" + "  " + referencedclassName + "}", 1);
                    CheckForAttributesOfReferencedClassName(item);

                    SearchForReferencedClassName(document, referencedclassName, item);
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
                        if (externalinterfaces.BaseClass!= null)
                        {
                           
                            referencedclassName = externalinterfaces.BaseClass.ToString();
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString() + "{" + "Class:" + "  " + referencedclassName + "}", 2);
                            externalinterafcenode.ForeColor = SystemColors.GrayText;
                            CheckForAttributesOfReferencedClassNameofExternalIterface(item, externalinterfaces);
                            SearchForReferencedClassNameofExternalIterface(document, referencedclassName, item, externalinterfaces);

                        }
                        else
                        {
                            externalinterafcenode = newnode.Nodes.Add(externalinterfaces.ToString(), externalinterfaces.ToString(), 2);
                            externalinterafcenode.ForeColor = SystemColors.GrayText;
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
                        newnode.ForeColor = SystemColors.GrayText;
                        CheckForAttributesOfReferencedClassNameofExternalIterface(InterafceclassType, item);
                        SearchForReferencedClassNameofExternalIterface(document, referencedClassName, InterafceclassType, item);
                    }
                    else
                    {
                        newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString() , 2);
                        newnode.ForeColor = SystemColors.GrayText;
                    }
                  

                    PrintExternalInterfaceNodes(document, newnode, item, InterafceclassType);
                }
            }

        }
        /// <summary>
        /// This method is called to print "External Interfaces" in both "Role class Library and Interface Class Library" in the plugin.
        /// </summary>
        /// <param name="oParentNode">This is "TreeNode", which is parent node for the new node that is going to be created using this method. </param>
        /// <param name="classType">This is "RoleFamilyType", which is a "Ground Class"</param>
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
                        newnode.ForeColor = SystemColors.GrayText;

                        //This Function is responsible to search attributes under the "Referenced Classs Name" i.e. in this part "RoleFamilyType"
                        CheckForAttributesOfReferencedClassNameofExternalIterface(RoleclassType, item);

                        //This method is responsible to check for "Referenced Class Name" of "External Interfaces" under the "Role Class"
                        SearchForReferencedClassNameofExternalIterface(document, referencedClassName, RoleclassType, item);
                    }
                    else
                    {
                        newnode = oParentNode.Nodes.Add(item.ToString(), item.ToString(), 2);
                        newnode.ForeColor = SystemColors.GrayText;
                       
                    }
                   
                    //This is a recursion of this method itself...
                    PrintExternalInterfaceNodes(document, newnode, item, RoleclassType);
                }
            }

        }
    }

}
