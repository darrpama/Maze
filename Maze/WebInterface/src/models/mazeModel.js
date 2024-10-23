import MazeCellModel, {pathFlags} from "@/models/mazeCellModel.js";
import MazeService from "@/services/mazeService.js";

class MazeModel {
  rows = 1;
  cols = 1;
  cells = null;
  mazeJson = null;
  mazeService = new MazeService();

  pathFrom = null;
  pathTo = null;

  checkGetPathPossible() {
    return this.pathFrom !== null && this.pathTo !== null && this.pathFrom !== this.pathTo;
  }

  setPathFrom(row, col) {
    this.pathFrom = this.cells[row][col];

    this.cells[row][col].pathFlag = pathFlags.FROM_PATH;
    this.cleanCellsByFlag(row, col, pathFlags.FROM_PATH);

    if (this.checkGetPathPossible()) {
      this.buildPath();
    }
  }

  setPathTo(row, col) {
    this.pathTo = this.cells[row][col];

    this.cells[row][col].pathFlag = pathFlags.TO_PATH;
    this.cleanCellsByFlag(row, col, pathFlags.TO_PATH);
    if (this.checkGetPathPossible()) {
      this.buildPath();
    }
  }
  cleanCellsByFlag(row, col, cleanFlag) {
    for (let i = 0; i < this.cells.length; i++) {
      for (let j = 0; j < this.cells[0].length; j++) {
        if (i === row && j === col) {
          continue;
        }
        if (this.cells[i][j].pathFlag === cleanFlag ) {
          this.cells[i][j].pathFlag = pathFlags.NO_PATH;
        }
      }
    }
  }
  cleanAllCellsFlag() {
    for (let i = 0; i < this.cells.length; i++) {
      for (let j = 0; j < this.cells[0].length; j++) {
        this.cells[i][j].pathFlag = pathFlags.NO_PATH;
      }
    }


  }

  async buildPath() {
    this.cleanAllCellsFlag()
    let path = await this.mazeService.getPath(this.mazeJson, this.pathFrom.row, this.pathFrom.col, this.pathTo.row, this.pathTo.col)
    for (let i = 0; i < path.length; i++) {
      let pathCell = path[i];

      if (i === 0) {
        this.cells[pathCell.row][pathCell.col].pathFlag = pathFlags.FROM_PATH;
        continue;
      }
      if (i === path.length - 1) {
        this.cells[pathCell.row][pathCell.col].pathFlag = pathFlags.TO_PATH;
        continue;
      }
      this.cells[pathCell.row][pathCell.col].pathFlag = pathFlags.MIDDLE_PATH;

    }
  }

  fromJson(mazeJson) {
    console.log(mazeJson);
    this.rows = mazeJson.length;
    this.cols = mazeJson[0].length;
    this.mazeJson = mazeJson;

    let cells = [];

    for (let row = 0; row < this.rows; row++) {
      let cellsRow = [];
      for (let col = 0; col < this.cols; col++) {
        let cell = mazeJson[row][col];
        cellsRow.push(new MazeCellModel(cell["right"], cell["down"], row, col));
      }
      cells.push(cellsRow);
    }
    this.cells = cells;
  }
  async fromString(mazeString) {
    let mazeJson = await this.mazeService.importString(mazeString)
    this.fromJson(mazeJson);

  }
  async exportString() {
    return await this.mazeService.exportString(this.mazeJson)
  }


  async generateMaze(rows, cols) {
    this.pathFrom = null;
    this.pathTo = null;

    let mazeJson = await this.mazeService.generateMaze(rows, cols);
    this.fromJson(mazeJson);
  }
}

export default MazeModel;
