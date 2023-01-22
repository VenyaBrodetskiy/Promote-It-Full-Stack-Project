using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace dotNetBackend.DTO
{
    public record TransactionDTO
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int CampaignId { get; set; }

        public int StateId { get; set; }
    }

    public record TransactionRequest
    {
        [JsonRequired]
        public int ProductId { get; set; }
        [JsonRequired]
        public int CampaignId { get; set; }
        [JsonRequired]
        public int StateId { get; set; }
    }

    public record TransactionResponse
    {
        public int TransactionId { get; set; }

        public int NewBalance { get; set; }

        public HttpStatusCode TwitterPostStatus { get; set; }
    }

    public record TransactionChangeState
    {
        [JsonRequired]
        public int Id { get; set; }
        [JsonRequired, Range(1, 2, ErrorMessage = "Can change only ordered and shipped products")]
        public int StateId { get; set; }
    }
}
