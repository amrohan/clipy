# Clipy — Instant Note Sharing Made Simple

[![Built with .NET](https://img.shields.io/badge/Built_with-.NET_9-blue?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Security](https://img.shields.io/badge/Encryption-AES--256-informational)](#security-and-privacy)

**Clipy** is a fast, minimal web app that lets you share notes instantly through unique links.  
No accounts, no clutter just type a note, generate a link, and share it with anyone.


## What Is Clipy?

Clipy makes it effortless to send short notes, private messages, or temporary text online.  
Each note gets its own secure URL, which you can share directly. Notes can even be set to disappear after being viewed once, ensuring complete privacy.

It’s perfect for sharing quick thoughts, confidential info, or one-time secrets like passwords.



## Demo

| | |
|---|---|
| <img width="1582" height="954" alt="Screenshot 1" src="https://github.com/user-attachments/assets/831b1dfb-5d66-4911-9528-a020a4b6fabc" /> | <img width="1582" height="954" alt="Screenshot 2" src="https://github.com/user-attachments/assets/d226ed30-f879-43f2-8f91-9488a9e549a6" /> |
| <img width="1582" height="954" alt="Screenshot 3" src="https://github.com/user-attachments/assets/11a4a0f8-11e6-4cf8-9fae-f326515fe93b" /> | <img width="1582" height="954" alt="Screenshot 4" src="https://github.com/user-attachments/assets/42f886f4-4138-425d-a85a-23520e94a4fc" /> |



## Key Features

### 1. Instant Note Sharing
Write your note and instantly get a unique shareable link. No sign-up, no setup — just create and share.

### 2. Self-Destructing Notes
You can choose to delete notes automatically after they’re viewed once.  
Before viewing, Clipy shows a confirmation screen to prevent accidental deletion.

### 3. Password Protection
Protect sensitive notes with a password. Only users with the correct password can unlock and read the note.

### 4. AES-256 Encryption at Rest
All notes are securely encrypted at rest using AES-256 encryption, ensuring that even if the database is accessed, note contents remain unreadable.  
Users can also optionally provide their own custom encryption key for additional privacy.  
Clipy automatically derives a strong 256-bit AES key from any provided key using SHA-256 hashing.

### 5. Custom Codes
Want a cleaner or memorable link? Add your own short code (slug) instead of the random one generated automatically.

### 6. Expiration Options
Choose how long a note should stay active — for an hour, a day, a week, or a custom expiry date. Notes automatically expire once their time runs out.

### 7. Copy-to-Clipboard
Each note comes with a simple “copy” button that lets you instantly copy the text to your clipboard.  
You’ll get visual feedback when it’s successful.

### 8. Clean and Lightweight Interface
Built with **Pico.css**, the UI is fast, responsive, and distraction-free — optimized for both desktop and mobile.


## Why Use Clipy?

Clipy is designed for speed, simplicity, and privacy.  
It’s a handy tool for developers, teams, or anyone who needs to quickly share text without leaving traces or signing up for anything.


## How It Works

1. **Create a Note** – Write your message and choose any extra options (password, expiry, encryption key, or delete-after-view).  
2. **Share the Link** – Copy the generated link and send it to anyone.  
3. **View Securely** – The recipient can open the link to view the note.  
   - If it’s password-protected, they’ll be prompted to enter the password.  
   - If it’s set to self-destruct, it will automatically delete after viewing.  
   - All note contents remain encrypted at rest until they are viewed.

That’s it — no accounts, no history, just simple note sharing.



## Technology Behind Clipy

- Built using **ASP.NET Core 9**
- Uses **Pico.css** for a minimal, modern look
- Stores notes securely in **SQLite** or **SQL Server**
- AES-256 encryption for all notes at rest
- Optional user-provided encryption key
- Passwords are safely hashed before saving


## Security and Privacy

- Notes are encrypted at rest using **AES-256**.  
- User-provided encryption keys are hashed securely with **SHA-256**.  
- Passwords are hashed before storage — never stored in plain text.  
- Self-destructing and expired notes are deleted automatically.  
- No analytics, ads, or tracking scripts.


## Contact

For questions, feedback, or feature suggestions, reach out to:

**Rohan Salunkhe**  
📧 [contact@amrohan.in](mailto:contact@amrohan.in)
