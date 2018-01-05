using System.Reflection;
using Xunit;

namespace Orion.API.Tests
{
    public class AssemblyUtilsTests
    {
        
        [Fact]
        public void GetMeta_Test1()
        {
            AssemblyUtils.GetMeta(Assembly.GetExecutingAssembly());
        }


        [Fact]
        public void GetMeta_Test2()
        {
            AssemblyUtils.GetMeta("Orion.API");
        }
    }
}
