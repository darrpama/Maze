export const pathFlags = {
  FROM_PATH: 'fromPath',
  TO_PATH: 'toPath',
  MIDDLE_PATH: 'middlePath',
  NO_PATH: 'noPath',
}

class MazeCellModel {
  right = false
  down = false
  row = null
  col = null
  pathFlag = pathFlags.NO_PATH
  constructor(right, down, row, col) {
    this.right = right
    this.down = down
    this.row = row
    this.col = col
  }
}
export default MazeCellModel
