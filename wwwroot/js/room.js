"use strict"

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/roomHub")
  .build();

let roomCode = "";

const joinView   = document.getElementById("joinView");
const roomView   = document.getElementById("roomView");
const activeRoom = document.getElementById("activeRoom");
const input      = document.getElementById("input");
const roomInput  = document.getElementById("roomCode");
const validHelper = document.getElementById("valid-helper");
const joinButton  = document.getElementById("joinBtn");

connection.on("ReceiveMessage", msg => {
  input.value = msg;
});

connection.on("RoomExpired", () => {
  alert("⏳ Room expired after 5 minutes of inactivity");
  exitRoom();
});

document.addEventListener("DOMContentLoaded", async () => {
  await connection.start();
});

joinButton.onclick = async () => {
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
    roomInput.focus();
    return;
  }

  roomInput.removeAttribute("aria-invalid");
  validHelper.innerText = "";
  await joinRoom(code);
};

input.addEventListener("input", e => {
  if (!roomCode) return;
  connection.invoke("SendMessage", roomCode, e.target.value);
});

document.getElementById("exitBtn").onclick = exitRoom;

function getCode() {
  roomInput.removeAttribute("aria-invalid");
  validHelper.innerText = "";
  return roomInput.value.trim().toUpperCase();
}

async function joinRoom(code) {
  roomCode = code;

  await connection.invoke("JoinRoom", roomCode);

  joinView.hidden = true;
  roomView.hidden = false;

  activeRoom.innerText = roomCode;
  input.value = "";
  input.focus();
}

function exitRoom() {
  roomCode = "";

  joinView.hidden = false;
  roomView.hidden = true;

  input.value = "";

  roomInput.removeAttribute("aria-invalid");
}

roomInput.addEventListener("input", () => {
  roomInput.removeAttribute("aria-invalid");
  validHelper.innerText = "";
});
