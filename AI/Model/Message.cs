﻿using System.Text.Json.Serialization;

namespace AI.Model;

/// <summary>
/// класс сообщения
/// </summary>
class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = "";
    [JsonPropertyName("content")]
    public string Content { get; set; } = "";
}
