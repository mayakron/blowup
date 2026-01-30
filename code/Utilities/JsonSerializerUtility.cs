using System.Text.Json;

namespace BlowUp.Utilities
{
    public static class JsonSerializerUtility
    {
        public static T DeserializeFromString<T>(string text)
        {
            return JsonSerializer.Deserialize<T>(text);
        }

        public static string SerializeToString(object item)
        {
            return JsonSerializer.Serialize(item, item.GetType());
        }
    }
}