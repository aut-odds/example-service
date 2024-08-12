using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace ExampleService.Api.Configurations;

public class SnakeCaseElementNameConvention : ConventionBase, IMemberMapConvention
{
    public void Apply(BsonMemberMap memberMap)
    {
        memberMap.SetElementName(SnakeCase(memberMap.MemberName));
    }

    private string SnakeCase(string input)
    {
        return Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
