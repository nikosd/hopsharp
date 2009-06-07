using System;
using System.Web;

namespace HopSharp
{
    /// <summary>
    /// An HttpModule for ASP.Net applications which adds transparent HopToad
    /// error logging.
    /// 
    /// For more info regarding HopToad see http://www.hoptoadapp.com.
    /// </summary>
	public class NotifierHttpModule : IHttpModule
	{
        /// <summary>
        /// Adds a new <see cref="HoptoadClient"/> <see cref="EventHandler"/> on 
        /// <paramref name="context.Error"/>.
        /// </summary>
        /// <param name="context"></param>
		public void Init(HttpApplication context)
		{
			context.Error += new EventHandler(context_Error);
		}

		void context_Error(object sender, EventArgs e)
		{
            try
            {
                var application = (HttpApplication)sender;
                var exception = application.Server.GetLastError();
                exception = (exception.GetType() == typeof (HttpUnhandledException))
                                ? exception.InnerException
                                : exception;
                var client = new HoptoadClient(exception);
                client.Send();
            }
            catch(Exception ex)
            {
                // TODO: Log the swallowed error...

                // We don't want to cause problems to the application if this
                // thing is not working so we swallow everything. We could log
                // the error for further inspection...
            }
			
		}

		public void Dispose()  { }
	}
}