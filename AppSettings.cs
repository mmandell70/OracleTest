public class AppSettings
{
    public string ConnectionString { get; set; }
    public string OracleWalletLocation { get; set; }
    public string OracleSSLVersion { get; set; }
    public string OracleTnsAdmin { get; set; }
    public string OracleSSLServerDNMatch { get; set; }
    public int TraceOption { get; set; }
    public string TraceFileLocation { get; set; }
    public int TraceLevel { get; set; }
}