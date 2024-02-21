namespace Models.Requests
{
    public record EditParentRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
