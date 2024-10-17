<script setup>
import MazeModel from "@/models/mazeModel.js";
import MazeCell from "@/components/MazeCell.vue";
import {reactive} from 'vue';
import { computed } from 'vue';
import MazeService from "@/services/mazeService.js";

const mazeModel = reactive(new MazeModel());
const mazeService = new MazeService()


async function generateMaze() {
  let mazeJson = await mazeService.generateMaze(5, 5)
  if (mazeJson !== null) {
    mazeModel.fromJson(mazeJson)
  }

}

const stylesForMatrix = computed(() => {
  return {
    'grid-template-columns': `repeat(${mazeModel.rows}, 1fr)`,
    'grid-template-rows': `repeat(${mazeModel.cols}, 1fr)`,
  }
})


</script>

<template>
  <div class="maze">
    <div v-if="mazeModel.cells !== null" :style="stylesForMatrix" class="cells" >
      <template v-for="(row, rowIndex) in mazeModel.cells" :key="rowIndex">
        <div v-for="(cell, colIndex) in row" :key="colIndex" class="cell">
          <MazeCell :cell="cell" :row="rowIndex" :col="colIndex"></MazeCell>
        </div>
      </template>
    </div>

    <button id="generate-button" @click="generateMaze">generate</button>
  </div>
</template>

<style scoped>
.maze {
  margin-top: 1rem;
  display: grid;
  justify-items: center;
}
.cells {
  display: grid;
  justify-content: center;
  width: 500px;
  height: 500px;

  border-left: 1px solid black;
  border-top: 1px solid black;
}
.cell {
  //border: 1px solid black;
}
#generate-button {
  margin-top: 1rem;
}

</style>
