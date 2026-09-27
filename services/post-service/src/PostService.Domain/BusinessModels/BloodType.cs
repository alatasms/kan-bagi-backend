using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PostService.Domain.BusinessModels
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BloodType
    {
        [Display(Name = "A+")]
        A_Positive = 1,

        [Display(Name = "A-")]
        A_Negative,

        [Display(Name = "B+")]
        B_Positive,

        [Display(Name = "B-")]
        B_Negative,

        [Display(Name = "AB+")]
        AB_Positive,

        [Display(Name = "AB-")]
        AB_Negative,

        [Display(Name = "O+")]
        O_Positive,

        [Display(Name = "O-")]
        O_Negative
    }
}
