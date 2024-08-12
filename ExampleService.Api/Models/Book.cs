using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ExampleService.Api.Dtos;

namespace ExampleService.Api.Models;

public class Book : BookDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}
