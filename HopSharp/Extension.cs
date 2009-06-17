namespace HopSharp
{
    using System;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public static void SendToHoptoad(this Exception exception)
        {
            var client = new HoptoadClient(exception);
            client.Send();
        }
    }
}