using Microsoft.Extensions.Configuration;
using VerticalSlice.POC.Services.IServices;

namespace VerticalSlice.POC.Infrastructure.Services
{
    public class ConfigsService : IConfigsService
    {
        IConfiguration _configuration { get; set; }
        public ConfigsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("VerticalSlicePOCDbConnection");
    }
}
