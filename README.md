# Retail Payment Reconciliation System

A full-stack enterprise retail payment reconciliation platform built with **ASP.NET 10 Web API** (Backend) and **Angular** (Frontend). Designed to ingest transactions from multiple disparate source systems, enforce composite key constraints, track field-level audits, apply business rules (such as 24-hour revocation windows), and present a real-time reconciliation dashboard.

---

## Tech Stack
* **Backend:** ASP.NET 10 Web API, Entity Framework Core (EF Core), SQLite
* **Frontend:** Angular (Standalone Components, TypeScript, RxJS), HTML5/CSS3
* **Development Tooling:** .NET CLI, Angular CLI, Swagger / OpenAPI

---

## A. Build Instructions

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/) installed
* [Node.js (LTS version)](https://nodejs.org/) installed

### 1. Build the Backend (.NET API)
Navigate to the API project directory and build the solution:
```bash
cd RetailPayments.Api
dotnet build