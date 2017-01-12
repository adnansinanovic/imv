namespace Sinantrop.Helper
{
    public static class ObjectExtension
    {
        public static object GetValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
