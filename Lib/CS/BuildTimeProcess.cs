using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CS
{
    /// <summary>
    /// 
    /// add project
    /// 
    /// <PropertyGroup>
    ///     <AssemblyVersion>1.0.*</AssemblyVersion>
	///     <Deterministic>false</Deterministic>
    /// </PropertyGroup>
    /// 
    /// </summary>
    internal class BuildTimeProcess
    {
        public static DateTime GetBuildTime()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new Exception();
            return new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
        }
    }
}
