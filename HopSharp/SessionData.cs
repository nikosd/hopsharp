namespace HopSharp
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class SessionData
    {
        [JsonProperty("data")]
        public IDictionary<string, object> Data { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public SessionData(System.Web.SessionState.HttpSessionState session)
        {
            if (session == null)
                return;

            Data = new Dictionary<string, object>();

            foreach (string key in session.Keys)
                Data.Add(key, session[key].ToString());
        }
    }
}