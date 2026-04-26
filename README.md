# 📧 MailSystem (ASP.NET Core)

A modular, scalable mail system inspired by modern email platforms (like Gmail), built using **ASP.NET Core Web API** and **Clean Architecture principles**.

---

## 🚀 Project Status

✅ Initial architecture setup completed
✅ Solution and project structure created
✅ Layered architecture (API, Application, Domain, Infrastructure, Persistence)
✅ Dependency flow configured
✅ Core packages installed
✅ Folder structure organized for scalable development

---

## 🧱 Architecture Overview

This project follows **Clean Architecture + Layered Design**:

```
Client → API → Application → Domain
                    ↓
        Infrastructure + Persistence
```

### 🔹 Layers

* **API (Presentation Layer)**

  * Handles HTTP requests and responses
  * Contains controllers, middleware, DTOs

* **Application (Business Logic Layer)**

  * Contains use cases (commands & queries)
  * Defines interfaces (abstractions)

* **Domain (Core Layer)**

  * Contains entities, enums, value objects
  * Pure business logic (no external dependencies)

* **Infrastructure (External Services)**

  * Implements services like authentication, storage, etc.

* **Persistence (Database Layer)**

  * Handles database operations using EF Core

---

## 📁 Project Structure

```
MailSystem/
│
├── MailSystem.sln
│
├── src/
│   ├── MailSystem.API
│   ├── MailSystem.Application
│   ├── MailSystem.Domain
│   ├── MailSystem.Infrastructure
│   └── MailSystem.Persistence
│
└── tests/
    ├── MailSystem.UnitTests
    └── MailSystem.IntegrationTests
```

---

## 🔗 Project Dependencies

Clean dependency direction is maintained:

```
API → Application
API → Infrastructure
API → Persistence

Application → Domain

Infrastructure → Application

Persistence → Application
Persistence → Domain
```

🚫 Domain does not depend on any other layer (kept independent)

---

## 📦 Installed Packages

### 🔹 API Layer

* JWT Authentication
* Swagger (API Documentation)
* Serilog (Logging)
* FluentValidation (Request Validation)

### 🔹 Persistence Layer

* Entity Framework Core
* SQL Server Provider
* EF Core Design Tools

### 🔹 Infrastructure Layer

* BCrypt (Password Hashing)

---

## 📂 Internal Folder Structure

### Domain

* Common/
* Entities/
* Enums/
* ValueObjects/
* Events/

### Application

* Abstractions/
* Common/
* Behaviors/
* Features/

### Persistence

* Context/
* Configurations/
* Repositories/

### Infrastructure

* Authentication/
* CurrentUser/
* Storage/
* Notifications/
* BackgroundJobs/
* Time/

### API

* Controllers/
* DTOs/
* Middleware/
* Extensions/
* Common/
* Hubs/

---

## 🧪 Testing

* **Unit Tests** → Business logic testing
* **Integration Tests** → End-to-end API testing

---

## 🔮 Planned Features (Next Steps)

* Custom JWT Authentication System
* Mailbox (Inbox, Sent, Drafts)
* Message Sending & Threading
* Attachments Handling
* Search Functionality
* Notifications (Real-time)
* Background Jobs (Hangfire)
* Email Integration (SMTP)

---

## 🛠️ Tech Stack

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Serilog Logging
* FluentValidation
* xUnit Testing

---

## 📌 Notes

* The project is currently in **initial setup phase**
* Focus has been on **clean architecture and scalability**
* Feature implementation will follow next

---

## 👨‍💻 Author

**Hussam Ishtiaq**

---

## ⭐ Goal

To build a **production-grade, scalable mail system** using best practices in ASP.NET Core and backend architecture.
