using System.Text.Json.Serialization;

namespace VehicleExplorer.Web.Integrations.ExternalVehicleProvider.Dtos
{
    public class MakeDto
    {
        [JsonPropertyName("Make_ID")]
        public int MakeId { get; set; }

        [JsonPropertyName("Make_Name")]
        public string MakeName { get; set; }
    }
}
