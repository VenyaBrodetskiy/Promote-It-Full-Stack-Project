﻿namespace dotNetBackend.DTO
{
    public record OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string TwitterHandle { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int ProductId { get; set; }

        public string ProductTitle { get; set; } = null!;

        public int ProductOwnerId { get; set; } 

        public string TransactionState { get; set; } = null!;

    }
}
