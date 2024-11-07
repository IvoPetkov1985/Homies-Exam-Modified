using System.ComponentModel.DataAnnotations;
using static Homies.Data.Common.DataConstants;

namespace Homies.Models
{
    public class EventFormModel
    {
        [Required]
        [StringLength(EventNameMaxLength, MinimumLength = EventNameMinLength, ErrorMessage = EventInputErrorMsg)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(EventDescMaxLength, MinimumLength = EventDescMinLength, ErrorMessage = EventInputErrorMsg)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression(DateTimeRegex, ErrorMessage = DateTimeMessage)]
        public string Start { get; set; } = string.Empty;

        [Required]
        [RegularExpression(DateTimeRegex, ErrorMessage = DateTimeMessage)]
        public string End { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int TypeId { get; set; }

        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}
