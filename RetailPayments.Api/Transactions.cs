namespace RetailPayments.Api.Models;

public class Transaction
{
    public string SourceSystem { get; set; } = string.Empty;
    public int TransactionId { get; set; }
    public string? CardNumber { get; set; }
    public string LocationCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime TransactionTime { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;
}

public class TransactionAudit
{
    public int Id { get; set; }
    public string SourceSystem { get; set; } = string.Empty;
    public int TransactionId { get; set; }
    public string Action { get; set; } = string.Empty; // Insert, Update, Revoke
    public string? FieldName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}