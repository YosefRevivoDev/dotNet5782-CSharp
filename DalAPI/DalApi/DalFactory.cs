using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    /// Static Factory class for creating Dal tier implementation object according to
    /// configuration in file config.xml
    /// </summary>
    public static class DalFactory
    {
        
        public static IDal GetDal()
        {
            // get dal implementation name from config.xml according to <data> element
            string dalType = DalConfig.DalName;
            // bring package name (dll file name) for the dal name (above) from the list of packages in config.xml
            DalConfig.DalPackage dlPackage;
            try // get dal package info according to <dal> element value in config file
            {
                dlPackage = DalConfig.DalPackages[dalType];
            }
            catch (KeyNotFoundException ex)
            {
                // if package name is not found in the list - there is a problem in config.xml
                throw new DalConfigException($"Wrong DL type: {dalType}", ex);
            }
            string dlPackageName = dlPackage.PkgName;
            string dlNameSpace = dlPackage.NameSpace;
            string dlClass = dlPackage.ClassName;

            try // Load into CLR the dal implementation assembly according to dll file name (taken above)
            {
                Assembly.Load(dlPackageName);
            }
            catch (Exception ex)
            {
                throw new DalConfigException($"Failed loading {dlPackageName}.dll", ex);
            }

            Type type;
            try
            {
                type = Type.GetType($"{dlNameSpace}.{dlClass}, {dlPackageName}", true);
            }
            catch (Exception ex)
            { // If the type is not found - the implementation is not correct - it looks like the class name is wrong...
                throw new DalConfigException($"Class not found due to a wrong namespace or/and class name: {dlPackageName}:{dlNameSpace}.{dlClass}", ex);
            }
            
            try
            {
                // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
                if (!(type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null) is IDal dal))
                    throw new DalConfigException($"Class {dlNameSpace}.{dlClass} instance is not initialized");
                // now it looks like we have appropriate dal implementation instance :-)
                return dal;
            }
            catch (NullReferenceException ex)
            {
                throw new DalConfigException($"Class {dlNameSpace}.{dlClass} is not a singleton", ex);
            }

        }
    }
}
