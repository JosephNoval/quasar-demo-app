<template>
        <q-table
          :class="mclass"
          :style="mstyle"
          bordered
          :data="data"
          :columns="mutableColumns"
          :filter="filter"
          :pagination="initialPagination"
          no-data-label="I didn't find anything for you"
          no-results-label="The filter didn't uncover any results"
          :row-key="rowKey"
        >
          <template v-slot:top>
            
            <!-- buttons -->
            <template v-for="(button, key) in mutableButtons">
              <q-btn
                :stretch="button.stretch"
                class="q-mr-sm"
                :style="button.style"
                :key="key"
                :icon="button.icon"
                :color="button.color"
                :label="button.label"
                :title="button.title"
                @click="$emit(button.click == undefined ? '' : button.click)" />
            </template>

            <q-space />
            <q-input
              v-show="mutableShowFilter"
              filled
              dense
              ref="Search"
              color="primary"
              v-model="filter"
              style="width:300px;background: rgba(231, 231, 231, 0.82);border-radius: 4px;margin-top: 4px;"
              label="Search"
              @keyup.esc="clearFilter">
              <template v-slot:append>
                <q-icon
                  v-if="filter !== ''"
                  name="close"
                  @click="clearFilter"
                  class="cursor-pointer"
                  title="Press ESC to Clear" />
                <q-icon name="search" />
              </template>
            </q-input>
          </template>
          <!-- table header -->
          <template v-slot:header="props">
            <q-tr :props="props">
              <q-th v-if="mutableTableButtons" auto-width class='bg-secondary text-white' >Actions</q-th>
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
              :props="props"
              clickable
              :title="mutableRowHoverTitle"
              @dblclick="$emit('on-edit', props)"
              @click="$emit('on-view', props)">
              <q-td
                v-if="mutableTableButtons"
                auto-width>
                <template v-for="(t_button, key) in mutableTableButtons">
                  <q-btn
                    v-if="(t_button.label != 'Delete') || (t_button.label == 'Delete' && props.row.RecordStatus == 'Active')"
                    dense
                    class="q-ma-xs"
                    :key="key"
                    :size="t_button.size"
                    :icon="t_button.icon"
                    :color="t_button.color"
                    :title="t_button.label"
                    @click="$emit(t_button.click == undefined ? '' : t_button.click, props)" />
                </template>
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
</template>

<script>
import Vue from "vue";
export default Vue.extend({
  name: "DynamicTableComponent",
  props: {
    data: Array,
    columns: Array,
    rowKey: String,
    buttons: Array,
    tableButtons: Array,
    showFilter: Boolean,
    mstyle: String,
    mclass: String,
    rowHoverTitle: String
    
  },
  data() {
    return {
      mutableRowKey: this.rowKey,
      mutableColumns: this.columns,
      mutableButtons: this.buttons,
      mutableTableButtons: this.tableButtons,
      mutableShowFilter: this.showFilter,
      mutableStyle: this.mstyle,
      mutableClass: this.mclass ? this.mclass + ' my-sticky-header-table' : 'my-sticky-header-table',
      mutableRowHoverTitle: this.rowHoverTitle,

      initialPagination: {
        sortBy: 'desc',
        descending: false,
        page: 1,
        rowsPerPage: 10
        // rowsNumber: xx if getting data from a server
      },

      filter: ''
    };
  },
  methods: {
    clearFilter(){
        this.filter = '';
        this.$refs.Search.focus();
    }
  },
  mounted() {
        this.$refs.Search.focus();
  }
})
</script>
