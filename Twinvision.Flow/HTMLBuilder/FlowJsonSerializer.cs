using Newtonsoft.Json;

namespace Twinvision.Flow
{
    public class FlowJsonSerializer : JsonSerializer
    {
        public FlowJsonSerializer()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            Formatting = Formatting.Indented;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }
}
