class LiftedPiece {
    constructor(possibleMoves, pieceType, player) {
        this.possibleMoves = possibleMoves;
        this.pieceType = pieceType;
        this.player = player;
    }

    link(element) {
        this.element = element;
        $(element).addClass("lifted-piece");

        switch (this.pieceType) {
            case 0:
                $(element).addClass("none");
                break;
            case 1:
                $(element).addClass("pawn");
                break;
            case 2:
                $(element).addClass("rook");
                break;
            case 3:
                $(element).addClass("knight");
                break;
            case 4:
                $(element).addClass("bishop");
                break;
            case 5:
                $(element).addClass("queen");
                break;
            case 6:
                $(element).addClass("king");
                break;
        }

        if (this.player === 1) {
            $(element).addClass("player1");
        } else if (this.player === 2){
            $(element).addClass("player2");
        }

        $("body").append(element);
    }
}

class ChessMove {
    constructor(oldPosition, newPosition) {
        this.oldX = oldPosition.x * 10;
        this.oldY = oldPosition.y * 10;
        this.newX = newPosition.x * 10;
        this.newY = newPosition.y * 10;
    }
}

function UpdateLiftedPiece(selectedHex) {
    liftedPieceObject = new LiftedPiece(selectedHex.piece.possibleMoves, selectedHex.piece.pieceType, selectedHex.piece.player);
}

function UpdateLastTarget(selectedHex, selectedDiv) {
    lastTargetObject = selectedHex;
    lastTargetDiv = selectedDiv;
}

function RemoveClasses(target) {
    $(target).removeClass("bishop");
    $(target).removeClass("king");
    $(target).removeClass("knight");
    $(target).removeClass("pawn");
    $(target).removeClass("queen");
    $(target).removeClass("rook");
    $(target).removeClass("none");
    $(target).removeClass("player1");
    $(target).removeClass("player2");
}

function RotateBoard(){
    if (gameMode == 2){
        if (turn == 1){
            document.getElementById("nextplayer").innerHTML = player1 + "'s " + turnText;
        }
        else{
            document.getElementById("nextplayer").innerHTML = player2 + "'s " + turnText;
        }
    }
    else{
        if (turn == 1){
            document.getElementById("nextplayer").innerHTML = player1Turn + " " + turnText;
        }
        else{
            document.getElementById("nextplayer").innerHTML = player2Turn + " " + turnText;
        }
    }
    if (gameMode != 1) {
        if (player == 2) {
            $(".hex-grid").addClass("rotate-home");
            $(".game-canvas").addClass("rotate");
            $(".hex").addClass("rotate-hex");
            $(".game-canvas").removeClass("rotate-reversed");
            $(".hex").removeClass("rotate-hex-reversed");
        }
    }    
    else{
        if (turn === 1){
            $(".hex-grid").removeClass("rotate-home");
            $(".game-canvas").addClass("animate-rotate-reversed");
            $(".hex").addClass("animate-rotate-hex-reversed");
            $(".game-canvas").removeClass("animate-rotate");
            $(".hex").removeClass("animate-rotate-hex");
        }
        else{
            $(".hex-grid").addClass("rotate-home");
            $(".game-canvas").addClass("animate-rotate");
            $(".hex").addClass("animate-rotate-hex");
            $(".game-canvas").removeClass("animate-rotate-reversed");
            $(".hex").removeClass("animate-rotate-hex-reversed");
        }        
    }
}

function LoadStats(){
    $.post("/loadstats", {
            gameToken: gameToken
        }, {
            dataType: "json",
            traditional: true
        }
    ).then(data =>{
        $("#moves").html(data);
    });
}

function LoadPlayers(){
    $.post("/loadplayersinfo", {
            gameToken: gameToken
        }, {
            dataType: "json",
            traditional: true
        }
    ).then(data =>{
        $("#players").html(data);
    });
}

function MovePiece(oldPosition, newPosition) {
    $.post("/movepiece", {
            chessMove: new ChessMove(oldPosition, newPosition),
            gameToken: gameToken
        }, {
            dataType: "json",
            traditional: true
        }
    ).then(() => {
        SaveThumbnail();
    });
}

$(function () {
    $("#btnback").click(function () {
        window.location.href = "/";
    })
});

var oldpiece= null;
var newpiece = null;
var mousedown = false;
var lastTargetObject = null;
var lastTargetDiv = null;
var liftedPieceObject = null;
var player = 0;
var orientation;
var turn;
var player1Turn;
var player2Turn;
var turnText;

$(document).ready(function () {
    $("body").css({"overflow": "hidden", "position": "fixed", "width": "100%", "height": "100%"});
    $("#share-url").val(window.location.origin + "/game/" + gameToken + "/join");
    
    LoadPlayers();
    
    if($(window).height() > $(".game-canvas").width()){
        $(".game-canvas").addClass("portrait");
        $(".game-canvas").removeClass("landscape");
        orientation = false;
    }
    else{
        $(".game-canvas").addClass("landscape");
        $(".game-canvas").removeClass("portrait");
        orientation = true;
    }
    
    $(window).resize(function () {
        if($(".game-canvas").height() > $(".game-canvas").width() && orientation){
            $(".game-canvas").addClass("portrait");
            $(".game-canvas").removeClass("landscape");
            orientation = !orientation;
        }
        else if ($(".game-canvas").height() < $(".game-canvas").width() && !orientation){
            $(".game-canvas").addClass("landscape");
            $(".game-canvas").removeClass("portrait");
            orientation = !orientation;
        }
    });

    $(".game-canvas").on("touchstart", ".hex-base", (e) => {
        var selectedHex = board.getDataFromElement(e.currentTarget.firstChild);
        oldpiece = selectedHex.piece;
        if (selectedHex.piece.player === turn && turn === player || selectedHex.piece.player === turn && gameMode == 1 && player == 1){
            for (let hex in selectedHex.piece.possibleMoves) {
                var value = selectedHex.piece.possibleMoves[hex];
                var hexElement = hexes.find(h => h.x == value.newX && h.y == value.newY).element;
                $(hexElement).css("opacity", "0.5");
            }

            UpdateLiftedPiece(selectedHex);
            if (selectedHex.piece.pieceType !== 0) {
                liftedPieceObject.link(document.createElement("div"));
                $(".lifted-piece").css({ left: e.originalEvent.changedTouches[0].pageX, top: e.originalEvent.changedTouches[0].pageY });
                if (orientation){
                    $(".lifted-piece").addClass("landscape")
                }
                else{
                    $(".lifted-piece").addClass("portrait")
                }
                UpdateLastTarget(selectedHex, e.currentTarget.firstChild.firstChild.firstChild);
                selectedHex.assignPiece(new ChessPiece(0, null));
                selectedHex.linkPiece(e.currentTarget.firstChild.firstChild.firstChild);
                $(".background").addClass("grabbing");
                $(".game-canvas").css("cursor", "not-allowed");
            }
        }
    });

    $(".game-canvas").on("mousedown", ".hex-base", (e) => {
        var selectedHex = board.getDataFromElement(e.currentTarget.firstChild);
        oldpiece = selectedHex.piece;
        if (selectedHex.piece.player === turn && turn === player || selectedHex.piece.player === turn && gameMode == 1 && player == 1){
            for (let hex in selectedHex.piece.possibleMoves) {
                var value = selectedHex.piece.possibleMoves[hex];
                var hexElement = hexes.find(h => h.x == value.newX && h.y == value.newY).element;
                $(hexElement).css("opacity", "0.5");
            }

            UpdateLiftedPiece(selectedHex);
            if (selectedHex.piece.pieceType !== 0) {
                liftedPieceObject.link(document.createElement("div"));
                $(".lifted-piece").css({ left: e.pageX, top: e.pageY });
                if (orientation){
                    $(".lifted-piece").addClass("landscape")
                }
                else{
                    $(".lifted-piece").addClass("portrait")
                }
                UpdateLastTarget(selectedHex, e.currentTarget.firstChild.firstChild.firstChild);
                selectedHex.assignPiece(new ChessPiece(0, null));
                selectedHex.linkPiece(e.currentTarget.firstChild.firstChild.firstChild);
                $(".background").addClass("grabbing");
                $(".game-canvas").css("cursor", "not-allowed");
            }
        }
    });

    $(".body-content").mouseup(function (e) {
        if (liftedPieceObject != null) {
            if (liftedPieceObject.pieceType !== 0) {
                var isIncluded = false;
                for (let hex in liftedPieceObject.possibleMoves) {
                    var value = liftedPieceObject.possibleMoves[hex];
                    var hexElement = hexes.find(h => h.x == value.newX && h.y == value.newY).element;
                    $(hexElement).css("opacity", "unset");
                }
                lastTargetObject.assignPiece(liftedPieceObject);
                lastTargetObject.linkPiece(lastTargetDiv);
                mousedown = false;
                $(".lifted-piece").remove();
                liftedPieceObject = null;
                $(".background").removeClass("grabbing");
                $(".game-canvas").css("cursor", "default");
            }
        }
    });

    $(".game-canvas").on("touchend", (e) => {
        var selectedHex = board.getDataFromElement(document.elementFromPoint(e.originalEvent.changedTouches[0].pageX, e.originalEvent.changedTouches[0].pageY).parentElement.parentElement);
        newpiece = selectedHex.piece;
        if (liftedPieceObject != null) {
            if (liftedPieceObject.pieceType !== 0) {
                var isIncluded = false;
                for (let hex in liftedPieceObject.possibleMoves) {
                    var value = liftedPieceObject.possibleMoves[hex];
                    if (value.newX === selectedHex.x && value.newY === selectedHex.y) {
                        isIncluded = true;
                    }
                    var hexElement = hexes.find(h => h.x == value.newX && h.y == value.newY).element;
                    $(hexElement).css("opacity", "unset");
                }

                if (liftedPieceObject.player === selectedHex.piece.player || !isIncluded) {
                    lastTargetObject.assignPiece(liftedPieceObject);
                    lastTargetObject.linkPiece(lastTargetDiv);
                }
                else {
                    selectedHex.assignPiece(liftedPieceObject);
                    selectedHex.linkPiece(e.originalEvent.changedTouches[0].pageX, e.originalEvent.changedTouches[0].pageY);
                    MovePiece(lastTargetObject, selectedHex);
                }
                mousedown = false;
                $(".lifted-piece").remove();
                liftedPieceObject = null;
                $(".background").removeClass("grabbing");
                $(".game-canvas").css("cursor", "default");
            }
        }
    });

    $(".game-canvas").on("mouseup", ".hex-base", (e) => {
        var selectedHex = board.getDataFromElement(e.currentTarget.firstChild);
        newpiece = selectedHex.piece;
        if (liftedPieceObject != null) {
            if (liftedPieceObject.pieceType !== 0) {
                var isIncluded = false;
                for (let hex in liftedPieceObject.possibleMoves) {
                    var value = liftedPieceObject.possibleMoves[hex];
                    if (value.newX === selectedHex.x && value.newY === selectedHex.y) {
                        isIncluded = true;
                    }
                    var hexElement = hexes.find(h => h.x == value.newX && h.y == value.newY).element;
                    $(hexElement).css("opacity", "unset");
                }

                if (liftedPieceObject.player === selectedHex.piece.player || !isIncluded) {
                    lastTargetObject.assignPiece(liftedPieceObject);
                    lastTargetObject.linkPiece(lastTargetDiv);
                }
                else {
                    selectedHex.assignPiece(liftedPieceObject);
                    selectedHex.linkPiece(e.currentTarget.firstChild.firstChild.firstChild);
                    MovePiece(lastTargetObject, selectedHex);
                }
                mousedown = false;
                $(".lifted-piece").remove();
                liftedPieceObject = null;
                $(".background").removeClass("grabbing");
                $(".game-canvas").css("cursor", "default");
            }
        }
    });

    $(".game-canvas").on("touchmove", (e) => {
        e.preventDefault();
        $(".lifted-piece").css({ left: e.originalEvent.changedTouches[0].pageX, top: e.originalEvent.changedTouches[0].pageY });
    });

    $(".game-canvas").mousemove(function (e) {
        $(".lifted-piece").css({left: e.pageX, top: e.pageY});
    })

    $(".game-canvas").mouseleave(function (e) {
        if (liftedPieceObject != null) {
            if (liftedPieceObject.pieceType !== 0) {
                var isIncluded = false;
                for (let hex in liftedPieceObject.possibleMoves) {
                    var value = liftedPieceObject.possibleMoves[hex];
                    var hexElement = hexes.find(h => h.x == value.x && h.y == value.y).element;
                    $(hexElement).css("opacity", "unset");
                }
                lastTargetObject.assignPiece(liftedPieceObject);
                lastTargetObject.linkPiece(lastTargetDiv);
                mousedown = false;
                $(".lifted-piece").remove();
                liftedPieceObject = null;
                $(".background").removeClass("grabbing");
                $(".game-canvas").css("cursor", "default");
            }
        }
    });


    $('.copy-url').click(function () {
        const copyText = $("#share-url");
        copyText.select();
        document.execCommand("copy");
    });
});