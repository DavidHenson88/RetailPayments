using Microsoft.EntityFrameworkCore;
using RetailPayments.Api.Data;
using RetailPayments.Api.Models;

namespace RetailPayments.Api.Services;

public class TransactionImportService
{
    private readonly AppDbContext _context;
    private readonly IEnumerable<ISourceSystemAdapter> _adapters;

    public TransactionImportService(AppDbContext context, IEnumerable<ISourceSystemAdapter> adapters)
    {
        _context = context;
        _adapters = adapters;
    }

    public async Task ImportTransactionsAsync()
    {
        var importTime = DateTime.UtcNow;

        foreach (var adapter in _adapters)
        {
            var incomingSnapshot = await adapter.FetchTransactionsAsync();
            var sourceSystemName = adapter.SourceSystemName;

            var existingTransactions = await _context.Transactions
                .Where(t => t.SourceSystem == sourceSystemName)
                .ToListAsync();

            var incomingKeys = incomingSnapshot.Select(i => i.TransactionId).ToHashSet();

            // 1. Process Inserts and Updates
            foreach (var incoming in incomingSnapshot)
            {
                var existing = existingTransactions.FirstOrDefault(t => t.TransactionId == incoming.TransactionId);

                if (existing == null)
                {
                    var newTx = new Transaction
                    {
                        SourceSystem = sourceSystemName,
                        TransactionId = incoming.TransactionId,
                        CardNumber = incoming.CardNumber,
                        LocationCode = incoming.LocationCode,
                        ProductName = incoming.ProductName,
                        Amount = incoming.Amount,
                        TransactionTime = incoming.TransactionTime,
                        Status = "Active",
                        LastSyncedAt = importTime
                    };
                    _context.Transactions.Add(newTx);
                    RecordAudit(sourceSystemName, incoming.TransactionId, "Insert", null, "New Transaction", importTime);
                }
                else
                {
                    // Field-level change detection & audit recording
                    CheckAndUpdateField(existing, "ProductName", existing.ProductName, incoming.ProductName, () => existing.ProductName = incoming.ProductName, importTime);
                    CheckAndUpdateField(existing, "Amount", existing.Amount.ToString(), incoming.Amount.ToString(), () => existing.Amount = incoming.Amount, importTime);
                    CheckAndUpdateField(existing, "LocationCode", existing.LocationCode, incoming.LocationCode, () => existing.LocationCode = incoming.LocationCode, importTime);

                    existing.LastSyncedAt = importTime;

                    if (existing.Status == "Revoked")
                    {
                        existing.Status = "Active";
                        RecordAudit(sourceSystemName, incoming.TransactionId, "Status", "Revoked", "Active", importTime);
                    }
                }
            }

            // 2. Revocation Rule: Missing from snapshot within 24 hours of TransactionTime
            foreach (var existing in existingTransactions)
            {
                if (!incomingKeys.Contains(existing.TransactionId) && existing.Status != "Revoked")
                {
                    var timeSinceTransaction = importTime - existing.TransactionTime;
                    if (timeSinceTransaction.TotalHours <= 24)
                    {
                        existing.Status = "Revoked";
                        RecordAudit(sourceSystemName, existing.TransactionId, "Status", "Active", "Revoked", importTime);
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    private void CheckAndUpdateField(Transaction tx, string fieldName, string? oldValue, string? newValue, Action updateAction, DateTime time)
    {
        if (oldValue != newValue)
        {
            updateAction();
            _context.TransactionAudits.Add(new TransactionAudit
            {
                SourceSystem = tx.SourceSystem,
                TransactionId = tx.TransactionId,
                Action = "Update",
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                Timestamp = time
            });
        }
    }

    private void RecordAudit(string sourceSystem, int txId, string action, string? oldVal, string? newVal, DateTime time)
    {
        _context.TransactionAudits.Add(new TransactionAudit
        {
            SourceSystem = sourceSystem,
            TransactionId = txId,
            Action = action,
            FieldName = action == "Insert" ? "Record" : "Status",
            OldValue = oldVal,
            NewValue = newVal,
            Timestamp = time
        });
    }
}