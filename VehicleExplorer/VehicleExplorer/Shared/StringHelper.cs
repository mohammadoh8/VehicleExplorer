namespace VehicleExplorer.Web.Shared
{
    public static class StringHelper
    {
        public static string EncodeString(this string value) => Uri.EscapeDataString(value);
        public static string CombineUrl(string baseUrl , string path) => $"{baseUrl.TrimEnd('/')}/{path.TrimStart('/')}";
    }
}
