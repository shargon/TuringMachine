using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace TuringMachine.Core.Design
{
    public class JsonFromToConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            dynamic d = existingValue;
            if (d == null) d = Activator.CreateInstance(objectType);

            TypeConverter conv = TypeDescriptor.GetConverter(objectType.GenericTypeArguments[0]);

            JObject jObject = JObject.Load(reader);

            if (jObject.Property("From") != null) d.From = (dynamic)conv.ConvertFromInvariantString(jObject.GetValue("From").ToString());
            if (jObject.Property("To") != null) d.To = (dynamic)conv.ConvertFromInvariantString(jObject.GetValue("To").ToString());
            if (jObject.Property("Excludes") != null)
            {
                d.Excludes.Clear();
                foreach (object o in ((JArray)jObject.GetValue("Excludes")))
                    d.Excludes.Add((dynamic)conv.ConvertFromInvariantString(o.ToString()));
            }
            //serializer.Populate(jObject.CreateReader(), d);

            return d;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dynamic d = value;

            writer.WriteStartObject();
            if (d.Excludes != null && d.Excludes.Count > 0)
            {
                writer.WritePropertyName("Excludes");
                writer.WriteStartArray();
                foreach (object o in d.Excludes)
                    writer.WriteValue(o);
                writer.WriteEndArray();
            }
            writer.WritePropertyName("To");
            writer.WriteValue(d.To);
            writer.WritePropertyName("From");
            writer.WriteValue(d.From);
            writer.WriteEndObject();
        }
    }
}