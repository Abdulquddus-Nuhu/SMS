namespace Models.Responses
{
    public record BusResponse
    {
        public Guid Id { get; set; }
        public string BusNumber { get; set; } = string.Empty;
        public int NumberOfSeat { get; set; }
    }
}
