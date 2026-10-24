using RetailPayments.Api.Models;

namespace RetailPayments.Api.Services;

public interface ISourceSystemAdapter
{
    string SourceSystemName { get; }
    Task<IEnumerable<TransactionDto>> FetchTransactionsAsync();
}

public class TransactionDto
{
    public int TransactionId { get; set; }
    public string? CardNumber { get; set; }
    public string LocationCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty; // Fixed syntax note: standard get/set
    public decimal Amount { get; set; }
    public DateTime TransactionTime { get; set; }
}

public class StoreUptownAdapter : ISourceSystemAdapter
{
    public string SourceSystemName => "Store Uptown";

    public async Task<IEnumerable<TransactionDto>> FetchTransactionsAsync()
    {
        // Mock sample data for Store Uptown
        return await Task.FromResult(new List<TransactionDto>
        {
            new() { TransactionId = 100001, CardNumber = "***1111", LocationCode = "UPTOWN", ProductName = "Wireless Mouse", Amount = 24.99m, TransactionTime = DateTime.UtcNow.AddHours(-2) }
        });
    }
}

public class StoreDowntownAdapter : ISourceSystemAdapter
{
    public string SourceSystemName => "Store Downtown";

    public async Task<IEnumerable<TransactionDto>> FetchTransactionsAsync()
    {
        return await Task.FromResult(new List<TransactionDto>
        {
            new() { TransactionId = 100001, CardNumber = "***0002", LocationCode = "DOWNTOWN", ProductName = "Mechanical Keyboard", Amount = 89.99m, TransactionTime = DateTime.UtcNow.AddHours(-3) }
        });
    }
}

public class OnlineAdapter : ISourceSystemAdapter
{
    public string SourceSystemName => "Online";

    public async Task<IEnumerable<TransactionDto>> FetchTransactionsAsync()
    {
        return await Task.FromResult(new List<TransactionDto>
        {
            new() { TransactionId = 100001, CardNumber = "***4444", LocationCode = "ONLINE", ProductName = "USB-C Docking Station", Amount = 129.95m, TransactionTime = DateTime.UtcNow.AddHours(-4) }
        });
    }
}