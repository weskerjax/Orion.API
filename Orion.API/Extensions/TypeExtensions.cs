using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.API.Extensions
{

	/// <summary>定義 Type 的 Extension</summary>
	public static class TypeExtensions
	{

		/// <summary>判斷型別是否為指定型別或子型別</summary>
		public static bool IsTypeOf<T>(this Type type)
		{
			return IsTypeOf(type, typeof(T));
		}


		/// <summary>判斷型別是否為指定型別或子型別</summary>
		public static bool IsTypeOf(this Type type, Type definition)
		{
			if (definition.IsAssignableFrom(type)) { return true; }
			if (!definition.IsGenericType) { return false; }


			bool isMatch = type.IsGenericType && type.GetGenericTypeDefinition() == definition.GetGenericTypeDefinition();
			if (isMatch) { return true; }

			if (type.BaseType != null) { isMatch = IsTypeOf(type.BaseType, definition); }
			if (isMatch) { return true; }

			if (!definition.IsInterface) { return false; }

			isMatch = type.GetInterfaces().Any(i => IsTypeOf(i, definition));
			return isMatch;
		}


	}
}
