using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailPayments.Api.Data;
using RetailPayments.Api.Models;
using RetailPayments.Api.Services;

namespace RetailPayments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TransactionImportService _importService;

    public TransactionsController(AppDbContext context, TransactionImportService importService)
    {
        _context = context;
        _importService = importService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        return await _context.Transactions
            .OrderByDescending(t => t.TransactionTime)
            .ToListAsync();
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportTransactions()
    {
        await _importService.ImportTransactionsAsync();
        return Ok(new { message = "Transaction import and reconciliation executed successfully." });
    }
}