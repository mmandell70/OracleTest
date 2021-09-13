using System;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OracleTest
{
    class Program
    {
        static AppSettings appSettings = new AppSettings();

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            ConfigurationBinder.Bind(configuration.GetSection("AppSettings"), appSettings);

            string conString = appSettings.ConnectionString;
            Console.WriteLine(conString);

            OracleConfiguration.WalletLocation = appSettings.OracleWalletLocation;
            OracleConfiguration.TnsAdmin = appSettings.OracleTnsAdmin.Length > 0 ? appSettings.OracleTnsAdmin : null;
            OracleConfiguration.SSLVersion = appSettings.OracleSSLVersion;
            OracleConfiguration.SSLServerDNMatch = appSettings.OracleSSLServerDNMatch == "true" ? true : false;

            // Set tracing options
            OracleConfiguration.TraceOption = appSettings.TraceOption;
            //OracleConfiguration.TraceFileLocation = appSettings.TraceFileLocation;
            OracleConfiguration.TraceFileLocation = Directory.GetCurrentDirectory();
            OracleConfiguration.TraceLevel = appSettings.TraceLevel;

            // Set default statement cache size to be used by all connections.
            OracleConfiguration.StatementCacheSize = 25;

            // Disable self tuning by default.
            OracleConfiguration.SelfTuning = false;

            // Bind all parameters by name.
            OracleConfiguration.BindByName = true;

            // Set default timeout to 60 seconds.
            OracleConfiguration.CommandTimeout = 60;

            // Set default fetch size as 1 MB.
            OracleConfiguration.FetchSize = 1024 * 1024;

            // Set network properties
            OracleConfiguration.SendBufferSize = 8192;
            // OracleConfiguration.ReceiveBuffereSize = 8192;
            OracleConfiguration.DisableOOB = true;

            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.CommandText = "SELECT * FROM v$version";

                        //Execute the command and use DataReader to retrieve the data.
                        OracleDataReader reader = cmd.ExecuteReader();
                        reader.Read();

                        //Output database version connection info to page.
                        Console.WriteLine("Connected to " + reader.GetString(0));
                    }
                    catch (Exception ex)
                    {
                        //If application fails, output error message to page.
                        string text = ex.Message;
                        Console.WriteLine(text);
                    }
                }
            }
        }
    }
}
