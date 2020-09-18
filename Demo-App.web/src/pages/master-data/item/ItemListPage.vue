<template>
  <q-page padding>
    <div class="q-pa-md">
      <app-list
        :mstyle="'margin-top: -27px; height: 600px !important;'"
        :mclass="'cursor-pointer'"
        :showFilter="true"
        :buttons="buttons"
        :tableButtons="tableButtons"
        :data="Items"
        :columns="columns"
        :rowKey="'ID'"
        :rowHoverTitle="'Double click to edit'"
        @on-edit="onEdit"
        @on-new="onNew"
        @on-confirm-delete="onShowConfirmDeleteDialog" />
      
      <!-- confirm delete dialog -->
      <app-confirm-dialog
        :showDialog="showDialog"
        :Icon="dialogIcon"
        :IconColor="dialogIconColor"
        :Title="dialogTitle"
        :Message="dialogMessage"
        :Buttons="dialogButtons"
        @on-delete="onDelete"
        @on-cancel="onCancelConfirmDialog" />

    </div>
  </q-page>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapActions } from 'vuex';
import AppListComponent from 'components/core/app-list-component.vue';
import AppConfirmDialogComponent from 'components/core/app-confirm-dialog.vue';
import { Route } from 'vue-router';

export default Vue.extend({
  // name: 'PageName',
  components: { 
      'app-list': AppListComponent,
      'app-confirm-dialog': AppConfirmDialogComponent
  },
  data() {
    return {
      dialogTitle: '',
      dialogMessage: '',
      dialogIcon: '',
      dialogIconColor: '',
      dialogButtons: [],
      showDialog: false,
      ItemID: 0,
      columns: [
        { name: 'ID', align: 'center', label: 'ID', field: 'ID', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Name', align: 'center',label: 'Name', field: 'Name', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Description', align: 'center',label: 'Description', field: 'Description', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Price', align: 'center',label: 'Price', field: 'Price', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'RecordStatus', align: 'center',label: 'Record Status', field: 'RecordStatus', sortable: true, headerClasses: 'bg-secondary text-white' }
      ],
      //for buttons
      buttons: [
        { icon: 'add_circle', color: 'positive', label: 'Add', title: 'Add New', click: 'on-new' }
      ],
      tableButtons: [
        { icon: 'create', color: 'secondary', size: 'sm', label: 'Edit', click: 'on-edit' },
        { icon: 'delete', color: 'negative', size: 'sm', label: 'Delete', click: 'on-confirm-delete' }
      ]
    }
  },
  methods: {
    ...mapActions('item_store',['get','delete']),
    onNew(){
      this.$router.push('/item').catch(err => {})
    },
    onEdit(props: any){
      this.$router.push('/item/' + props.row.ID).catch(err => {})
    },
    onDelete(){
      if(this.ItemID > 0){
          if(this.delete(this.ItemID)){
              this.$q.notify({
                color: "green-4",
                textColor: "white",
                icon: "cloud_done",
                position: "top-right",
                message: "Successfully Deleted"
              });
          }
      }
      this.showDialog = false;
    },
    onShowConfirmDeleteDialog(props: any){
      this.ItemID = props.row.ID;
      this.dialogTitle="Confirm Delete";
      this.dialogMessage="Are you sure to delete this record?";
      this.dialogIcon="delete";
      this.dialogIconColor="negative";
      // @ts-ignore
      this.dialogButtons=[{ label: 'Cancel', color: 'negative', click: 'on-cancel'}, {label: 'Yes', color: 'primary', click: 'on-delete'}];
      this.showDialog = true;
    },
    onCancelConfirmDialog(){
      this.showDialog = false;
      this.dialogTitle="";
      this.dialogMessage="";
      this.dialogIcon="";
      this.dialogIconColor="";
      this.dialogButtons=[];
      this.ItemID = 0;
    }
  },
  computed: {
    ...mapGetters('item_store',['Items'])
  },
  beforeMount(){
    this.get();
  }
})
</script>
