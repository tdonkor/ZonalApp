using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using ZonalApp.classes;

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

            //get the venue information
             string venueBody = @"{""request"": {""method"":""venues"", ""platform"":""web""}}";

            Console.WriteLine(venueBody);

            //execute the venue body
             request.AddParameter("request", venueBody);

            //generate the response
            IRestResponse response = client.Execute(request);

            //get the venue data first as a string then an object
            // string venuesStr = JsonConvert.SerializeObject(response.Content);
            string venuesStr = JsonConvert.SerializeObject(response.Content);

            //prepare the class for conversion
            dynamic venuesData =  JsonConvert.DeserializeObject<dynamic>(response.Content);

            //  Console.WriteLine(venuesStr);
            // Console.WriteLine(venuesData);

            //
            //Console.WriteLine(venuesData["platform"].ToString());
            //Console.WriteLine(venuesData["rearMenu"].ToString());
            //Console.WriteLine(venuesData["services"].ToString());
            //Console.WriteLine(venuesData["venues"].ToString());

            //get the venue Id details
            string siteId = venuesData["venues"][0].id.ToString();
            string salesAreaId = venuesData["venues"][0]["salesArea"][0].id.ToString();
            string servicesId = venuesData["services"][0].id.ToString();

            Console.WriteLine(" Site Id     = " + siteId);
            Console.WriteLine(" Area Id     = " + salesAreaId);
            Console.WriteLine(" Services Id = " + servicesId);



            //Console.WriteLine(venuesData["count"].ToString());
            //Console.WriteLine(venuesData["supportsLoyalty"].ToString());
            //Console.WriteLine(venuesData["response"].ToString());

            //get the menu information

            GetMenus menus = new GetMenus(siteId, salesAreaId, servicesId);
            menus.GetMenusInformation();
            Console.ReadKey();
        }
    }
}
