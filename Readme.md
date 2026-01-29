# Clipy — Instant Note Sharing Made Simple

[![Built with .NET](https://img.shields.io/badge/Built_with-.NET_10-blue?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Security](https://img.shields.io/badge/Encryption-AES--256-informational)](#security-and-privacy)

**Clipy** is a fast, minimal web app for sharing text securely and effortlessly.  
Create encrypted notes with shareable links — or spin up live, temporary rooms for real-time collaboration.

No accounts. No clutter. Just share what you need, when you need it.

---

## What Is Clipy?

Clipy helps you share text in two simple ways:

- **Secure Notes** — link-based, encrypted, and optionally self-destructing
- **Live Rooms** — real-time shared text that expires automatically

Whether you’re sharing a password, a one-time message, or collaborating live for a few minutes, Clipy keeps things fast, private, and disposable.

---

## Demo

| Live                                                                                                                                                    |                                                                                                                                         |
|---------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|
| <img width="1582" height="954" alt="Home page" src="https://github.com/user-attachments/assets/bd586712-ee58-4826-a392-09f68f18d08e" />                 | <img width="1582" height="954" alt="Add page" src="https://github.com/user-attachments/assets/d226ed30-f879-43f2-8f91-9488a9e549a6" />  |
| <img width="1582" height="954" alt="Add with advanced options" src="https://github.com/user-attachments/assets/11a4a0f8-11e6-4cf8-9fae-f326515fe93b" /> | <img width="1582" height="954" alt="View note" src="https://github.com/user-attachments/assets/42f886f4-4138-425d-a85a-23520e94a4fc" /> |
   

---

## Key Features

###  Secure Notes (Link-Based)

#### 1. Instant Note Sharing
Write a note and instantly get a unique shareable link.  
No sign-up, no setup — just create and share.

#### 2. Self-Destructing Notes
Optionally delete notes automatically after they’re viewed once.  
A confirmation screen prevents accidental deletion.

#### 3. Password Protection
Protect notes with a password. Only users with the correct password can view them.

#### 4. AES-256 Encryption at Rest
All notes are encrypted at rest using **AES-256**.  
Users can optionally provide a custom encryption key, which Clipy safely derives into a strong AES key using **SHA-256**.

#### 5. Custom Codes
Use your own short, memorable code instead of a randomly generated one.

#### 6. Expiration Options
Set notes to expire after a fixed duration or a custom date.  
Expired notes are deleted automatically.

#### 7. Copy-to-Clipboard
Quickly copy note contents with visual feedback.

---

###  Live Rooms (Real-Time)

#### 8. Live Text Sharing
Create a room and share a short room code.  
Everyone in the room sees updates instantly as you type.

#### 9. No Storage, No History
Room content lives only in memory and is never written to disk.

#### 10. Automatic Room Expiry
Rooms automatically expire after inactivity, ensuring nothing lingers longer than necessary.

#### 11. No Login Required
Anyone with the room code can join instantly — perfect for quick collaboration or temporary sharing.

---

## When Should I Use Notes vs Rooms?

| Use Case                | Notes | Rooms |
|-------------------------|-------|-------|
| One-time secrets        | ✅     | ❌     |
| Encrypted storage       | ✅     | ❌     |
| Share via link          | ✅     | ❌     |
| Real-time collaboration | ❌     | ✅     |
| Temporary brainstorming | ❌     | ✅     |
| No persistence          | ❌     | ✅     |

---

## Why Use Clipy?

Clipy is built for speed, simplicity, and privacy.

- No accounts
- No tracking
- No unnecessary features
- Everything expires or deletes when it should

It’s ideal for developers, teams, and anyone who needs to share text without leaving traces.

---

## How It Works

### Secure Notes
1. **Create a Note** — Write your message and choose options (password, expiry, encryption key, delete-after-view).
2. **Share the Link** — Copy the generated URL.
3. **View Securely** — Notes decrypt only when accessed and are deleted based on your settings.

### Live Rooms
1. **Create or Join a Room** — Enter or generate a room code.
2. **Share the Code** — Others join instantly.
3. **Type Together** — Everyone sees updates live.
4. **Room Expires** — Automatically cleaned up after inactivity.

---

## Technology Behind Clipy

- Built with **ASP.NET Core 9**
- **SignalR** for real-time rooms
- **Pico.css** for a clean, responsive UI
- **SQLite** or **SQL Server** for note storage
- AES-256 encryption for notes at rest
- Passwords hashed securely before storage

---

## Security and Privacy

- AES-256 encryption for all stored notes
- Optional user-provided encryption keys
- Passwords are hashed — never stored in plain text
- Expired and self-destructed notes are deleted automatically
- Rooms are in-memory only
- No analytics, ads, or tracking

---

## Contact

For questions, feedback, or feature suggestions:

**Rohan Salunkhe**  
📧 [contact@amrohan.in](mailto:contact@amrohan.in)
