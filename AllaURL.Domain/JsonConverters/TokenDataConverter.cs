
using AllaURL.Domain.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization; 

namespace AllaURL.Domain.JsonConverters;

public class TokenDataConverter : JsonConverter<ITokenData>
{
    public override ITokenData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;

            // Determine the TokenType and deserialize accordingly
            if (root.TryGetProperty("TokenType", out var tokenTypeElement))
            {
                var tokenType = tokenTypeElement.GetString();

                if (tokenType == "Url")
                {
                    return JsonSerializer.Deserialize<UrlData>(root.GetRawText(), options);
                }
                else if (tokenType == "Vcard")
                {
                    // Handle Person or Company vCard
                    if (root.TryGetProperty("Title", out _)) // Check if it's a Person vCard
                    {
                        return JsonSerializer.Deserialize<PersonVCardData>(root.GetRawText(), options);
                    }
                    else
                    {
                        return JsonSerializer.Deserialize<CompanyVCardData>(root.GetRawText(), options);
                    }
                }
            }

            throw new JsonException("Unknown TokenType or invalid data.");
        }
    }

    public override void Write(Utf8JsonWriter writer, ITokenData value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
