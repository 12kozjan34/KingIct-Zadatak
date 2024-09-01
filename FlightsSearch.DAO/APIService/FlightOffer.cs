namespace FlightsSearch.Models
{
    public class FlightOffer
    {
        public string DepartureAirport { get; set; }
        public string DestinationAirport { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public int NumberOfTransfers { get; set; }
        public int NumberOfPassengers { get; set; }
        public string Currency { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
