using System.Text.Json.Serialization;

namespace VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa.Dtos
{
    public class NhtsaModelDto
    {
        [JsonPropertyName("Make_ID")]
        public int MakeId { get; set; }

        [JsonPropertyName("Make_Name")]
        public string MakeName { get; set; } = string.Empty;

        [JsonPropertyName("Model_ID")]
        public int ModelId { get; set; }

        [JsonPropertyName("Model_Name")]
        public string ModelName { get; set; } = string.Empty;

        public static VehicleModel ToVehicleModel(NhtsaModelDto vehicleType)
        {
            return new VehicleModel(vehicleType.ModelId, vehicleType.ModelName?.Trim() ?? string.Empty);
        }
    }
}
