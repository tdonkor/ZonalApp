using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZonalApp.classes
{
    public class DBConnection
    {

        /// <summary>
        /// Connect to the database 
        /// Run stored Procedures
        /// </summary>
        /// <param name="menuStr"></param>
        /// <param name="guid"></param>
        public void CallStoredProcs(string menuStr, string guid)
        {
            int result = 0;

            // Create a new SqlConnection object
            using (SqlConnection con = new SqlConnection())
            {
                // Configure the SqlConnection object
                con.ConnectionString = ConfigurationManager.AppSettings["connectionString"];
                con.Open();
                Log.Info("Connected to the Database");

                //Store the JSonPayload in receivedData table
                result = iOrderGetMenuPages(con, menuStr, guid);

                //Store the xmlPayload data
                FullImport(con, result);

                //Get the XML Payload
                OutputXMLPayload(con);

            }
            Log.Info("Disconnected from the Database");
        }

        /// <summary>
        /// Calls the IOrderGetMenuPages stored Procedure
        /// </summary>
        /// <param name="con"></param>
        /// <param name="menuStr"></param>
        /// <param name="guid"></param>
        /// <returns>result from Stored procedure</returns>
        public static int iOrderGetMenuPages(SqlConnection con, string menuStr, string guid)
        {
            // create and configure a new command 
            SqlCommand com = con.CreateCommand();

            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "iOrderGetMenuPages";

            // Create SqlParameter objects 
            SqlParameter p1 = com.CreateParameter();
            p1.ParameterName = "@JSON";
            p1.SqlDbType = SqlDbType.VarChar;
            p1.Value = menuStr;
            com.Parameters.Add(p1);

            SqlParameter p2 = com.CreateParameter();
            p2.ParameterName = "@URN";
            p2.SqlDbType = SqlDbType.VarChar;
            p2.Value = guid;
            com.Parameters.Add(p2);

            var jsonResult = new StringBuilder();
            int result = 0; 

            // Execute the command and process the results
            using (var reader = com.ExecuteReader())
            {

                if (!reader.HasRows)
                {
                    jsonResult.Append("[]");
                }
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader.GetValue(0));

                    //Log the result
                    Log.Info($"iOrderGetMenuPages output: {result}");
                }
            }
                

            return result;

        }
        /// <summary>
        /// Calls the FullImport stored Procedure with the reults from the 
        /// iOrderGetMenuPages stored procedure
        /// </summary>
        /// <param name="con"></param>
        /// <param name="importResult"></param>
        public static void FullImport(SqlConnection con, int importResult)
        {

            // create and configure a new command 
            SqlCommand com = con.CreateCommand();

            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "FullImport";

            // Create SqlParameter objects 
            SqlParameter p1 = com.CreateParameter();
            p1.ParameterName = "@ReceivedDataID";
            p1.SqlDbType = SqlDbType.Int;
            p1.Value = importResult;
            com.Parameters.Add(p1);

            string urnResult = string.Empty;
            var jsonResult = new StringBuilder();
        
            // Execute the command and process the results
            using (var reader = com.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    jsonResult.Append("[]");
                }

                while (reader.Read())
                {
                    //Display The details
                 
                    jsonResult.Append(reader.GetValue(0).ToString());
                }
            }

        }
        
        public static void OutputXMLPayload(SqlConnection con)
        {
            string path = ConfigurationManager.AppSettings["path"];
            SqlCommand com = con.CreateCommand();
            com.CommandType = CommandType.Text;
            // com.Parameters.Add("RefInt", SqlDbType.VarChar).Value = RefInt;
           

            string xmlPayloadData = string.Empty;
            string tableName = ConfigurationManager.AppSettings["tableName"];

            com.CommandText = $"SELECT XMLPayload FROM {tableName} WHERE id IN (SELECT id FROM {tableName} WHERE datetime = (SELECT MAX(datetime) FROM {tableName}))";

            xmlPayloadData = (string)com.ExecuteScalar();

            //write the data to a string overwrite it if it is already there

            Log.Info($"Write the StoreDataXML file");
            File.WriteAllText(path, xmlPayloadData);

        }

    }
}



