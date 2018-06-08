using Fbo.CryptoMarketData.Exceptions;
using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Fbo.CryptoMarketData.JsonUtils
{
    /// <summary>
    /// Json converter between Unix timestamp and <see cref="System.Nullable{DateTime}" <c>(DateTime?)</c>/>.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class UnixTimestampNullableDateTimeConverter : JsonConverter
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="Fbo.CryptoMarketData.Exceptions.CryptoMarketDataException"> 
        /// is thrown if <paramref name="objectType"/> is not <see cref="System.Int32"/> or <see langword="null"/>. 
        /// </exception>
        public override object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType != JsonToken.Integer)
                throw new CryptoMarketDataException($"Invalid token type. Expected: '{nameof(JsonToken.Integer)}'; Received: '{reader.TokenType}'.");

            var valueRead = reader.Value.ToString();
            if (!Double.TryParse(valueRead, NumberStyles.Number, CultureInfo.InvariantCulture, out double totalSeconds))
                throw new CryptoMarketDataException($"Invalid number value. Data received: '{valueRead}'.");

            return UnixEpoch.AddSeconds(totalSeconds);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <exception cref="Fbo.CryptoMarketData.Exceptions.CryptoMarketDataException">
        /// is thrown if <paramref name="value"/> is not <see cref="System.DateTime"/> or <see langword="null"/>.
        /// An inner exception is created with more information about the <paramref name="value"/> received and its data type.</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if ((value != null) && (!value.GetType().Equals(typeof(DateTime))))
                throw new CryptoMarketDataException("Unable to convert value due to incompatible data types.", 
                                                    new ArgumentException($"Invalid data type. Expected: '{nameof(DateTime)}'; Received: '{value.GetType()}'.", nameof(value))
                                                   );

            if (value == null)
                writer.WriteNull();
            else
                writer.WriteValue((Int64)((DateTime)value - UnixEpoch).TotalSeconds);
        }
    }
}
