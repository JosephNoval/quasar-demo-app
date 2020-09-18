<template>
  <q-dialog v-model="mutableShowDialog" persistent>
    <q-card>
      <q-bar class="bg-primary text-white">
        <div>{{mutableTitle}}</div>
      </q-bar>
      <q-card-section class="row items-center">
        <q-avatar :icon="mutableIcon" :color="mutableIconColor" text-color="white" />
        <span class="q-ml-sm">{{mutableMessage}}</span>
      </q-card-section>

      <q-card-actions align="right">
        <template v-for="(button, key) in mutableButtons">
              <q-btn
                flat
                :key="key"
                :color="button.color"
                :label="button.label"
                :title="button.label"
                @click="$emit((button.click == undefined) ? '' : button.click)" 
                v-close-popup />
            </template>
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script>
import Vue from 'vue';
export default Vue.extend({
  name: 'DynamicConfirmDialogComponent',
  props:['showDialog', 'Title', 'Message', 'Buttons', 'Icon', 'IconColor'],
  data () {
    return {}
  },
  computed: {
    mutableShowDialog:function() {
      return this.showDialog;
    },
    mutableMessage: function() {
      return this.Message;
    },
    mutableTitle: function() {
      return this.Title;
    },
    mutableButtons: function() {
      return this.Buttons;
    },
    mutableIcon: function() {
      return this.Icon;
    },
    mutableIconColor: function() {
      return this.IconColor;
    }
  }
})
</script>

