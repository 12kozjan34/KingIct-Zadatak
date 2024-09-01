using FlightsSearch.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FlightsSearch.DAO.APIService
{
    public class FlightService
    {
        private readonly HttpClient _client;
        private readonly ApiSettings _apiSettings;

        public FlightService(HttpClient client, IOptions<ApiSettings> apiSettings)
        {
            _client = client;
            _apiSettings = apiSettings.Value;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var values = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _apiSettings.ApiKey },
                { "client_secret", _apiSettings.ApiSecret }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await _client.PostAsync("https://test.api.amadeus.com/v1/security/oauth2/token", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
            return jsonResponse.access_token;
        }

        public async Task<IEnumerable<FlightOffer>> GetFlightOffersAsync(string origin, string destination, string departureDate, int? adults, string? currency)
        {
            /*var token = await GetAccessTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode=SYD&destinationLocationCode=BKK&departureDate=2021-11-01&adults=1&max=2";

            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();
             */

            var flightOffers = GetMockFlightOffers();

            var query = flightOffers.Where(f => f.DepartureAirport == origin &&
                                                 f.DestinationAirport == destination &&
                                                 f.DepartureDate == departureDate);

            if (adults.HasValue && adults.Value > 0)
            {
                query = query.Where(f => f.NumberOfPassengers >= adults.Value);
            }

            if (!string.IsNullOrWhiteSpace(currency))
            {
                query = query.Where(f => f.Currency == currency);
            }

            query = query.OrderByDescending(f => f.TotalPrice);

            return query;

        }

        public IEnumerable<FlightOffer> GetMockFlightOffers()
        {
            return new List<FlightOffer>
            {
                // SYD to JFK
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "JFK", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 5, Currency = "AUD", TotalPrice = 1200.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "JFK", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 3, Currency = "AUD", TotalPrice = 1100.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "JFK", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 2, Currency = "AUD", TotalPrice = 2100.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "JFK", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 4, Currency = "AUD", TotalPrice = 1250.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "JFK", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 6, Currency = "AUD", TotalPrice = 1150.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "JFK", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 1, Currency = "AUD", TotalPrice = 2150.00m },

                // SYD to LHR
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "LHR", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-16", NumberOfPassengers = 7, Currency = "AUD", TotalPrice = 1300.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "LHR", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-16", NumberOfPassengers = 4, Currency = "AUD", TotalPrice = 1200.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "LHR", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-16", NumberOfPassengers = 5, Currency = "AUD", TotalPrice = 2300.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "LHR", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-21", NumberOfPassengers = 3, Currency = "AUD", TotalPrice = 1350.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "LHR", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-21", NumberOfPassengers = 5, Currency = "AUD", TotalPrice = 1250.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "LHR", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-21", NumberOfPassengers = 2, Currency = "AUD", TotalPrice = 2350.00m },

                // SYD to DXB
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 8, Currency = "AUD", TotalPrice = 1100.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 6, Currency = "AUD", TotalPrice = 1050.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 4, Currency = "AUD", TotalPrice = 2000.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 7, Currency = "AUD", TotalPrice = 1150.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 9, Currency = "AUD", TotalPrice = 1100.00m },
                new FlightOffer { DepartureAirport = "SYD", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 5, Currency = "AUD", TotalPrice = 2050.00m },

                // JFK to LHR
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "LHR", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 10, Currency = "USD", TotalPrice = 800.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "LHR", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 8, Currency = "USD", TotalPrice = 750.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "LHR", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 4, Currency = "USD", TotalPrice = 1500.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "LHR", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 6, Currency = "USD", TotalPrice = 850.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "LHR", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 5, Currency = "USD", TotalPrice = 800.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "LHR", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 3, Currency = "USD", TotalPrice = 1600.00m },

                // JFK to DXB
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-16", NumberOfPassengers = 6, Currency = "USD", TotalPrice = 950.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-16", NumberOfPassengers = 7, Currency = "USD", TotalPrice = 900.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-16", NumberOfPassengers = 5, Currency = "USD", TotalPrice = 1800.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-21", NumberOfPassengers = 4, Currency = "USD", TotalPrice = 1000.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-21", NumberOfPassengers = 3, Currency = "USD", TotalPrice = 950.00m },
                new FlightOffer { DepartureAirport = "JFK", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-21", NumberOfPassengers = 2, Currency = "USD", TotalPrice = 1900.00m },

                // LHR to DXB
                new FlightOffer { DepartureAirport = "LHR", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 9, Currency = "GBP", TotalPrice = 600.00m },
                new FlightOffer { DepartureAirport = "LHR", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 7, Currency = "GBP", TotalPrice = 550.00m },
                new FlightOffer { DepartureAirport = "LHR", DestinationAirport = "DXB", DepartureDate = "2024-09-15", ArrivalDate = "2024-09-15", NumberOfPassengers = 5, Currency = "GBP", TotalPrice = 1100.00m },
                new FlightOffer { DepartureAirport = "LHR", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 8, Currency = "GBP", TotalPrice = 650.00m },
                new FlightOffer { DepartureAirport = "LHR", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 4, Currency = "GBP", TotalPrice = 600.00m },
                new FlightOffer { DepartureAirport = "LHR", DestinationAirport = "DXB", DepartureDate = "2024-09-20", ArrivalDate = "2024-09-20", NumberOfPassengers = 2, Currency = "GBP", TotalPrice = 1200.00m }
            };
        }
    }
}
