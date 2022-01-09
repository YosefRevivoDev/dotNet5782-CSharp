using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalAPI
{
    static class DalConfig
    {
        public class DalPackage
        {
            public string Name;
            public string PkgName;
            public string NameSpace;
            public string ClassName;
        }

        internal static string DaLName;
        internal static Dictionary<string, DalPackage> DaLPackages;

        ///<summary>
        ///static Ctor extracts Dal packages list and Dal type from
        ///Dal configuration file config.xml
        ///</summary> 
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"config.xml");
            DaLName = dalConfig.Element("dal").Value;
            DaLPackages = (from pkg in dalConfig.Element("dal-LPackages").Elements()
                           let temp1 = pkg.Attribute("namespace")
                           let nameSpace = temp1 == null ? "DAL" : temp1.Value
                           let temp2 = pkg.Attribute("class")
                           let className = temp2 == null ? pkg.Value : temp2.Value
                           select new DalPackage()
                           {
                               Name = pkg.Name.ToString(),
                               PkgName = pkg.Value,
                               NameSpace = nameSpace,
                               ClassName = className
                           }).ToDictionary(i => i.Name, i => i);
        }
    }

    /// <summary>
    /// Represents errors during DalApi initialization
    /// </summary>

    [Serializable]
    public class DALConfigException : Exception
    {
        public DALConfigException(string message) : base(message) { }
        public DALConfigException(string message, Exception inner) : base(message, inner) { }
    }
}
