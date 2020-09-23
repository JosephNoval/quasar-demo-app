<template>
  <q-dialog
      v-model="mutableShowModal"
      persistent>
      <q-card style="max-width: 80%; min-width: 80%;max-height: 80%; min-height: 80%;">
        <q-bar class="bg-primary text-white">
          <q-icon :name="mutableModalIcon" />
          <div>{{ mutableModalTitle }}</div>

          <q-space />

          <q-btn dense flat icon="close" @click="$emit('on-close-modal')" v-close-popup>
            <q-tooltip content-class="bg-white text-primary">Close</q-tooltip>
          </q-btn>
        </q-bar>

        <q-card-section>

          <q-table
            style="height: 470px !important;"
            :data="mutableModalTableData"
            :columns="mutableModalTableColumns"
            :visible-columns="mutableModalTableVisibleColumns"
            row-key="ID"
            :filter="modalFilter"
            :selection="mutableModalTableSelectionType"
            :selected.sync="selected"
            no-data-label="I didn't find anything for you"
            no-results-label="The filter didn't uncover any results"
          >
            <template v-slot:top>
              <!-- buttons -->
              <template v-for="(button, key) in mutableModalButtons">
                <q-btn
                  class="q-ma-xs"
                  :disable="(selected.length == 0 && button.label == 'Choose')"
                  :key="key"
                  :icon="button.icon"
                  :color="button.color"
                  :label="button.label"
                  :title="button.label"
                  @click="$emit((button.click == undefined ? '' : button.click), selected)" />
              </template>

              <q-space />
              <q-input
                v-show="mutableShowFilter"
                filled
                dense
                ref="modalSearch"
                color="primary"
                v-model="modalFilter"
                label="Search"
                @keyup.esc="modalFilter = ''">
                <template v-slot:append>
                  <q-icon
                    v-if="modalFilter !== ''"
                    name="close"
                    @click="modalFilter = ''"
                    class="cursor-pointer"
                    title="Press ESC to Clear" />
                  <q-icon name="search" />
                </template>
              </q-input>
            </template>

             <!-- table header -->
            <template v-slot:header="props">
              <q-tr :props="props">
                <q-th auto-width class='bg-secondary text-white' ></q-th>
                <q-th
                  v-for="col in props.cols"
                  :key="col.name"
                  :props="props"
                >
                  {{ col.label }}
                </q-th>
              </q-tr>
            </template>

            <!-- table body -->
            <template v-slot:body="props">
              <q-tr
                title="Double click to pick"
                :props="props"
                @click="!props.selected ? props.selected = true : false"
                @dblclick="$emit('on-row-dbl-click', selected)"
                class="cursor-pointer">
                <q-td>
                  <q-checkbox
                    :title="props.selected ? 'Unselect' : 'Select'"
                    v-model="props.selected"/>
                </q-td>
                <q-td
                  v-for="col in props.cols"
                  :key="col.name"
                  :props="props"
                >
                  {{ col.value }}
                </q-td>
              </q-tr>
            </template>
          
            <template v-slot:no-data="{ icon, message, filter }">
            <div class="full-width row flex-center text-accent q-gutter-sm">
              <q-icon size="2em" name="sentiment_dissatisfied" />
              <span>
                Well this is sad... {{ message }}
              </span>
              <q-icon size="2em" :name="filter ? 'filter_b_and_w' : icon" />
            </div>
          </template>

          </q-table>
        </q-card-section>

      </q-card>
    </q-dialog>
</template>

<script>
import Vue from "vue";
export default Vue.extend({
  name: 'DynamicPickerListComponent',
  props:['showModal', 'modalTitle', 'modalIcon', 'modalTableData', 'modalTableColumns', 'modalTableVisibleColumns',
   'modalTableSelectionType', 'modalButtons', 'showFilter', 'filter', 'modalSelected'],
  data () {
    return {}
  },
  computed: {
    selected: {
      get() {
        return this.modalSelected;
      },
      set(value) {
        this.$emit('on-modal-selected-change', value)
      }
    },
    modalFilter: {
      get(){
        return this.filter;
      },
      set(value) {
        this.$emit('on-modal-filter-change', value)
      }
    },
    mutableShowModal:function() {
      return this.showModal;
    },
    mutableModalTitle: function() {
      return this.modalTitle;
    },
    mutableModalIcon: function() {
      return this.modalIcon;
    },
    mutableModalTableData: function() {
      return this.modalTableData;
    },
    mutableModalTableColumns: function() {
      return this.modalTableColumns;
    },
    mutableModalTableVisibleColumns: function() {
      return this.modalTableVisibleColumns;
    },
    mutableModalTableSelectionType: function() {
      return this.modalTableSelectionType;
    },
    mutableModalButtons: function() {
      return this.modalButtons;
    },
    mutableShowFilter: function() {
      return this.showFilter;
    }
  }
})
</script>

