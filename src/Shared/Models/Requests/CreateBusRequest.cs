namespace Models.Requests
{
    public record CreateBusRequest
    {
        public string BusNumber { get; set; } = string.Empty;
        public int NumberOfSeat { get; set; }
    }
}
