namespace Access.API.Models.Responses
{
    public record BusResponse
    {
        public Guid Id { get; set; }
        public string BusNumber { get; set; } = string.Empty;
    }
}
