<script setup>
import {computed} from "vue";

const props = defineProps(["cell", "row", "col"])
var sidesStyles = computed(() => {
  var result = {}
  if (props.cell.right)
    result["border-right"] = "1px solid black"
  if (props.cell.down)
    result["border-bottom"] = "1px solid black"
  return result
})

</script>

<template>
  <div :style="sidesStyles"></div>
</template>

<style scoped>
div {
  display: block;
  height: 100%;
  width: 100%;
}

</style>
