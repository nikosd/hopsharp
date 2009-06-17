namespace HopSharp
{
    using System.Collections.Generic;
    using System.Web;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class RequestInfo
    {
        [JsonProperty("params")]
        public IDictionary<string, object> Params { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("rails_root")]
        public string ApplicationRoot { get; set; }

        public RequestInfo(HttpRequest request)
        {
            Url = request.Url.AbsoluteUri;

            ApplicationRoot = request.PhysicalApplicationPath != null
                                  ? request.PhysicalApplicationPath.Replace(":", string.Empty).Replace('\\', '/')
                                  : string.Empty;

            Params = new Dictionary<string, object>
                         {
                             { "action", request.HttpMethod },
                             { "controller", request.FilePath }
                         };

            foreach (string param in request.QueryString)
                Params.Add(param, request.QueryString[param]);

            foreach (string param in request.Form)
                Params.Add(param, request.Form[param]);
        }
    }
}