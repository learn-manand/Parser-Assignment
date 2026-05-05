using Parser.API.Models;

namespace Parser.API.Services
{
    public interface IParserService
    {
        ParseResponse Parse(string input);
    }
}
