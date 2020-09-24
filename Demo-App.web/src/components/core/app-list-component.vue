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
          <!-- Top Buttons and Search Box -->
          <template v-slot:top>
            <q-btn-group>
              <!-- buttons -->
              <template v-for="(button, key) in mutableButtons">
                <q-btn
                  glossy
                  :key="key"
                  :icon="button.icon"
                  :color="button.color"
                  :label="button.label"
                  :title="button.title"
                  @click="$emit(button.click == undefined ? '' : button.click)" />
              </template>
            </q-btn-group>
          
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
              <!-- if mutableTableButtons not null then First column Header is Actions -->
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
              <!-- if mutableTableButtons not null then display buttons in First column in body-->
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
                :props="props">
                {{ !col.name.toLowerCase().includes('e_input') ? col.value : '' }}
                <!-- for popup edit -->
                <q-btn
                  v-if="col.name.toLowerCase().includes('e_popup')"
                  dense
                  color="warning"
                  size="sm"
                  :title="`Click to Edit ${col.field}`"
                  icon="create" />
                <q-popup-edit v-if="col.name.toLowerCase().includes('e_popup')" v-model="props.row[col.field]" :title="`Update ${col.field}`" buttons persistent>
                  <q-input
                    :type="col.field.toLowerCase().includes('quantity') ? 'number' : ''"
                    v-model="props.row[col.field]"
                    @change="$emit('on-generate-total')"
                    dense
                    autofocus
                    lazy-rules
                    :rules="[
                    val => val !== null && val !== '' || `Please input ${col.field}`,
                    val => val > 0 || `Please type a real ${col.field}`]" />
                </q-popup-edit>
                <!-- for input type col for edit -->
                <q-input
                    outlined
                    :style="col.field.toLowerCase().includes('price') ? 'width: 120px !important;' : col.field.toLowerCase().includes('quantity') ? 'width: auto !important;' : 'width: 200px !important;'"
                    class="absolute-center q-pa-sm"
                    v-if="col.name.toLowerCase().includes('e_input')"
                    :type="col.field.toLowerCase().includes('quantity') ? 'number' : ''"
                    v-model="props.row[col.field]"
                    @change="$emit('on-generate-total')"
                    dense
                    lazy-rules
                    :rules="[
                    val => val !== null && val !== '' || `Please input ${col.field}`,
                    val => val > 0 || `Please type a real ${col.field}`]" />
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
