using System.ComponentModel.DataAnnotations;

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
        public int ProductId { get; set; }

        public int CampaignId { get; set; }

        public int StateId { get; set; }
    }

    public record TransactionChangeState
    {
        [Required(ErrorMessage = "Transaction ID is requred param")]
        public int Id { get; set; }
        [Range(1, 2, ErrorMessage = "Can change only ordered and shipped products")]
        public int StateId { get; set; }
    }
}
