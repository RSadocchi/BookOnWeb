namespace BookOnWeb.Options
{
    public class ApplicationOptions
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Logo { get; set; }
        public string? Producer_Name { get; set; }
        public string? DefaultCulture { get; set; }
        public string[]? SupportedCultures { get; set; }
        
        public string? Version { get; set; }
        public bool VersionVisible { get; set; }
        public string? SK { get; set; }
        public string? AppURI { get; set; }
        public string? AppAPI_URI { get; set; }
        public string? AppDataPath { get; set; }
        public string? AppLogsPath { get; set; }
        public bool PrivacyRequest { get; set; }
        public bool MigrateOnStart { get; set; }
    }
}
