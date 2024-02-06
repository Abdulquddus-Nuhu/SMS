namespace Models.Responses
{
    public class CampusResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public List<GradeResponse>? Grades { get; set; }
    }
}
