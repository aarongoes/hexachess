"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/moveHub").configureLogging(signalR.LogLevel.None).build();

connection.on("BoardUpdate", function () {
    updateHexagons();
    LoadStats();
});

connection.on("PlayerJoined", function () {
    $('#invite-second-player-dialog').collapse("hide");
    if (player != 2){
        generateHexagons();
        LoadStats();
        LoadPlayers();
    }
});

$(document).ready(function () {
    connection.start().then(function(){
        joinRoom();
    }).catch(function (err) {
        return console.error(err.toString());
    });
});

function joinRoom(){
    connection.invoke("JoinRoom", gameToken).catch(function (err) {
        return console.error(err.toString());
    });
}