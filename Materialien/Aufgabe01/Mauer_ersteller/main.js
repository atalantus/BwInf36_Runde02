let curRowElement;
let wallContainerElement

let wallsIndexes = [2,3,4,8]

let walls = [
    [
        [1, 2],
        [2, 1]
    ],
    [
        [1, 3, 2],
        [2, 3, 1]
    ],
    [
        [2, 1, 3, 4],
        [4, 3, 2, 1],
        [1, 4, 3, 2]
    ],
    [
        [1, 2, 3, 4, 5, 6, 7, 8],
        [8, 5, 7, 6, 4, 3, 2, 1],
        [2, 3, 4, 5, 8, 1, 6, 7],
        [7, 5, 6, 1, 8, 4, 3, 2],
        [4, 7, 5, 1, 8, 2, 3, 6]
    ]
];

$(function() {
    wallContainerElement = $("#wall-container");
    displayWalls();
});

function displayWalls() {
    for (let i = 0; i < walls.length; i++) {
        let header = $(`<h2>N = ${wallsIndexes[i]}</h2>`);
        let wallContainer = $(`<div></div>`);
        wallContainerElement.append(header, wallContainer);
        for(let y = 0; y < walls[i].length; y++) {
            let row = walls[i][y];
            curRowElement = $(`<div class="row"></div>`)
            wallContainer.append(curRowElement);
            row = row.map(displayRow);
        }
    }
}

function displayRow(x) {
    let newBrick = $(`<div class="brick">${x}</div>`);
    newBrick.width(25 * x);
    curRowElement.append(newBrick);
}