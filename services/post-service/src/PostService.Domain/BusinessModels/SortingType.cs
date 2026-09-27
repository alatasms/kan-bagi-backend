using System.Text.Json.Serialization;

namespace PostService.Domain.BusinessModels
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortingType
    {
        Newest = 1,
        Oldest = 2
    }
}
