<template>
  <q-page padding>
    <div class="q-pa-md">
      <app-list
        :mstyle="'margin-top: -27px; height: 600px !important;'"
        :mclass="'cursor-pointer'"
        :showFilter="true"
        :buttons="buttons"
        :tableButtons="tableButtons"
        :data="Orders"
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
      OrderID: 0,
      columns: [
        { name: 'ID', align: 'center', label: 'ID', field: 'ID', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'OrderNo', align: 'center',label: 'Order No', field: 'OrderNo', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'CustomerName', align: 'center',label: 'Customer', field: 'CustomerName', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'PaymentMethod', align: 'center',label: 'Payment Method', field: 'PaymentMethod', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Total', align: 'center',label: 'Total', field: 'Total', sortable: true, headerClasses: 'bg-secondary text-white' },
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
    ...mapActions('order_store',['get','delete']),
    onNew(){
      this.$router.push('/order').catch(err => {})
    },
    onEdit(props: any){
      this.$router.push('/order/' + props.row.ID).catch(err => {})
    },
    onDelete(){
      if(this.OrderID > 0){
          if(this.delete(this.OrderID)){
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
      this.OrderID = props.row.ID;
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
      this.OrderID = 0;
    }
  },
  computed: {
    ...mapGetters('order_store',['Orders'])
  },
  beforeMount(){
    this.get();
  }
})
</script>
