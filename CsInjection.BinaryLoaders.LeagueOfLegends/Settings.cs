namespace LolBinaryLoader
{
    public class Settings
    {
        public string BaseUrl { get; set; }
        public string ListFileName { get; set; }
        public string UrlPath { get; set; }
        public ClientType ClientType { get; set; }
    }

    public enum ClientType
    {
        Mac,
        Windows
    }
}