using System.Text.Json;

namespace Entra.EventHandlers.TestHelpers;

public static class TestUtils
{
    public static async Task<T> ReadJson<T>(Stream body)
    {
        body.Position = 0;
        return (await JsonSerializer.DeserializeAsync<T>(body))!;
    }
}
