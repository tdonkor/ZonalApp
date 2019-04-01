using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;

namespace ZonalApp.classes
{
    public class RequestHeader
    {
        public void HeaderInformation()
        {
            var client = new RestClient(ConfigurationManager.AppSettings["url"]);
            var request = new RestRequest(Method.POST);

            //header items
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("x-auth-brandtoken", "Ym91cm5lLWFjcmVsZWMtYmstc3RnOnBhc3N3b3Jk=");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        }
    }
}
