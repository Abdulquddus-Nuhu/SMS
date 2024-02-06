using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public record CreateCampusRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;
    }
}
