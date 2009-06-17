namespace HopSharp
{
    using Newtonsoft.Json;

    /// <summary>
    /// Used only to "enclose" the <see cref="HoptoadNotice"/> JSON object 
    /// inside a "notice" one (when serializing).
    /// </summary>
    internal class HoptoadNoticeSub
    {
        public HoptoadNoticeSub(HoptoadNotice notice)
        {
            Notice = notice;
        }

        [JsonProperty("notice")]
        public HoptoadNotice Notice { get; set; }
    }
}