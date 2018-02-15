using System;
using Newtonsoft.Json;

namespace ReddeTT.Models
{
    //testje
    public class User
    {
        [JsonProperty("is_employee")]
        public bool IsEmployee { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("hide_from_robots")]
        public bool HideFromRobots { get; set; }
        [JsonProperty("is_suspended")]
        public bool IsSuspended { get; set; }
        [JsonProperty("link_karma")]
        public int LinkKarma { get; set; }
        [JsonProperty("in_beta")]
        public bool InBeta { get; set; }
        [JsonProperty("comment_karma")]
        public int CommentKarma { get; set; }
        [JsonProperty("over_18")]
        public bool Over18 { get; set; }
        [JsonProperty("is_gold")]
        public bool IsGold { get; set; }
        [JsonProperty("is_mod")]
        public bool IsMod { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("gold_expiration")]
        public int GoldExpiration { get; set; }
        [JsonProperty("inbox_count")]
        public int InboxCount { get; set; }
        [JsonProperty("has_verified_email")]
        public bool HasVerifiedEmail { get; set; }
        [JsonProperty("gold_creddits")]
        public int GoldCreddits { get; set; }
        [JsonProperty("suspension_expiration_utc")]
        public object SuspensionExpirationUtc { get; set; }
    }
}