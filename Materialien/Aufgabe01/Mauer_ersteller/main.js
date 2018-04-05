/**
 * ALGORITHMUS:
 * 
 *  Validation N:[O/X]:
 *      2:[O]
 *      3:[O]
 *      4:[O]
 *      5:[O]
 *      6:[O] 
 *      7:[O] 
 *      8:[X]
 *      9:[O/X]
 *      10:[O/X]
 * 
 *  Beschreibung:
 *      geheim :)
 */

let curRowElement;
let wallContainerElement

let wallsIndexes = [/*10,9,8,*/8,8,7,6,5,4,3,2]

let walls = [
    /*[
        // 10
        [1],
        [2],
        [3],
        [4],
        [5],
        [6]
    ],
    [
        // 9
        [1],
        [2],
        [3],
        [4],
        [5]
    ],
    [
        // 8
        [1, 2, 3, 4, 5, 6, 7, 8],
        [2, 7, 5, 6, 3, 4, 8, 1],
        [7, 1, 4, 6, 3, 8, 5, 2],
        [4, 5, 6, 7, 8, 1, 2, 3],
        [5, 6, 7, 8, 1, 2, 3, 4]
    ],*/
    [
        // 8
        [1, 2, 3, 4, 5, 6, 7, 8],
        [5, 3, 8, 1, 6, 7, 4, 2],
        [7, 4, 8, 6, 1, 3, 2, 5],
        [4, 8, 1, 7, 2, 5, 6, 3],
        [2, 7, 5, 4, 6, 8, 3, 1]
    ],
    [
        // 8
        [1, 5, 4, 6, 3, 7, 2, 8],
        [2, 5, 6, 4, 3, 7, 8, 1],
        [3, 5, 4, 6, 7, 8, 1, 2],
        [4, 5, 6, 7, 2, 8, 1, 3],
        [5, 6, 3, 7, 2, 8, 1, 4] // ERROR
    ],
    [
        // 7
        [1, 4, 3, 2, 6, 5, 7],
        [2, 4, 5, 3, 6, 7, 1],
        [3, 4, 5, 1, 6, 7, 2],
        [4, 5, 6, 2, 1, 7, 3]
    ],
    [
        // 6
        [1, 4, 3, 2, 6, 5],
        [2, 4, 5, 3, 6, 1],
        [3, 4, 5, 1, 6, 2],
        [4, 5, 6, 2, 1, 3]
    ],
    [
        // 5
        [1, 3, 2, 4, 5],
        [2, 3, 4, 5, 1],
        [3, 4, 1, 5, 2]
    ],
    [
        // 4
        [1, 3, 2, 4],
        [2, 3, 4, 1],
        [3, 4, 1, 2]
    ], 
    [
        // 3
        [1, 2, 3],
        [2, 3, 1]
    ],
    [
        // 2
        [1, 2],
        [2, 1]
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