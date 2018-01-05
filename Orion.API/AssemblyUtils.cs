using Orion.API.Models;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Orion.API
{
	/// <summary></summary>
	public static class AssemblyUtils
    {

        /// <summary></summary>
        public static string GetTitle(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetTitle(asm);
        }
		/// <summary></summary>
		public static string GetTitle(Type type)
		{
			return GetTitle(type.Assembly);
		}
		/// <summary></summary>
		public static string GetTitle(Assembly asm)
        {
            return asm.GetName().Name;
        }


        /// <summary></summary>
        public static Version GetVersion(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetVersion(asm);
        }
		/// <summary></summary>
		public static Version GetVersion(Type type)
		{
			return GetVersion(type.Assembly);
		}
		/// <summary></summary>
		public static Version GetVersion(Assembly asm)
        {
            return asm.GetName().Version;
        }


        /// <summary></summary>
        public static string GetDescription(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetDescription(asm);
        }
		/// <summary></summary>
		public static string GetDescription(Type type)
		{
			return GetDescription(type.Assembly);
		}
		/// <summary></summary>
		public static string GetDescription(Assembly asm)
        {
            return asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        }



        /// <summary></summary>
        public static AssemblyMeta GetMeta(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetMeta(asm);
        }

		/// <summary></summary>
		public static AssemblyMeta GetMeta(Type type)
		{
			return GetMeta(type.Assembly);
		}

		/// <summary></summary>
		public static AssemblyMeta GetMeta(Assembly asm)
        {
            var meta = new AssemblyMeta
            {
                Title = GetTitle(asm),
                Version = GetVersion(asm),
                Description = GetDescription(asm),
                Culture = asm.GetName().CultureName,
                Configuration = asm.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration,
                Company = asm.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company,
                Product = asm.GetCustomAttribute<AssemblyProductAttribute>()?.Product,
                Copyright = asm.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright,
                Trademark = asm.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark,
                Guid = asm.GetCustomAttribute<GuidAttribute>()?.Value,
                FileVersion = asm.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version,
            };

            return meta;
        }


    }
}
