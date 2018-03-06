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
 *      8:[O/X]
 *      9:[O/X]
 *      10:[O/X]
 * 
 *  Beschreibung:
 *      geheim :)
 */

let curRowElement;
let wallContainerElement

let wallsIndexes = [10,9,8,7,6,5,4,3,2]

let walls = [
    [
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
        [1],
        [2],
        [3],
        [4],
        [5]
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