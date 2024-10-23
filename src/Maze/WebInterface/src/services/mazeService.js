import axios from 'axios'
import FetchError from '@/errors/FetchError.js'

class MazeService {
  apiUri = import.meta.env.VITE_API_URI

  async generateMaze(rows, cols) {
    let response
    try {
      response = await axios.post(
        this.apiUri + `/maze/generate?rows=${rows}&cols=${cols}`,
      )
    } catch (err) {
      if (err.response.status === 400) throw new FetchError(err.response.data)
    }
    return response.data
  }

  async getPath(matrix, sourceX, sourceY, destX, destY) {
    let response = await axios.post(
      this.apiUri +
        `/maze/findPath?sourceX=${sourceX}&sourceY=${sourceY}&destX=${destX}&destY=${destY}`,
      matrix,
    )

    return response.data
  }

  async importString(mazeString) {
    let response
    try {
      response = await axios.post(
        this.apiUri + `/maze/fromString`,
        mazeString,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        },
      )
    } catch (err) {
      if (err.response.status === 400) throw new FetchError(err.response.data)
    }

    return response.data
  }
  async exportString(mazeJson) {
    let response = await axios.post(this.apiUri + `/maze/toString`, mazeJson)

    return response.data
  }
}

export default MazeService
