using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace ZonalApp.classes
{
    public class GetMenuPages
    {
        RestClient client;
        RestRequest request;
        DBConnection dbConnection;

        public GetMenuPages()
        {
            dbConnection = new DBConnection();
        }
        public void GetMenuPagesInformation(string guid)
        {
       
            RequestDetails details = new RequestDetails();

            details.HeaderInformation(out client, out request);

            //get the menu pages information
            string getMenusBodyStr = "{\"request\": {\"method\":\"getmenupages\"," +
                " \"siteId\": " +
                Convert.ToInt32(ConfigurationManager.AppSettings["siteId"]) + ", " +
                 "\"salesAreaId\" : " +
                Convert.ToInt32(ConfigurationManager.AppSettings["salesAreaId"]) +
                ", \"servicesId\" : " +
                Convert.ToInt32(ConfigurationManager.AppSettings["serviceId"]) +
                ", \"menuId\" : " +
                Convert.ToInt32(ConfigurationManager.AppSettings["menuId"]) +
                  ", \"platform\" : " + "\"" + (ConfigurationManager.AppSettings["platform"]) + "\"" +
                "}}";
            
            //execute the venue body
             request.AddParameter("request", getMenusBodyStr);
            Log.Info($"Execute the getmenupages request");

            //generate the response
            IRestResponse response = client.Execute(request);

            //get the Menu data as a string
             string menuStr = JsonConvert.SerializeObject(response.Content);

            //remove formatting
            menuStr = menuStr.Replace(@"\", string.Empty);
            menuStr = menuStr.Remove(0, 1); // remove " from beginning 
           
            dbConnection.CallStoredProcs(menuStr, guid);

         
        }

    }
}
