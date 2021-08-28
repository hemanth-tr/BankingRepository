using System.Text.Json.Serialization;

namespace BankingRepository.Model
{
    /// <summary>
    /// Represents status.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        /// <summary>
        /// Created status.
        /// </summary>
        Created = 0,

        /// <summary>
        /// Created status.
        /// </summary>
        Active = 1,
    }
}
