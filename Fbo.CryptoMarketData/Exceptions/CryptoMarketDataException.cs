using System;

namespace Fbo.CryptoMarketData.Exceptions
{
    /// <summary>
    /// Represents errors that occur during manipulation of crypto market data.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CryptoMarketDataException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoMarketDataException"/> class.
        /// </summary>
        public CryptoMarketDataException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoMarketDataException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CryptoMarketDataException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoMarketDataException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CryptoMarketDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
