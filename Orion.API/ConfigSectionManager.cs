using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;

namespace Orion.API
{
	/// <summary></summary>
	public static class ConfigSectionManager
	{

		/// <summary></summary>
		public static TSection TryGet<TSection>(string sectionName) where TSection : ConfigurationSection
		{
			return ConfigurationManager.GetSection(sectionName) as TSection;
		}


		/// <summary></summary>
		public static TSection Get<TSection>(string sectionName) where TSection : ConfigurationSection
		{
			var section = TryGet<TSection>(sectionName);
			if (section == null) { throw new ConfigurationErrorsException("缺少 " + sectionName + " 的設定"); }

			return section;
		}


		/// <summary>取得 system.serviceModel/serviceHostingEnvironment 的 RelativeAddress 清單</summary>
		public static List<string> GetServiceAddressList()
		{
			string xpath = "system.serviceModel/serviceHostingEnvironment";
			var section = Get<ServiceHostingEnvironmentSection>(xpath);

			List<string> list = section.ServiceActivations
				.Cast<ServiceActivationElement>()
				.Select(x => x.RelativeAddress)
				.ToList();

			return list;
		}


	}
}
