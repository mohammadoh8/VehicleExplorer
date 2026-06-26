namespace VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa.Dtos
{
    public class NhtsaModelDto
    {
        public int VehicleTypeId { get; set; }

        public string VehicleTypeName { get; set; }

        public static VehicleModel ToVehicleModel(NhtsaModelDto model)
        {
            return new VehicleModel(model.VehicleTypeId, model.VehicleTypeName.Trim());
        }
    }
}
