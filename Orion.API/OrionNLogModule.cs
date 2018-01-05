using Autofac;
using Autofac.Core;
using System;
using System.Linq;

namespace Orion.API
{
	/// <summary>Autofac NLog 模組，會根據 class Type 配置 log name</summary>
	public class OrionNLogModule : Module
	{

		/// <summary>在模組載入後註冊類別到 ContainerBuilder</summary>
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<OrionNLogLogger>().As<IOrionLogger>();
			builder.RegisterType<OrionNLogLoggerFactory>().As<IOrionLoggerFactory>();
		}


		/// <summary>配置 Resolve 事件，可以在物件解析建構子 Parameter 時，取得被注入的類別</summary>
		protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
		{
			registration.Preparing += onComponentPreparing;
		}


		private static void onComponentPreparing(object sender, PreparingEventArgs e)
		{
			/* 取得要建構的類別 Type */
			Type typePreparing = e.Component.Activator.LimitType;

			/* 建立 ResolvedParameter，會根據條件判斷才去建立需要的類別 */
			var parameter = new ResolvedParameter(
				(p, i) => p.ParameterType == typeof(IOrionLogger),
				(p, i) => new OrionNLogLogger(typePreparing)
			);

			/* 增加 Parameter 到優先候選清單中 */
			e.Parameters = e.Parameters.Concat(new[] { parameter });
		}
	}

}
