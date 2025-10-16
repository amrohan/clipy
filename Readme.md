# Clipy – Instant Notes Sharing

## Overview

**Clipy** is a lightweight web application that allows users to quickly create, share, and view notes via unique URLs. Users can optionally choose a custom code/slug for their notes or allow Clipy to generate one automatically. Notes can be configured to **delete after being viewed** or be **password protected** for extra privacy.

Built with:

- **ASP.NET Core 9 Razor Pages** – server-side rendering and page handling
- **Pico.css** – lightweight, modern, responsive UI
- **Entity Framework Core 9** – database operations
- **SQLite / SQL Server** – database backend

---

## Features

- **Instant Note Sharing** – Create a note and get a unique URL immediately.
- **Custom Codes** – Optional user-defined URL codes.
- **Self-Destructing Notes** – Notes can be marked to delete automatically after the first view.
- **Password-Protected Notes** – Protect sensitive notes with a password before viewing.
- **Copy-to-Clipboard** – Quickly copy note content with a single click; icon shows success (checkmark) or error (cross).
- **Confirmation Before Viewing** – Prevent accidental deletion by requiring a confirmation before viewing self-destructing notes.
- **Lightweight UI** – Clean and responsive interface with Pico.css.

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- A database supported by EF Core (SQLite or SQL Server recommended)

### Setup

1. Clone the repository:

```bash
git clone https://github.com/amrohan/clipy.git
cd clipy
```

2. Configure the database connection in `appsettings.json`. For example, using SQLite:

```json
{
  "ConnectionStrings": {
    "ClipyDb": "Data Source=clipy.db"
  }
}
```

3. Apply database migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run the application:

```bash
dotnet run
```

Open your browser at `https://localhost:5001` (or the configured port).

---

## Usage

1. **Add Note**: Navigate to `/AddNote`, enter your note, optional custom code, password (optional), and optionally check “Delete after view”.
2. **Share Note**: Copy the generated URL and share it with anyone.
3. **View Note**: Anyone with the URL can view the note.

   - If password protected, users must enter the correct password.
   - If “Delete after view” is enabled, the note will be removed after the first view.
   - Users see a **confirmation page** for self-destructing notes to avoid accidental deletion.
   - A **copy-to-clipboard button** lets users quickly copy note content with visual feedback (checkmark for success, cross for failure).

4. **Landing Page**: Provides an overview of the app and quick access to add a note.

---

## Database Model

**Note Table**

| Column          | Type   | Description                            |
| --------------- | ------ | -------------------------------------- |
| Id              | int    | Primary key                            |
| Content         | string | Note content                           |
| Code            | string | Unique code or slug                    |
| PasswordHash    | string | Hashed password (nullable)             |
| DeleteAfterView | bool   | If true, note deletes after first view |
| Viewed          | bool   | Indicates if the note was viewed       |
| IsActive        | bool   | If false, the note is inactive/deleted |

---

## Technologies Used

- **ASP.NET Core 9 Razor Pages** – Web framework
- **Pico.css** – Minimal, modern CSS framework
- **Entity Framework Core 9** – ORM for database interactions
- **SQLite / SQL Server** – Database storage

---

## Contributing

Contributions are welcome!

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/YourFeature`
3. Commit changes: `git commit -m "Add feature"`
4. Push to branch: `git push origin feature/YourFeature`
5. Open a Pull Request.

---

## License

This project is licensed under the MIT License. See the LICENSE file for details.

---

## Contact

For questions or issues, contact **Rohan Salunkhe** at [contact@amrohan.in](mailto:contact@amrohan.in).
