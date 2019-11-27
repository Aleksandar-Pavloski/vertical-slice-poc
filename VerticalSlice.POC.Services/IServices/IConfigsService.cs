using StructureMap;

namespace VerticalSlice.POC.Services.IServices
{
    public interface IConfigsService
    {
        string ConnectionString { get; }
    }
}
