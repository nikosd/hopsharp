using Newtonsoft.Json;

namespace HopSharp
{
    internal class HoptoadNoticeSub
    {
        public HoptoadNoticeSub(HoptoadNotice notice)
        {
            this.Notice = notice;
        }

        [JsonProperty("notice")]
        public HoptoadNotice Notice { get; set; }
    }
}