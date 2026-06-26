using System.Text.Json.Serialization;

namespace VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa.Dtos
{
    public class NhtsaVehicleTypeDto
    {
        public int VehicleTypeId { get; set; }

        public string VehicleTypeName { get; set; } = string.Empty;

        public static VehicleType ToVehicleType(NhtsaVehicleTypeDto vehicleType)
        {
            return new VehicleType(vehicleType.VehicleTypeId, vehicleType.VehicleTypeName?.Trim() ?? string.Empty);
        }
    }
}
