using ErrorDespacho.Models.ModelsDto.Pyload;
using System.Text.Json.Serialization;

namespace ErrorDespacho.Models.P2h
{
    public class P2hGetResponse
    {
        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }

        [JsonPropertyName("dispatchOrders")]
        public List<DispatchOrder>? DispatchOrders { get; set; }
    }

    public class DispatchOrder
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("identificationType")]
        public string? IdentificationType { get; set; }

        [JsonPropertyName("identification")]
        public string? Identification { get; set; }

        [JsonPropertyName("dispatch")]
        public string? Dispatch { get; set; }

        [JsonPropertyName("authorization")]
        public string? Authorization { get; set; }

        [JsonPropertyName("payload")]
        public string? Payload { get; set; } // Este es el JSON string que mencionas

        [JsonPropertyName("sent")]
        public bool Sent { get; set; }

        [JsonPropertyName("creationDate")]
        public DateTime? CreationDate { get; set; }

        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; }

        [JsonPropertyName("sentDate")]
        public DateTime? SentDate { get; set; }

        [JsonPropertyName("statusMessage")]
        public string? StatusMessage { get; set; }

        [JsonPropertyName("statusCode")]
        public string? StatusCode { get; set; }

        [JsonPropertyName("getResponse")]
        public bool GetResponse { get; set; }

        [JsonPropertyName("statusResponse")]
        public string? StatusResponse { get; set; }

        [JsonPropertyName("responseDate")]
        public DateTime? ResponseDate { get; set; }

        [JsonPropertyName("response")]
        public string? Response { get; set; }
    }

}
