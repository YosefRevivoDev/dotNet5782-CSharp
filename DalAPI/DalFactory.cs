﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace DalAPI
{
    /// <summary>
    /// Static Factory class for creating Dal tier implementation object according to
    /// configuration in file config.xml
    /// </summary>
    public static class DalFactory
    {
        /// <summary>
        /// The function creates Dal tier implementation object according to Dal type
        /// as appears in "dal" element in the configuration file config.xml.<br/>
        /// The configuration file also includes element "dal-packages" with list
        /// of available packages (.dll files) per Dal type.<br/>
        /// Each Dal package must use "Dal" namespace and it must include internal access
        /// singleton class with the same name as package's name.<br/>
        /// The singleton class must include public static property called "Instance"
        /// which must contain the single instance of the class.
        /// </summary>
        /// <returns>Dal tier implementation object</returns>
        public static IDal GetDal()
        {
            // get dal implementation name from config.xml according to <data> element
            string DalType = DalConfig.DaLName;
            // bring package name (dll file name) for the dal name (above) from the list of packages in config.xml
            DalConfig.DalPackage dalPackage;
            try // get dal package info according to <dal> element value in config file
            {
                dalPackage = DalConfig.DaLPackages[DalType];
            }
            catch (KeyNotFoundException ex)
            {
                // if package name is not found in the list - there is a problem in config.xml
                throw new DALConfigException($"Wrong Dal Type: {DalType}", ex);
            }

            string dalPackageName = dalPackage.PkgName;
            string dalNameSpace = dalPackage.NameSpace;
            string dalClass = dalPackage.ClassName;

            try // Load into CLR the dal implementation assembly according to dll file name (taken above)
            {
                Assembly.Load(dalPackageName);
            }
            catch (Exception ex)
            {
                throw new DALConfigException($"Failed loading {dalPackageName}.dll", ex);
            }

            // Get concrete Dal implementation's class metadata object
            // 1st element in the list inside the string is full class name:
            //    namespace = "Dal" or as specified in the "namespace" attribute in the config file,
            //    class name = package name or as specified in the "class" attribute in the config file
            //    the last requirement (class name = package name) is not mandatory in general - but this is the way it
            //    is configured per the implementation here, otherwise we'd need to add class name in addition to package
            //    name in the config.xml file - which is clearly a good option.
            //    NB: the class may not be public - it will still be found... Our approach that the implemntation class
            //        should hold "internal" access permission (which is actually the default access permission)
            // 2nd element is the package name = assembly name (as above)
            Type type;

            try
            {
                type = Type.GetType($"{dalNameSpace}.{dalClass}.{dalPackageName}");

            }
            catch (Exception ex) // If the type is not found - the implementation is not correct - it looks like the class name is wrong...
            {
                throw new DALConfigException($"Class not found due to a wrong namespace or/and class name:" +
                    $" {dalPackageName}:{dalNameSpace}.{dalClass}", ex);
            }

            // *** Get concrete Dal implementation's Instance
            // Get property info for public static property named "Instance" (in the dal implementation class- taken above)
            // If the property is not found or it's not public or not static then it is not properly implemented
            // as a Singleton...
            // Get the value of the property Instance (get function is automatically called by the system)
            // Since the property is static - the object parameter is irrelevant for the GetValue() function and we can use null

            try // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
            {
                if (!(type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null) is IDal dal))
                       throw new DALConfigException($"Class {dalNameSpace}.{dalClass} instance is not initialized");
                // now it looks like we have appropriate dal implementation instance :-)
                return dal;
            }
            catch (NullReferenceException ex)
            {
                throw new DALConfigException($"Class {dalNameSpace}.{dalClass} is not a singleton", ex);
            }
        }
    }
}






