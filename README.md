# 📒 Contact CLI

> A clean, layered **.NET 8** command-line application for managing contacts — built with a proper architecture separating concerns across Entity, Application, Infrastructure, and Presentation layers.

---

## ✨ Features

- 📇 Manage contacts (Name, Phone, Email, Creation Date)
- 💾 JSON-based persistence via a dedicated repository
- 🏗️ Clean architecture with dependency inversion
- ⚡ Lightweight — no database setup required

---

## 🚀 Getting Started

### Prerequisites

| Tool | Version |
|------|---------|
| [.NET SDK](https://dotnet.microsoft.com/download) | **8.0 or later** |
| Any terminal (PowerShell, CMD, bash) | — |

> You can verify your .NET version with:
> ```bash
> dotnet --version
> ```

---

### ▶️ Run the Project

```bash
# 1. Clone the repository
git clone https://github.com/mohamedahmed2005/Contact_CLI.git

# 2. Navigate into the project
cd Contact_CLI

# 3. Restore dependencies
dotnet restore

# 4. Run the application
dotnet run --project Contact_CLI.csproj
```

---

### 🏗️ Build (optional)

```bash
dotnet build
```

---

## 🗂️ Project Structure

```
Contact_CLI/
│
├── 📄 Contact_CLI.csproj          # Project file (targets net8.0)
│
├── 📁 Entity/                     # Domain models
│   └── Contact.cs                 # Contact entity (Id, Name, Phone, Email, CreationDate)
│
├── 📁 Application_Layer/          # Business logic & abstractions
│   ├── Interfaces/
│   │   └── IContact_Repository.cs # Repository contract / interface
│   └── Services/
│       └── ContactService.cs      # Business logic service
│
├── 📁 Json_Infrastructure/        # Data access implementation
│   └── JsonRepository.cs          # JSON-based implementation of IContact_Repository
│
└── 📁 Presentation_Layer/         # Entry point
    └── Program.cs                 # Main / CLI entry point
```

---

## 🧱 Architecture Overview

This project follows a **clean layered architecture**:

```
┌─────────────────────────────┐
│      Presentation Layer      │  ← CLI (Program.cs) — user interaction
├─────────────────────────────┤
│      Application Layer       │  ← Services & Interfaces — business rules
├─────────────────────────────┤
│     Json Infrastructure      │  ← Repository implementation using JSON
├─────────────────────────────┤
│           Entity             │  ← Core domain models (Contact)
└─────────────────────────────┘
```

| Layer | Responsibility |
|-------|---------------|
| **Entity** | Defines the `Contact` domain model with pure properties |
| **Application Layer** | Holds the `IContact_Repository` interface and `ContactService` business logic |
| **Json Infrastructure** | Implements the repository using JSON file storage |
| **Presentation Layer** | CLI entry point — wires everything together and drives the app |

---

## 📦 Tech Stack

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![JSON](https://img.shields.io/badge/Storage-JSON-F7DF1E?style=for-the-badge&logo=json&logoColor=black)

---

## 👨‍💻 Author

**Mohamed Ahmed** — [@mohamedahmed2005](https://github.com/mohamedahmed2005)

---

> *Built as a assignment showcasing clean architecture principles in .NET and C#.*