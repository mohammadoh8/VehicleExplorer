using System.Text.Json.Serialization;

namespace VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa.Dtos
{
    public class NhtsaVehicleTypeDto
    {
        [JsonPropertyName("Make_ID")]
        public int MakeId { get; set; }

        [JsonPropertyName("Make_Name")]
        public string MakeName { get; set; }

        [JsonPropertyName("Model_ID")]
        public int ModelId { get; set; }

        [JsonPropertyName("Model_Name")]
        public string ModelName { get; set; }

        public static VehicleType ToVehicleType(NhtsaVehicleTypeDto vehicleType)
        {
            return new VehicleType(vehicleType.ModelId, vehicleType.ModelName.Trim());
        }
    }
}
