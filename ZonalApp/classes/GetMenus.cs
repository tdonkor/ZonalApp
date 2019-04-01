using System;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;

namespace ZonalApp.classes
{
    public class GetMenus
    {
        string siteId;
        string salesAreaId;
        string servicesId;

        public GetMenus(string siteId, string salesAreaId, string servicesId)
        {
            this.siteId = siteId;
            this.salesAreaId = salesAreaId;
            this.servicesId = servicesId;
        }

        public void GetMenusInformation()
        {
            var client = new RestClient(ConfigurationManager.AppSettings["url"]);
            var request = new RestRequest(Method.POST);

            //header items
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("x-auth-brandtoken", "Ym91cm5lLWFjcmVsZWMtYmstc3RnOnBhc3N3b3Jk=");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            //get the venue information
            string getMenusBodyStr = "{\"request\": {\"method\":\"getMenus\", \"siteId\": " +
                Convert.ToInt32(siteId) + ", " +
                 "\"salesAreaId\" : " +
                Convert.ToInt32(salesAreaId) +
                ", \"platform\" : \"web\"" +
                ", \"servicesId\" : " +
                Convert.ToInt32(servicesId) +
                "}}";

            Console.WriteLine(getMenusBodyStr);

            //execute the Menus body
            request.AddParameter("request", getMenusBodyStr);

            //generate the response
            IRestResponse response = client.Execute(request);

            dynamic getMenusData = JsonConvert.DeserializeObject<dynamic>(response.Content);

            Console.WriteLine(getMenusData);
        }
    }
    
}
