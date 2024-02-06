namespace Models.Responses
{
    public record DepartmentResponse
    {
        public Guid Id { get; set; }
        public string DeparmentName { get; set; } = string.Empty;
    }
}
