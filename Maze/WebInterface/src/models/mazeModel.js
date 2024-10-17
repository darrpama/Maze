import MazeCellModel from "@/models/mazeCellModel.js";

class MazeModel {
  rows = 1;
  cols = 1;
  cells = null;

  generateMaze() {
    this.rows = 5;
    this.cols = 5;
    this.cells = [
      [new MazeCellModel(true, false), new MazeCellModel(true, true), new MazeCellModel(false, false), new MazeCellModel(false, false), new MazeCellModel(false, true)],
      [new MazeCellModel(true, false), new MazeCellModel(true, true), new MazeCellModel(false, false), new MazeCellModel(false, false), new MazeCellModel(false, true)],
      [new MazeCellModel(true, false), new MazeCellModel(true, true), new MazeCellModel(false, false), new MazeCellModel(false, false), new MazeCellModel(false, true)],
      [new MazeCellModel(true, false), new MazeCellModel(true, true), new MazeCellModel(false, false), new MazeCellModel(false, false), new MazeCellModel(false, true)],
      [new MazeCellModel(true, false), new MazeCellModel(true, true), new MazeCellModel(false, false), new MazeCellModel(false, false), new MazeCellModel(false, true)],
    ];
  }

  fromJson(mazeJson) {
    this.rows = mazeJson.length;
    this.cols = mazeJson[0].length;

    for (let row = 0; row < mazeJson.length; row++) {
      for (let col = 0; col < row.length; col++) {
        let cell = mazeJson[row][col];
        mazeJson[row][col] = new MazeCellModel(cell["right"], cell["down"]);
      }
    }

    this.cells = mazeJson;

  }

}

export default MazeModel;
