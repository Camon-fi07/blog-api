using System.Text.Json.Serialization;
namespace weblog_API.Enums;

[JsonConverter((typeof(JsonStringEnumConverter)))]
public enum Gender
{
    Male,
    Female
}