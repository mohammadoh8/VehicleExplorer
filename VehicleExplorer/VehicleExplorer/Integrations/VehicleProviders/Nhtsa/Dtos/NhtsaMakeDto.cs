using System.Text.Json.Serialization;

namespace VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa.Dtos
{
    public class NhtsaMakeDto
    {
        [JsonPropertyName("Make_ID")]
        public int MakeId { get; set; }

        [JsonPropertyName("Make_Name")]
        public string MakeName { get; set; }

        public static VehicleMake ToVehicleMake(NhtsaMakeDto make)
        {
            return new VehicleMake(make.MakeId, make.MakeName.Trim());
        }
    }
}
