using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace TuringMachine.Helpers.Converters
{
    public class IPEndPointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPEndPoint));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IPEndPoint ep = (IPEndPoint)value;
            writer.WriteValue(ep.Address.ToString() + "," + ep.Port.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            return token.Value<string>().ToIpEndPoint();

            //JObject jo = JObject.Load(reader);
            //IPAddress address = jo["Address"].ToObject<IPAddress>(serializer);
            //int port = jo["Port"].Value<int>();
            //return new IPEndPoint(address, port);
        }
    }
}