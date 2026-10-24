# Retail Payment Reconciliation System

## Overview
A local-only retail payment reconciliation system featuring an **ASP.NET 10** backend API and an **Angular v22** frontend client. It ingests card transactions from multiple mock source systems (Store Uptown, Store Downtown, Online), reconciles records, detects field-level changes and revoked transactions, maintains an audit history, and supports idempotent execution[cite: 1, 2, 4].

---

## Build & Run Instructions

### Prerequisites
* .NET 10 SDK
* Node.js (v24+) and Angular CLI

### 1. Run the Backend API
```powershell
cd RetailPayments.Api
dotnet run