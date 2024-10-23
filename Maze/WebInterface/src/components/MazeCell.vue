<script setup>
import {computed, ref} from "vue";
import {pathFlags} from "@/models/mazeCellModel.js";
const emit = defineEmits(['dblclick', 'oneclick'])


const props = defineProps(["cell", "row", "col"])
const delay = 300;
const clicks = ref(0)
let timer = ref(null);


let sidesStyles = computed(() => {
  let result = {}
  if (props.cell.right)
    result["border-right"] = "2px solid black"
  if (props.cell.down)
    result["border-bottom"] = "2px solid black"
  return result
})

let circleStyles = computed(() => {
  let result = {}
  switch (props.cell.pathFlag) {
    case pathFlags.FROM_PATH:
      result["background-color"] = "blue";
      break;
    case pathFlags.TO_PATH:
      result["background-color"] = "green";
      break;
    case pathFlags.MIDDLE_PATH:
      result["background-color"] = "red";
      break;
    case pathFlags.NO_PATH:
      result["display"] = "none";
      break;
  }
  return result;
})

function oneClick(event) {
  clicks.value++
  if (clicks.value === 1) {
    timer.value = setTimeout(function () {
      emit('oneclick')
      clicks.value = 0
    }, delay);
  } else {
    clearTimeout(timer.value);
    emit('dblclick')
    clicks.value = 0;
  }
}
</script>

<template>
  <div :style="sidesStyles" class="innerCell" @click="oneClick" @dblclick="onDblClick">
    <div class="circle" :style="circleStyles"></div>
  </div>
</template>

<style scoped>
div {
  display: block;
  height: 100%;
  width: 100%;
}

.innerCell:hover {
  background-color: lightgray;
}
.circle {
  width: 100%;
  height: 100%;
  border-radius: 50%;
}
</style>
