 "use strict";

class Hexagon {
    constructor(x, y, type, piece) {
        this.x = x;
        this.y = y;
        this.type = type;
        this.piece = piece;
    }

    link(hex, background, backgroundBackground) {
        this.element = hex;
        switch (this.type) {
            case 0:
                hex.classList.add("light");
                background.classList.add("background-light");
                backgroundBackground.classList.add("backgroundBackground-light")
                break;
            case 1:
                hex.classList.add("normal");
                background.classList.add("background-normal");
                backgroundBackground.classList.add("backgroundBackground-normal");
                break;
            case 2:
                hex.classList.add("dark");
                background.classList.add("background-dark");
                backgroundBackground.classList.add("backgroundBackground-dark");
                break;
        }
        this.linkPiece(background);
    }

    linkPiece(background) {
        RemoveClasses(background);
        $(background).removeClass("grabbed");
        $(background).addClass("grab");
        try {
            if (this.piece.player === 1) {
                $(background).addClass("player1")
            }
            else if (this.piece.player === 2){
                $(background).addClass("player2")
            }
        } catch (error) {
        };
        switch (this.piece.pieceType) {
            case 0:
                $(background).addClass("none");
                $(background).removeClass("grab");
                $(background).addClass("grabbed");
                break;
            case 1:
                $(background).addClass("pawn");
                break;
            case 2:
                $(background).addClass("rook");
                break;
            case 3:
                $(background).addClass("knight");
                break;
            case 4:
                $(background).addClass("bishop");
                break;
            case 5:
                $(background).addClass("queen");
                break;
            case 6:
                $(background).addClass("king");
                break;
        }
    }

    assignPiece(piece) {
        this.piece = piece;
    }
}

class ChessPiece {
    constructor(pieceType, player, possibleMoves) {
        this.possibleMoves = possibleMoves;
        this.pieceType = pieceType;
        this.player = player;
    }
}

class GridColumn {
    constructor(hexagons) {
        this.hexagons = hexagons;
    }
}

class Board {
    constructor(boardData, element) {
        element.empty();
        this.columns = [];
        this.element = element;
        boardData.columns.forEach((columns, index) => this.columns[index] = new GridColumn(columns));
    }

    linkElementsToData() {
        hexes.length = 0;
        this.columns.forEach((rowvalue) => {
            let row = document.createElement("div");
            row.classList.add("hex-row");
            this.element.append(row);
    
            rowvalue.hexagons.forEach(value => {
                let base = document.createElement("div");
                base.classList.add("hex-base");
                this.element.append(base);
                let hex = document.createElement("div");
                hex.classList.add("hex");
                if (gameMode == 1){
                    if (turn == 2){
                        hex.classList.add("animate-rotate-hex-reversed");
                    }
                    if (turn == 1){
                        hex.classList.add("animate-rotate-hex");
                    }
                }
                hex.id = value.piece.player;
                let backgroundElement = document.createElement("div");
                let backgroundBackgroundElement = document.createElement("div");
                backgroundElement.classList.add("background");
                backgroundBackgroundElement.classList.add("backgroundBackground");
                backgroundBackgroundElement.appendChild(backgroundElement);
                hex.appendChild(backgroundBackgroundElement);
                base.appendChild(hex);
                row.appendChild(base);
                value.link(hex, backgroundElement, backgroundBackgroundElement);
                hexes.push(value);
            })
        })
        if (gameMode == 1){
            setTimeout(RotateBoard, 10);       
        }
        else{
            RotateBoard();
        }
    }

    getDataFromElement(element) {
        for (let y = 0; y < this.columns.length; y++) {
            for (let x = 0; x < this.columns[y].hexagons.length; x++) {
                let hexData = this.columns[y].hexagons[x];
                if (element === hexData.element) {
                    return hexData;
                }
            }
        }
        return null;
    }
}

let board;
var hexes = [];

$(function () {
    if (gameMode == 2 && typeof player2 !== 'undefined'){
        generateHexagons();
    }
    else if(gameMode != 2){
        generateHexagons();
    }
    else if (gameMode == 2){
        $('#invite-second-player-dialog').collapse("show");
    }
});

function generateHexagons() {
    loadHexagons(function (result) {
        board = new Board(result.board, $("#game-canvas"));
        board.linkElementsToData();
    });
}

 function updateHexagons() {
     loadUpdatedHexagons(function (result) {
         board = new Board(result.board, $("#game-canvas"));
         board.linkElementsToData();
     });
 }

function loadHexagons(cb) {
    $(".overlay").addClass("active");
    $.post("/loadgame", {
            gameToken: gameToken
        }, {
            dataType: "json",
            traditional: true
        }
    ).then(data => {
        let result = {
            board: {
                columns: []
            }
        };
        $(".overlay").removeClass("active");
        $(".hex-grid").addClass("active");
        for (let columnIndex in data.grid) {
            let columns = data.grid[columnIndex];
            result.board.columns[columnIndex] = [];
            for (let hexIndex in columns) {
                let hex = columns[hexIndex];
                let hexagon = new Hexagon(hex.x, hex.y, hex.color, new ChessPiece(hex.piece.pieceType, hex.piece.player, hex.piece.possibleMoves));
                result.board.columns[columnIndex].push(hexagon);
            }
        }
        gameMode = data.mode;
        turn = data.nextPlayer;
        if (cb) {
            cb(result);
        }
    });
}

 function loadUpdatedHexagons(cb) {
     $.post("/loadgame", {
             gameToken: gameToken
         }, {
             dataType: "json",
             traditional: true
         }
     ).then(data => {
         let result = {
             board: {
                 columns: []
             }
         };
         $(".overlay").removeClass("active");
         $(".hex-grid").addClass("active");
         for (let columnIndex in data.grid) {
             let columns = data.grid[columnIndex];
             result.board.columns[columnIndex] = [];
             for (let hexIndex in columns) {
                 let hex = columns[hexIndex];
                 let hexagon = new Hexagon(hex.x, hex.y, hex.color, new ChessPiece(hex.piece.pieceType, hex.piece.player, hex.piece.possibleMoves));
                 result.board.columns[columnIndex].push(hexagon);
             }
         }
         turn = data.nextPlayer;
         if (cb) {
             cb(result);
         }
     });
 }