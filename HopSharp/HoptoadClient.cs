namespace HopSharp
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;

    /// <summary>
    /// 
    /// </summary>
    public class HoptoadClient
    {
        /// <summary>
        /// Prepare a message for HopToad from an exception.
        /// </summary>
        /// <param name="e"></param>
        /// <exception cref="ArgumentNullException"><c>e</c> is null.</exception>
        public HoptoadClient(Exception e)
        {
            if (e == null) throw new ArgumentNullException("e");
            
            Notice = new HoptoadNotice
                         {
                             ErrorClass = e.GetType().FullName,
                             ErrorMessage = e.GetType().Name + ": " + e.Message,
                             Backtrace = e.StackTrace,
                         };

            if (HttpContext.Current != null)
                Notice.HttpContext = HttpContext.Current;
        }

        /// <summary>
        /// Prepare a message for HopToad from a custom created notice.
        /// </summary>
        /// <param name="notice"></param>
        /// <exception cref="ArgumentNullException"><c>notice</c> is null.</exception>
        public HoptoadClient(HoptoadNotice notice)
        {
            if (notice == null)
                throw new ArgumentNullException("notice");
            
            Notice = notice;
        }

        /// <summary>
        /// Gets the <see cref="HoptoadNotice"/> object that will be serialized and sent to
        /// HopToad.
        /// </summary>
        public HoptoadNotice Notice { get; private set; }

        /// <summary>
        /// Send the notice to HopToad.
        /// </summary>
        /// <remarks>Notice must be already set up either from 
        /// <see cref="HoptoadClient(Exception)"/> or from 
        /// <see cref="HoptoadClient(HoptoadNotice)"/>.</remarks>
        public void Send()
        {
            try
            {
                // If no API key, get it from the appSettings
                if (string.IsNullOrEmpty(Notice.ApiKey))
                {
                    var configuration = Configuration.GetConfig();

                    // If none is set, just return... throwing an exception is pointless, since one was already thrown!
                    if (string.IsNullOrEmpty(configuration.Key))
                        return;

                    Notice.ApiKey = configuration.Key;
                }

                // Create the web request
                var request = WebRequest.Create("http://hoptoadapp.com/notices/") as HttpWebRequest;
                if (request == null)
                    return;

                // Set the basic headers
                request.ContentType = "application/json";
                request.Accept = "text/xml, application/xml";
                request.KeepAlive = false;

                // It is important to set the method late... .NET quirk, it will interfere with headers set after
                request.Method = "POST";

                // Go populate the body
                setRequestBody(request);

                // Begin the request, yay async
                request.BeginGetResponse(requestCallback, null);
            }
            catch
            {
                // Since an exception was already thrown, allowing another one to bubble up is pointless
                // But we should log it or something
                // TODO this could be better
            }
        }

        /// <summary>
        /// Handle the request when it finishes.
        /// </summary>
        /// <param name="ar"></param>
        private static void requestCallback(IAsyncResult ar)
        {
            // Get it back
            var request = ar.AsyncState as HttpWebRequest;
            if (request == null)
                return;

            // We want to swallow any error responses
            try
            {
                request.EndGetResponse(ar);
            }
            catch (WebException e)
            {
                // Since an exception was already thrown, allowing another one to bubble up is pointless
                // But we should log it or something
                // TODO this could be better
                Console.WriteLine("." + e.Message + ".");
                var sr = new StreamReader(e.Response.GetResponseStream());
                Console.WriteLine(sr.ReadToEnd());
                sr.Close();
            }
        }

        /// <summary>
        /// Serialize the <see cref="Notice"/> and write the data to the 
        /// <paramref name="request"/>content.
        /// </summary>
        /// <param name="request"></param>
        private void setRequestBody(HttpWebRequest request)
        {
            // Get the bytes
            var payload = Encoding.UTF8.GetBytes(Notice.Serialize());
            request.ContentLength = payload.Length;

            // Get the request stream and write the bytes
            using (var stream = request.GetRequestStream())
            {
                stream.Write(payload, 0, payload.Length);
                stream.Close();
            }
        }
    }
}