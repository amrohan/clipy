"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/roomHub")
    .withAutomaticReconnect()
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

const joinBtn = document.getElementById("joinBtn");
const createBtn = document.getElementById("createBtn");
const exitBtn = document.getElementById("exitBtn");


function debounce(fn, delay = 250) {
    let t;
    return (...args) => {
        clearTimeout(t);
        t = setTimeout(() => fn(...args), delay);
    };
}

function clearValidation() {
    roomInput?.removeAttribute("aria-invalid");
    if (validHelper) validHelper.innerText = "";
}

function getCode() {
    clearValidation();
    return roomInput?.value.trim().toUpperCase();
}


connection.on("ReceiveMessage", msg => {
    if (!input) return;
    input.value = msg;
});

connection.on("ReceiveCursor", pos => {
    if (!input || isActivelyTyping) return;
    input.setSelectionRange(pos, pos);
});

connection.on("UserTyping", () => {
    if (typingHint) typingHint.hidden = false;
});

connection.on("UserStoppedTyping", () => {
    if (typingHint) typingHint.hidden = true;
});

connection.on("RoomExpired", () => {
    alert("⏳ Room expired after 5 minutes of inactivity");
    exitRoom();
});


document.addEventListener("DOMContentLoaded", async () => {

    try {
        await connection.start();
    } catch (err) {
        console.error("SignalR connection failed", err);
    }

});


const sendMessage = debounce(value => {
    if (!roomCode || connection.state !== "Connected") return;
    connection.invoke("SendMessage", roomCode, value);
});

const sendCursor = debounce(() => {
    if (!roomCode || connection.state !== "Connected") return;
    connection.invoke("CursorMove", roomCode, input.selectionStart);
}, 100);


if (input) {

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

}


joinBtn?.addEventListener("click", async () => {

    const code = getCode();
    if (!code) return;

    await joinRoom(code);

});

createBtn?.addEventListener("click", async () => {

    const code = getCode();
    if (!code) return;

    const exists = await connection.invoke("RoomExists", code);

    if (exists) {

        roomInput.setAttribute("aria-invalid", "true");
        validHelper.innerText = "Room already exists. Try joining instead.";

        return;
    }

    clearValidation();
    await joinRoom(code);

});


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

exitBtn?.addEventListener("click", exitRoom);


async function addToClipboard() {

    if (!input) return;

    const textToCopy = input.value;

    if (!textToCopy) return;

    try {

        if (navigator.clipboard && window.isSecureContext) {

            await navigator.clipboard.writeText(textToCopy);

        } else {

            const textarea = document.createElement("textarea");
            textarea.value = textToCopy;
            textarea.style.position = "fixed";
            textarea.style.left = "-9999px";

            document.body.appendChild(textarea);
            textarea.select();

            document.execCommand("copy");

            textarea.remove();
        }

        if (copyBtn) {
            copyBtn.innerText = "Copied!";
            setTimeout(() => copyBtn.innerText = "Copy", 1000);
        }

    } catch (err) {
        console.error("Copy failed", err);
    }

}

copyBtn?.addEventListener("click", addToClipboard);

roomInput?.addEventListener("input", clearValidation);
