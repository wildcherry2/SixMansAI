﻿using System.Text.Json.Serialization;

namespace Database.Structs;

public class FField {
    [JsonPropertyName("name")]
    public string name { get; init; }

    [JsonPropertyName("value")]
    public string value { get; init; }
}