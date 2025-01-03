namespace IdentityService.Configurations
{
    public class DbConnectionInfo
    {
        private string Host { get; set; }
        private int? Port { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Database { get; set; }

        public string ToConnectionString()
        {
            var conn = $"Host={Host};Database={Database};Username={Username};Password={Password}";
            
            if (Port.HasValue)
            {
                conn += $";Port={Port.Value}";
            }

            return conn;
        }
    }
}
