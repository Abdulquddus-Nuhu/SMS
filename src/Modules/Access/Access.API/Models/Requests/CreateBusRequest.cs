namespace Access.API.Models.Requests
{
    public record CreateBusRequest
    {
        public string BusNumber { get; set; } = string.Empty;
    }
}
