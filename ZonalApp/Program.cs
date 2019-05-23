using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using ZonalApp.classes;

namespace ZonalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Log.Info("\nMenu Pages Started.");
                GetMenuPages menuPages = new GetMenuPages();

                var guid = Guid.NewGuid().ToString();
                Log.Info($"Guid Value set at : {guid}");

                menuPages.GetMenuPagesInformation(guid);
                Log.Info("Menu Pages Finished.\n"); 
            }
            catch (Exception ex)
            {
                Log.Error("Error" + ex);
            }
           
        }

     
    }
}
