import axios from "axios";
import qs from "qs"

class MazeService {
  #apiUri = import.meta.env.VITE_API_URI

  async generateMaze(rows, cols) {
    let response = await axios.post(this.#apiUri + `/maze/generate?rows=${rows}&cols=${cols}`)
    return response.data
  }

  getPath(matrix, sourceX, sourceY, destX, destY) {

    axios.post(
      this.#apiUri + `/maze/findPath?sourceX=${sourceX}&sourceY=${sourceY}&destX=${destX}&destY=${destY}`,
      matrix)
      .then(response => {
        console.log(response.data);
      })

  }
}

export default MazeService;
