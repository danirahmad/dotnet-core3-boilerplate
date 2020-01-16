using Moonlay.Core.Models;

namespace Moonlay.MasterData.WebApi.Models
{
    public class DataSetAttribute : IModel
    {
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public string DataSetName { get; internal set; }
        public string DomainName { get; internal set; }
    }
}
