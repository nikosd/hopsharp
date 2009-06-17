namespace HopSharp
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using Newtonsoft.Json;

    /// <summary>
    /// The object class that encapsulates all the data regarding the exception and the
    /// environment details that will be sent to HopToad.
    /// </summary>
    public class HoptoadNotice
    {
        #region JSON Serialized objects.
        
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("error_class")]
        public string ErrorClass { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("environment")]
        public IDictionary<string, object> Environment { get; private set; }

        [JsonProperty("request")]
        public RequestInfo Request { get; set; }

        [JsonProperty("session")]
        public SessionData Session { get; set; }

        [JsonProperty("backtrace")]
        [JsonConverter(typeof(BacktraceConverter))]
        public string Backtrace { get; set; }
        
        #endregion

        #region Normal properties

        [JsonIgnore]
        public HttpContext HttpContext
        {
            get
            {
                return _httpContext;
            }

            set
            {
                _httpContext = value;
                Request = new RequestInfo(_httpContext.Request);
                Session = new SessionData(_httpContext.Session);
                setRequestParamsInEnvironment();
            }
        }
        private HttpContext _httpContext;

        #endregion

        /// <summary>
        /// Creates a new HoptoadNotice, inits the dictionaries and sets the build
        /// configuration (debug or release) plus the environment (tries to read the
        /// environment name from the config file or sets it as "Default".
        /// </summary>
        public HoptoadNotice()
        {
            Environment = new Dictionary<string, object>();
            setEnvironment();
        }

        /// <summary>
        /// Serializes the current instance into JSON form (including the root "notice"
        /// element).
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return JavaScriptConvert.SerializeObject(new HoptoadNoticeSub(this));
        }

        private void setEnvironment()
        {
#if (DEBUG)
            const string build = "Debug";
#else
            const string build = "Release"
#endif

            var configuration =
                (Configuration.GetConfig() == null ||
                 string.IsNullOrEmpty(Configuration.GetConfig().Environment))
                    ? "Default"
                    : Configuration.GetConfig().Environment;

            var environment = string.Format("{0} [{1}]", configuration, build);

            Environment.Add("RAILS_ENV", environment);
        }

        private void setRequestParamsInEnvironment()
        {
            if (HttpContext == null || HttpContext.Request == null)
                return;

            foreach (var key in HttpContext.Request.Params.AllKeys)
                if (HttpContext.Request.Params[key] != null && 
                    !string.IsNullOrEmpty(HttpContext.Request.Params[key]))
                Environment.Add(key, HttpContext.Request.Params[key]);

            // BUG: When we send the following parameter to HopToad it breaks...
            // (This is why it's removed from the "Environment" entries.
            Environment.Remove("APPL_PHYSICAL_PATH");
        }
    }
}