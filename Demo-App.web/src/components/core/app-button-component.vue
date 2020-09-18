<template>
  <div class="column">
        <div
          :class="mutableClass"
          :style="mutableStyle"
        >
          <!-- buttons -->
            <template v-for="(button, key) in mutableButtons">
              <q-btn
                v-if="(button.label != 'Delete') || (button.label == 'Delete' && showDeleteButton)"
                :no-caps="button.no_caps"
                :class="button.class"
                :style="button.style"
                :stretch="button.stretch"
                :key="key"
                :icon="button.icon"
                :type="button.type"
                :color="button.color"
                :label="button.label"
                :title="button.label"
                @click="$emit(button.click == undefined ? '' : button.click)" />
            </template>

        </div>
      </div>
</template>

<script>
import Vue from 'vue';

export default Vue.extend({
  name: 'AppButtonsComponent',
  props: {
    buttons: Array,
    mclass: String,
    mstyle: String,
    showDeleteButton: Boolean,
    showActivateButton: Boolean
  },
  data() {
    return {
      mutableButtons: this.buttons,
      mutableClass: this.mclass,
      mutableStyle: this.mstyle
    };
  }
})
</script>
