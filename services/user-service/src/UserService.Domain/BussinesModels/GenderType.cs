using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserService.Domain.BussinesModels
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GenderType
    {
        [Display(Name ="Male")]
        Male = 1,
        [Display(Name = "Female")]
        Female
    }
}
