using System;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace CrezberBlog.ApplicationCore.Dto
{
    public class GoogleREspo
    {
        [J("success")]
        public bool? Success { get; set; }

        [J("score")]
        public double? Score { get; set; }

        [J("action")]
        public string Action { get; set; }

        [J("challenge_ts")]
        public DateTime Challenge_ts { get; set; }

        [J("hostname")]
        public string Hostname { get; set; }
    }
}
