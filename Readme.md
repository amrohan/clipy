# Clipy

**Instant, secure note and file sharing — no accounts, no friction.**

[![Built with .NET](https://img.shields.io/badge/Built_with-.NET_10-blue?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

Clipy is a minimal web app for sharing text (and optional file attachments) securely and temporarily.

Create a note → get a link → share it → it disappears when it should.

No logins. No tracking. No unnecessary complexity.

---

## Why Clipy?

Sometimes you just need to share:

- A password
- A config snippet
- A quick secret
- A temporary file
- A few lines of text in real time

Most tools are either too heavy or not private enough.

Clipy is built to be:

- Fast
- Disposable
- Secure
- Simple

---

## Features

### Secure Notes

- Encrypted at rest using AES-256
- Optional password protection
- Custom short codes
- Expiration (1h, 24h, 7d, or custom date)
- Delete-after-first-view
- Optional file attachment
- Clean shareable links

Notes are encrypted before storage and decrypted only when accessed.

---

### File Attachments

Notes can include a file.

- Stored in a private Backblaze B2 bucket
- Never exposed directly
- Downloaded through the backend
- Deleted when the note expires (if configured)

Designed for temporary sharing — not permanent storage.

---

### Live Rooms

Need real-time collaboration instead?

- Join with a simple room code
- See updates instantly (SignalR)
- In-memory only
- Automatically cleaned up after inactivity
- No persistence, no history

Perfect for quick brainstorming or short-lived sessions.

---

## Notes vs Rooms

| Use Case                | Notes | Rooms |
| ----------------------- | ----- | ----- |
| One-time secret         | ✅    | ❌    |
| Encrypted storage       | ✅    | ❌    |
| File sharing            | ✅    | ❌    |
| Real-time typing        | ❌    | ✅    |
| Temporary collaboration | ❌    | ✅    |
| No persistence          | ❌    | ✅    |

---

## Tech Stack

- .NET 10 (ASP.NET Core Razor Pages)
- Entity Framework Core
- SQLite / SQL Server
- SignalR
- Backblaze B2 (S3-compatible API)
- BCrypt for password hashing
- Pico.css for UI

---

## Security

- AES-256 encryption for stored notes
- Passwords hashed with BCrypt
- Optional user-provided encryption keys
- Private object storage for files
- Automatic cleanup of expired content
- No analytics, no tracking, no ads

Clipy is designed to leave as little behind as possible.

---

## Running Locally

### 1. Clone the repo

```bash
git clone https://github.com/yourusername/clipy.git
cd clipy
```

### 2. Create your config file

```bash
cp appsettings.example.json appsettings.json
```

Edit `appsettings.json` and provide:

- Database connection string
- Backblaze B2 credentials
- Encryption key

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=clipy.db"
  },
  "B2": {
    "ServiceUrl": "https://s3.us-west-004.backblazeb2.com",
    "BucketName": "your-bucket-name",
    "KeyId": "your-key-id",
    "ApplicationKey": "your-application-key"
  },
  "Encryption": {
    "DefaultKey": "your-strong-default-key"
  }
}
```

`appsettings.json` is intentionally ignored by Git.
Use `appsettings.example.json` as a template.

### 3. Run the app

```bash
dotnet build
dotnet run
```

Open:

```
http://localhost:5000
```

---

## Environment Variables (Optional)

For production, you can override secrets using environment variables:

Example:

```
B2__KeyId
B2__ApplicationKey
```

Double underscore maps to nested config keys.

---

## Philosophy

Clipy isn’t trying to replace cloud storage or build a social platform.

It’s built for:

- Developers
- Small teams
- Quick, private sharing
- Temporary collaboration
- Ephemeral workflows

Create. Share. Done.

---

## License

MIT

---

## Author

Rohan Salunkhe
[contact@amrohan.in](mailto:contact@amrohan.in)
