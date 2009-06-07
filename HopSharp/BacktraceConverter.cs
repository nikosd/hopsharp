using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace HopSharp
{
    public class BacktraceConverter : JsonConverter
    {
        private string[] _lines;
        
        private string formatFirstLine()
        {
            string filename;
            string methodName;
            string lineNumber;

            var regex =
                new Regex(@"^\s*at\s(?<method>.+\(.*\))\sin\s(?<filename>.*:)line\s(?<lineNumber>\d*)");
            var match = regex.Match(_lines[0]);

            if (match.Success)
            {
                methodName = match.Groups["method"].Captures[0].Value;
                filename = match.Groups["filename"].Captures[0].Value.Replace(":", "");
                lineNumber = match.Groups["lineNumber"].Captures[0].Value;
            }
            else
            {
                regex = new Regex(@"^\s*at\s(?<method>.+\(.*\))");
                match = regex.Match(_lines[0]);

                methodName = match.Groups["method"].Captures[0].Value;
                filename = "(Source not available)";
                lineNumber = "0";
            }

            return string.Format("{0}:{1}:in `{2}`", filename, lineNumber, methodName);
        }

        #region Newtonsoft.Json.JsonConverter Overrides

        public override void WriteJson(JsonWriter writer, object value)
        {
            _lines = ((string) value).Split('\n');
            
            writer.WriteStartArray();

            // Format the first line to match ruby's way of writing stack trace lines.
            // This is needed because HopToad tries to parse that line to set the
            // "File" row.
            try
            {
                writer.WriteValue(formatFirstLine());
                // Leave some free space from the above custom/hacked line.
                writer.WriteValue(" ");
                writer.WriteValue("--");
                writer.WriteValue("--");
                writer.WriteValue(" ");
            }
            catch{}
            
            // Now write the actual Backtrace
            foreach (string line in _lines)
                writer.WriteValue(line);
            
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) { return true; }

        #endregion
    }
}