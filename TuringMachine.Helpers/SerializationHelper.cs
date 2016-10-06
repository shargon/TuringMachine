using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TuringMachine.Helpers.Converters;

namespace TuringMachine.Helpers
{
    public class SerializationHelper
    {
        // encoding subset to implement
        public enum EEncoding
        {
            ASCII,
            UTF8,
            UTF7,
            UTF32,
            Unicode,
            BigEndianUnicode
        };

        public enum EFormat : byte
        {
            /// <summary>
            /// Salida en json
            /// </summary>
            Json = 0,
            /// <summary>
            /// Salida en json escapada
            /// </summary>
            JsonIndented = 1,
            /// <summary>
            /// Salida convertida a string
            /// </summary>
            ToString = 2
        }

        static JsonSerializerSettings _Settings;

        static SerializationHelper()
        {
            _Settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            _Settings.Converters.Add(new IPAddressConverter());
            _Settings.Converters.Add(new IPEndPointConverter());
        }

        /// <summary>
        /// Serializa un objeto a un json
        /// </summary>
        /// <param name="data">Datos</param>
        /// <param name="indented">Indented</param>
        public static string SerializeToJson(object data, bool indented = false)
        {
            if (data == null) return null;

            return JsonConvert.SerializeObject(data, indented ? Formatting.Indented : Formatting.None, _Settings);
        }
        /// <summary>
        /// Deserializa un json
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="json">Json</param>
        public static T DeserializeFromJson<T>(string json)
        {
            if (json == null) return default(T);
            return JsonConvert.DeserializeObject<T>(json, _Settings);
        }
        /// <summary>
        /// Deserializa un json
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="json">Json</param>
        public static object DeserializeFromJson(string json, Type type)
        {
            if (json == null || type == null) return null;
            return JsonConvert.DeserializeObject(json, type, _Settings);
        }
        /// <summary>
        /// Serializa un objeto al tipo especificado
        /// </summary>
        /// <param name="o">Objeto</param>
        /// <param name="format">Formato</param>
        public static string Serialize(object o, EFormat format)
        {
            if (o == null) return "";

            switch (format)
            {
                case EFormat.Json: return SerializationHelper.SerializeToJson(o);
                case EFormat.JsonIndented: return SerializationHelper.SerializeToJson(o, true);
                case EFormat.ToString: return o.ToString();
                default: return "";
            }
        }
        /// <summary>
        /// Enumerate all values
        /// </summary>
        /// <param name="json">Json</param>
        public static IEnumerable<KeyValuePair<string, object>> EnumerateFromJson(string json)
        {
            foreach (KeyValuePair<string, JToken> pi in JObject.Parse(json))
                yield return new KeyValuePair<string, object>(pi.Key, pi.Value);
        }
        /// <summary>
        /// Get Encoding
        /// </summary>
        /// <param name="encoding">Encoding</param>
        public static Encoding GetEncoding(EEncoding encoding)
        {
            switch (encoding)
            {
                case EEncoding.ASCII: return Encoding.ASCII;
                case EEncoding.UTF7: return Encoding.UTF7;
                case EEncoding.UTF8: return Encoding.UTF8;
                case EEncoding.UTF32: return Encoding.UTF32;
                case EEncoding.Unicode: return Encoding.Unicode;
                case EEncoding.BigEndianUnicode: return Encoding.BigEndianUnicode;
            }

            return Encoding.Default;
        }
    }
}