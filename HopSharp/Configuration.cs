namespace HopSharp
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Configuration for HopToad.
    /// </summary>
    public class Configuration : ConfigurationSection
    {
        /// <summary>
        /// Gets the HopToad key.
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return this["key"] as string; }
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        [ConfigurationProperty("environment", IsRequired = false)]
        public string Environment
        {
            get { return this["environment"] as string; }
        }

        /// <summary>
        /// Gets the config section for <see cref="HopSharp"/>.
        /// </summary>
        /// <returns></returns>
        public static Configuration GetConfig()
        {
            return (Configuration)ConfigurationManager.GetSection("HopSharp");
        }
    }
}
