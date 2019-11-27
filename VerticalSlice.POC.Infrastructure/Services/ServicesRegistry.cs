using StructureMap;
using VerticalSlice.POC.Services.IServices;

namespace VerticalSlice.POC.Infrastructure.Services
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry()
        {
            For<IConfigsService>().Use<ConfigsService>();
        }
    }
}
