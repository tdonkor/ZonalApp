using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Net;

namespace ZonalApp
{
    class Program
    {
        static void Main(string[] args)
        {
      


            var client = new RestClient(ConfigurationManager.AppSettings["url"]);
            var request = new RestRequest(Method.POST);

            //header items
             request.AddHeader("cache-control", "no-cache");
             request.AddHeader("x-auth-brandtoken", "Ym91cm5lLWFjcmVsZWMtYmstc3RnOnBhc3N3b3Jk=");
             request.AddHeader("Accept", "application/json");
             request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            //get the venue
             string venueBody = @"{""request"": {""method"":""venues"", ""platform"":""web""}}";

            request.AddParameter("request", venueBody);

            IRestResponse response = client.Execute(request);

            //get the venue data first as a string then an object
            string jsonVenueStr = JsonConvert.SerializeObject(response.Content);
            dynamic venueData =  JsonConvert.DeserializeObject(response.Content);

            Console.WriteLine(jsonVenueStr);
            Console.WriteLine(venueData);

            string siteID = venueData["venues"].;

            //  string testGetMenuPages = "{""request"": {""method"":""venues"", ""platform"":""web""}}"; "

            Console.ReadKey();
        }
    }
}
