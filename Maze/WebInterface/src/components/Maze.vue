<script setup>
import MazeModel from "@/models/mazeModel.js";
import {pathFlags }  from "@/models/mazeCellModel.js";
import MazeCell from "@/components/MazeCell.vue";
import {reactive, ref} from 'vue';
import { computed } from 'vue';
import MazeService from "@/services/mazeService.js";

const mazeModel = reactive(new MazeModel());
const mazeService = new MazeService()

let mazeRows = ref(5);
let mazeCols = ref(5);

async function generateMaze() {
  try {
    await mazeModel.generateMaze(mazeRows.value, mazeCols.value)
  }
  catch({message}) {
    alert(message)
  }

}
function onDblClick(row, col) {
  mazeModel.setPathFrom(row, col)
}
function onClick(row, col) {
  mazeModel.setPathTo(row, col)

}
async function onImport(event) {
  let mazeString = await event.target.files[0].text()
  try {
    await mazeModel.fromString(mazeString)
  }
  catch ({message}) {
    alert(message)
  }
  finally {
    event.target.value = null
  }
  mazeRows.value = mazeModel.rows
  mazeCols.value = mazeModel.cols
}

async function onExport() {
  if (mazeModel.cells === null)
    alert("Maze not defined.")
    return;

  let mazeString = await mazeModel.exportString();

  var link = document.createElement('a');
  link.download = 'maze.txt';
  var blob = new Blob([mazeString], {type: 'text/plain'});
  link.href = window.URL.createObjectURL(blob);
  link.click();
}

const stylesForMatrix = computed(() => {
  return {
    'grid-template-columns': `repeat(${mazeModel.cols}, 1fr)`,
    'grid-template-rows': `repeat(${mazeModel.rows}, 1fr)`,
  }
})


</script>

<template>
  <div class="maze">
    <div v-if="mazeModel.cells !== null" :style="stylesForMatrix" class="cells" >
      <template v-for="(row, rowIndex) in mazeModel.cells" :key="rowIndex">
        <div v-for="(cell, colIndex) in row" :key="colIndex" class="cell">
          <MazeCell :cell="cell" :row="rowIndex" :col="colIndex" @oneclick="onClick(rowIndex, colIndex)" @dblclick="onDblClick(rowIndex, colIndex)"></MazeCell>
        </div>
      </template>
    </div>
    <div v-else class="cells cells-placeholder">

    </div>
    <div class="size">
      <input v-model="mazeRows" type="number">
      <input v-model="mazeCols" type="number">
    </div>
    <div class="size">
      <input @input="onImport" type="file" accept="text/plain">
      <button @click="onExport">Export</button>
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

  border-left: 2px solid black;
  border-top: 2px solid black;
}
.cells-placeholder {
  border: 2px solid black;
}
.cell {
  //border: 1px solid black;
}
.size {
  margin-top: 1rem;
}
#generate-button {
  margin-top: 1rem;
}

</style>
