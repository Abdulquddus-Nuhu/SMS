namespace Models.Responses
{
    public record GradeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public int Arms { get; set; }
        public string CampusName { get; set; } = string.Empty;
    }
}
