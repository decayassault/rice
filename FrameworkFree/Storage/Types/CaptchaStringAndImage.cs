using System.Text.Json.Serialization;
namespace Own.Types
{
    [JsonSerializable(typeof(CaptchaStringAndImage))]
    public sealed class CaptchaStringAndImage
    {
        [JsonPropertyName("key")]
        public uint stringHash { get; set; }
        [JsonPropertyName("image")]
        public string image { get; set; }
    }
}