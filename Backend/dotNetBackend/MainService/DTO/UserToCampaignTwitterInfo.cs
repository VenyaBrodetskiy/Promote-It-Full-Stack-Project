﻿using System.Text.Json.Serialization;

namespace dotNetBackend.DTO
{
    public record UserToCampaignTwitterInfo
    {
        [JsonRequired]
        public int UserId { get; set; }
        [JsonRequired]
        public int CampaignId { get; set; }
        [JsonRequired]
        public int CurrentTweetCount { get; set; }
    }
}