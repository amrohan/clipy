"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/roomHub")
    .build();

let roomCode = "";
let isActivelyTyping = false;
let typingTimer = null;

const joinView = document.getElementById("joinView");
const roomView = document.getElementById("roomView");
const activeRoom = document.getElementById("activeRoom");
const input = document.getElementById("input");
const copyBtn = document.getElementById("copyBtn");
const roomInput = document.getElementById("roomCode");
const validHelper = document.getElementById("valid-helper");
const typingHint = document.getElementById("typing-indicator");

function debounce(fn, delay = 250) {
    let t;
    return (...args) => {
        clearTimeout(t);
        t = setTimeout(() => fn(...args), delay);
    };
}

connection.on("ReceiveMessage", msg => {
    input.value = msg;
});

connection.on("ReceiveCursor", pos => {
    if (isActivelyTyping) return;
    input.setSelectionRange(pos, pos);
});

connection.on("UserTyping", () => {
    typingHint.hidden = false;
});

connection.on("UserStoppedTyping", () => {
    typingHint.hidden = true;
});

connection.on("RoomExpired", () => {
    alert("⏳ Room expired after 5 minutes of inactivity");
    exitRoom();
});

document.addEventListener("DOMContentLoaded", async () => {
    await connection.start();
});

/* -------------------------------
   Debounced senders
-------------------------------- */
const sendMessage = debounce(value => {
    if (!roomCode) return;
    connection.invoke("SendMessage", roomCode, value);
});

const sendCursor = debounce(() => {
    if (!roomCode) return;
    connection.invoke("CursorMove", roomCode, input.selectionStart);
}, 100);

input.addEventListener("input", e => {
    isActivelyTyping = true;

    sendMessage(e.target.value);
    connection.invoke("Typing", roomCode);

    clearTimeout(typingTimer);
    typingTimer = setTimeout(() => {
        isActivelyTyping = false;
        connection.invoke("StopTyping", roomCode);
    }, 500);
});
input.addEventListener("keyup", sendCursor);
input.addEventListener("click", sendCursor);

document.getElementById("joinBtn").onclick = async () => {
    const code = getCode();
    if (!code) return;
    await joinRoom(code);
};

document.getElementById("createBtn").onclick = async () => {
    const code = getCode();
    if (!code) return;

    const exists = await connection.invoke("RoomExists", code);
    if (exists) {
        roomInput.setAttribute("aria-invalid", "true");
        validHelper.innerText = "Room already exists. Want to join instead?";
        return;
    }

    clearValidation();
    await joinRoom(code);
};

document.getElementById("exitBtn").onclick = exitRoom;

function getCode() {
    clearValidation();
    return roomInput.value.trim().toUpperCase();
}

async function joinRoom(code) {
    roomCode = code;
    await connection.invoke("JoinRoom", roomCode);

    joinView.hidden = true;
    roomView.hidden = false;

    activeRoom.innerText = roomCode;
    typingHint.hidden = true;
    input.value = "";
    input.focus();
}

function exitRoom() {
    roomCode = "";
    joinView.hidden = false;
    roomView.hidden = true;

    input.value = "";
    typingHint.hidden = true;
    clearValidation();
}

function clearValidation() {
    roomInput.removeAttribute("aria-invalid");
    validHelper.innerText = "";
}

async function addToClipboard() {
    const copyText = document.getElementById("input");
    var textToCopy = copyText.value;

    try {
        await navigator.clipboard.writeText(textToCopy);
        copyBtn.innerText = "Copied!";
        setTimeout(() => { copyBtn.innerText = "Copy"; }, 1000);

    } catch (err) {
        alert(err);
    }
}

roomInput.addEventListener("input", clearValidation);
copyBtn.addEventListener("click", addToClipboard);
