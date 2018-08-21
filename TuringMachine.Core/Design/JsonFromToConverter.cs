using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace TuringMachine.Core.Design
{
    public class JsonFromToConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return true; }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var tar = objectType.GenericTypeArguments[0];
            var conv = TypeDescriptor.GetConverter(tar);

            dynamic d = existingValue;

            if (jObject.Property("Name") != null)
            {
                switch (jObject.GetValue("Name").ToString())
                {
                    case "From-To":
                        {
                            d = Activator.CreateInstance(typeof(FromToValue<>).MakeGenericType(tar));

                            if (jObject.Property("From") != null) d.From = (dynamic)conv.ConvertFromInvariantString(jObject.GetValue("From").ToString());
                            if (jObject.Property("To") != null) d.To = (dynamic)conv.ConvertFromInvariantString(jObject.GetValue("To").ToString());
                            if (jObject.Property("Excludes") != null)
                            {
                                d.Excludes.Clear();
                                foreach (object o in ((JArray)jObject.GetValue("Excludes")))
                                    d.Excludes.Add((dynamic)conv.ConvertFromInvariantString(o.ToString()));
                            }
                            break;
                        }
                    case "Fixed":
                        {
                            d = Activator.CreateInstance(typeof(FixedValue<>).MakeGenericType(tar));

                            if (jObject.Property("Allowed") != null)
                            {
                                d.Allowed.Clear();
                                foreach (object o in ((JArray)jObject.GetValue("Allowed")))
                                    d.Allowed.Add((dynamic)conv.ConvertFromInvariantString(o.ToString()));
                            }
                            break;
                        }
                }
            }

            return d;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dynamic d = value;

            var name = d.Name.ToString();
            switch (name)
            {
                case "From-To":
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("Name");
                        writer.WriteValue(name);
                        if (d.Excludes != null && d.Excludes.Count > 0)
                        {
                            writer.WritePropertyName("Excludes");
                            writer.WriteStartArray();
                            foreach (var o in d.Excludes)
                                writer.WriteValue(o);
                            writer.WriteEndArray();
                        }
                        writer.WritePropertyName("To");
                        writer.WriteValue(d.To);
                        writer.WritePropertyName("From");
                        writer.WriteValue(d.From);
                        writer.WriteEndObject();
                        break;
                    }
                case "Fixed":
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("Name");
                        writer.WriteValue(name);
                        if (d.Allowed != null && d.Allowed.Count > 0)
                        {
                            writer.WritePropertyName("Allowed");
                            writer.WriteStartArray();
                            foreach (var o in d.Allowed)
                                writer.WriteValue(o);
                            writer.WriteEndArray();
                        }
                        writer.WriteEndObject();
                        break;
                    }
            }
        }
    }
}