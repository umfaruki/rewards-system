using Domain.Entities.Customers;
using Domain.Entities.Transactions;
using Newtonsoft.Json;

namespace Infrastructure.Helper;

public static class MockDataHelper
{
    public static List<T> GetMockData<T>()
    {
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"DataSeed/{typeof(T).Name}.json");        
        var jsonData = File.ReadAllText(fullPath);
        var parsedData = JsonConvert.DeserializeObject<List<T>>(jsonData);
        return parsedData ?? new List<T>();        
    }
}
