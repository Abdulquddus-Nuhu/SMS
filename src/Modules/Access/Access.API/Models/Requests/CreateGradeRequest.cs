namespace Access.API.Models.Requests
{
    public record CreateGradeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public int Arms { get; set; }
        public Guid CampusId { get; set; }
    }
}
