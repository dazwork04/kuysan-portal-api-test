namespace SAPB1SLayerWebAPI.Utils
{
    public class Logger
    {
        //public const string LIVE_PATH = "D:\\MAR\\API_LOGS\\";
        //public const string LIVE_PATH = "C:\\MAR\\API_LOGS\\";
        //public const string LIVE_PATH = "E:\\SUPERSPEED\\PORTAL_LOGS\\";
        public const string LIVE_PATH = "C:\\DZL Files\\CLIENT\\SuperSpeed\\DEVELOPMENT PROJECT\\PORTAL WEB API\\Kuysan\\Logger\\";

        public const string PATH = LIVE_PATH;
        public static void CreateLog(bool isError, string title, string message, string data)
        {
            string dateTimeToday = DateTime.Now.ToString("yyyyMMdd");
            string file = isError ? "ERRORS\\" : "INFO\\";
            StreamWriter sw = new(PATH + file + dateTimeToday + ".txt", true);
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + title + ": ");
            sw.WriteLine(message);
            sw.WriteLine(data);
            sw.WriteLine("============================================================");
            sw.Close();
        }
    }
}
