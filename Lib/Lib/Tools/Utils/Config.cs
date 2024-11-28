// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Config type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Tools.Utils
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The config.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The primary connection id.
        /// </summary>
        public const string PrimaryConnectionId = "SqlConn";

        /// <summary>
        /// The get web config value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="fromRoot">
        /// The from Root.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetConfigValue(string key, IConfiguration Configuration=null)
        {
            if (Configuration == null)
            {
                var lEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var jSonFile = "appsettings.json";
                if(!string.IsNullOrWhiteSpace(lEnvironment))
                    jSonFile = "appsettings."+ lEnvironment + ".json";
                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jSonFile);

                Configuration = builder.Build();
            }
            return Configuration["App:"+ key];
        }

        /// <summary>
        /// The get connection string.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="fromRoot">
        /// The from root.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetConnectionString(string key, IConfiguration Configuration = null)
        {
            if (Configuration == null)
            {
                var lEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var jSonFile = "appsettings.json";
                if (!string.IsNullOrWhiteSpace(lEnvironment))
                    jSonFile = "appsettings." + lEnvironment + ".json";
                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jSonFile)
                .AddEnvironmentVariables();

                Configuration = builder.Build();
            }
            return Configuration["Data:" + key];
        }
    }
}