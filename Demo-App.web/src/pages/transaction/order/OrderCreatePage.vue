<template>
  <q-page padding>
    <div class="q-pa-md no-border my-card" style="margin-top: -27px;">

      <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-xs q-pb-md">

        <q-separator />

        <div class="text-black text-left text-h5">Order Setup</div>

        <q-separator />
        
        <app-buttons
          :showDeleteButton="showDeleteButton"
          :mclass="'q-gutter-xs q-pt-md'"
          :buttons="buttons"
          @on-confirm-dialog="onShowConfirmDeleteDialog" 
          @on-find="onFind"/>
        
        <q-input
          outlined
          v-show="false"
          dense
          v-model="formData.ID"
          label="Hidden Id"
        />

        <q-input
          outlined
          readonly
          dense
          v-model="formData.OrderNo"
          label="Order No"
        />

        <q-input
          outlined
          dense
          readonly
          v-model="formData.CustomerName"
          label="Customer"
          lazy-rules
          :rules="[ val => !!val || 'Please choose Customer']">
          <template v-slot:after>
            <q-btn
              dense
              title="Pick Customer"
              @click="pickCustomer"
              color="primary"
              icon="search" />
          </template>
        </q-input>

        <q-select
          outlined
          v-model="PaymentMethodType"
          :options="PaymentMethodTypeOptions"
          label="Payment Type"
          dense
        />

        <q-input
          readonly
          dense
          outlined
          v-model="formData.Total"
          label="Total"
          input-class="text-left"
        />

      </q-form>
  
    </div>
    <!-- table for order details -->
    <div class="q-pa-md">
      <app-list
        :mstyle="'margin-top: -27px; height: 350px !important;'"
        :mclass="'cursor-pointer'"
        :showFilter="false"
        :buttons="orderDetailButtons"
        :tableButtons="tableButtons"
        :data="formData.Details"
        :columns="orderDetailsColumns"
        @pick-item="pickItem"
        @on-remove-order-item="onRemoveOrderItem"
        @on-generate-total="generateTotal" />
    </div>

    <!-- modal picker -->
    <app-list-picker
      :showModal="showModal"
      :modalIcon="modalIcon"
      :modalTitle="modalTitle"
      :modalTableData="modalData"
      :modalTableColumns="modalColumns"
      :modalTableSelectionType="modalSelectionType"
      :modalButtons="modalButtons"
      :showFilter="showFilter"
      :filter="modalFilter"
      :modalSelected="modalSelected"
      @on-pick-customer="onPickCustomer"
      @on-pick-item="onPickItem"
      @on-close-modal="onCloseModal"
      @on-row-dbl-click="onRowDblClick"
      @on-modal-filter-change="onModalFilterChange"
      @on-modal-selected-change="onModalSelectedChange" />

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

  </q-page>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapActions } from 'vuex';
import AppConfirmDialogComponent from "components/core/app-confirm-dialog.vue";
import AppButtonComponent from "components/core/app-button-component.vue";
import AppPickerComponent from "components/core/app-list-picker.vue";
import AppListComponent from "components/core/app-list-component.vue";
import itemStore from 'pages/master-data/item/store/itemStore';
import { Utils } from 'src/core/utils/utils';
import { Order, OrderDetails } from './model/order';
import { orderService } from './service/ordeService';
import { Item } from 'src/pages/master-data/item/model/item';
import { Customer } from 'src/pages/master-data/customer/model/customer';

export default Vue.extend({
  // name: 'PageName',
  components: { 
      'app-confirm-dialog': AppConfirmDialogComponent,
      'app-buttons': AppButtonComponent,
      'app-list-picker': AppPickerComponent,
      'app-list': AppListComponent
  },
  data(){
    return {
      // for confirm dialog
      dialogTitle: '',
      dialogMessage: '',
      dialogIcon: '',
      dialogIconColor: '',
      dialogButtons: [],
      showDialog: false,
      showDeleteButton: false,
      // for form data
      formData: { 
        ID:0, 
        OrderNo: Math.floor(100000 + Math.random() * 900000).toString(),
        Details: [] as OrderDetails[] 
      } as Order,
      orderDetailsData: {} as OrderDetails,
      PaymentMethodType: { label: '', value: -1},
      PaymentMethodTypeOptions: [
        {
          label: 'Cash',
          value: 0
        },
        {
          label: 'Credit',
          value: 1
        }
      ],
      // for buttons
      buttons: [
        { stretch: false, no_caps: true, icon: 'save', color: 'secondary', label: 'Save', type: 'submit' },
        { stretch: false, no_caps: true, icon: 'search', color: 'info', label: 'Find', click: 'on-find'},
        { stretch: false, no_caps: true, icon: 'clear', color: 'accent', label: 'Clear', type: 'reset' },
        { stretch: false, no_caps: true, icon: 'delete', color: 'negative', label: 'Delete', click: 'on-confirm-dialog' }
      ],
      customerColumns: [
        { name: 'ID', align: 'center', label: 'ID', field: 'ID', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Firstname', align: 'center',label: 'Firstname', field: 'Firstname', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Middlename', align: 'center',label: 'Middlename', field: 'Middlename', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Lastname', align: 'center',label: 'Lastname', field: 'Lastname', sortable: true, headerClasses: 'bg-secondary text-white' }
      ],
      itemColumns: [
        { name: 'ID', align: 'center', label: 'ID', field: 'ID', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Name', align: 'center',label: 'Name', field: 'Name', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Description', align: 'center',label: 'Description', field: 'Description', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Price', align: 'center',label: 'Price', field: 'Price', sortable: true, headerClasses: 'bg-secondary text-white' }
      ],
      modalData: [] as Array<any>,
      modalColumns: [] as Array<any>,
      modalVisibleColumns: [] as Array<any>,
      modalSelectionType: '',
      modalSelected: [] as Array<any>,
      modalButtons: [] as Array<any>,
      modalFilter: '',
      modalTitle: '',
      modalIcon: '',
      showModal: false,
      showFilter: false,
      modalName: '',

      //for order details
      orderDetailsColumns: [
        { name: 'ItemId', align: 'center', label: 'Item Id', field: 'ItemId', sortable: true, headerClasses: 'bg-secondary text-white' },
        // @ts-ignore
        { name: 'ItemName', align: 'center',label: 'Item Name', field: row => row.Item.Name, sortable: true, headerClasses: 'bg-secondary text-white' },
        // @ts-ignore
        { name: 'Description', align: 'center',label: 'Description', field: row => row.Item.Description, sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'E_InputQuantity', align: 'center',label: 'Quantity', field: 'Quantity', sortable: true, headerClasses: 'bg-secondary text-white' },
        { name: 'Price', align: 'center',label: 'Price', field: 'Price', sortable: true, headerClasses: 'bg-secondary text-white' }
      ],
      tableButtons: [
        { icon: 'delete', color: 'negative', size: 'sm', label: 'Delete Item', click: 'on-remove-order-item' }
      ],
      //for order details buttons
      orderDetailButtons: [
        { icon: 'add_circle', color: 'positive', label: 'Add Item', title: 'Add Item', click: 'pick-item' }
      ]
    }
  },
  methods: {
    ...mapActions('item_store',['getItems']),
    ...mapActions('customer_store',['getCustomers']),
    ...mapActions('order_store',['addOrUpdate','delete','getById']),
    onSubmit() {
      if(this.PaymentMethodType?.value == -1){
        this.$q.notify({
          color: "negative",
          textColor: "white",
          icon: "warning",
          position: "top-right",
          message: "Please Select Payment Method"
        });
        return;
      }
      if(this.formData.Details.length == 0){
        this.$q.notify({
          color: "negative",
          textColor: "white",
          icon: "warning",
          position: "top-right",
          message: "Please Select Order Items"
        });
        return;
      }
      this.formData.PaymentMethod = this.PaymentMethodType?.value;
      //console.log(this.formData)
      if(this.formData.ID == 0){
        this.addOrUpdate(this.formData).then(res => {
          if(res != 0){
            this.$router.push('/order/' + res).catch(err => {});
            this.$q.notify({
              color: "green-4",
              textColor: "white",
              icon: "cloud_done",
              position: "top-right",
              message: "Successfully Save"
            });
            //show delete button
            this.showButtons();
          }
          else{
            this.$q.notify({
              color: "negative",
              textColor: "white",
              icon: "warning",
              position: "top-right",
              message: "Save was not successfull"
            });
          }
        })
      }
      else{
        this.addOrUpdate(this.formData).then(res => {
          if(res != 0){
            this.$q.notify({
              color: "green-4",
              textColor: "white",
              icon: "cloud_done",
              position: "top-right",
              message: "Successfully Updated"
            });
            //show delete button
            this.showButtons();
          }
          else{
            this.$q.notify({
              color: "negative",
              textColor: "white",
              icon: "warning",
              position: "top-right",
              message: "Update was not successfull"
            });
          }
        })
      }
    },
    onReset() {
      this.formData = { 
        ID:0, 
        OrderNo: Math.floor(100000 + Math.random() * 900000).toString(),
        Details: [] as OrderDetails[] 
      } as Order;
      this.PaymentMethodType = { label: '', value: -1};
      this.showDeleteButton = false;
      if (this.$route.params.ID !== undefined) {
        this.$router.push('/order').catch(err => {});
      }
    },
    onDelete(){
      if(this.formData.ID > 0){
          if(this.delete(this.formData.ID)){
            this.formData.RecordStatus = 1;
              this.$q.notify({
                color: "green-4",
                textColor: "white",
                icon: "cloud_done",
                position: "top-right",
                message: "Successfully Deleted"
              });
            //show delete button
            this.showButtons();
          }
      }
      this.showDialog = false;
    },
    onRemoveOrderItem(item: any){
      this.formData.Details.splice(item.pageIndex,1);
      this.generateTotal();
    },
    onShowConfirmDeleteDialog(){
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
    },
    showButtons(){
      //show delete button
      if(this.formData.RecordStatus == 0 || this.formData.RecordStatus == 'Active'){
        this.showDeleteButton = true;
      }
      else{
        this.showDeleteButton = false;
      }
    },
    onFind(){
      this.$router.push('/order-list').catch(err => {});
    },
    onPickCustomer(selected:any){
      this.showModal = false;
      // @ts-ignore
      this.formData.CustomerId = selected[0]?.ID;
      this.formData.Customer = selected[0] as Customer;
      this.formData.CustomerName = `${selected[0]?.Firstname} ${selected[0]?.Middlename} ${selected[0]?.Lastname}`;
      this.onCloseModal();
    },
    onPickItem(selected: any){
      for (let x = 0; x < selected.length; x++) {
        var element = selected[x] as Item;
        this.formData.Details.forEach(y => {
          if(element.ID == y.ItemId){
            y.Quantity = +y.Quantity + 1;
          }
        });
        var _orderDetails = {
          OrderId : this.formData.ID,
          ItemId : element.ID,
          ItemName : element.Name,
          Item : element,
          Quantity : 1,
          Price : element.Price
        } as OrderDetails;
        if(this.formData.Details.filter(z => z.ItemId == element.ID).length == 0){
          this.formData.Details.push(_orderDetails);
        }
      }
      this.onCloseModal();
      this.generateTotal();
    },
    pickCustomer(){
      this.modalButtons = [{ label: 'Choose', icon: 'touch_app', color: 'accent', click: 'on-pick-customer'}];
      this.modalTitle = 'CUSTOMER LIST';
      this.modalIcon = 'people';
      this.showModal = true;
      this.modalData = this.ActiveCustomers;
      this.modalColumns = this.customerColumns;
      this.modalSelectionType = 'single';
      this.modalSelected = [];
      this.showFilter = true;
      this.modalFilter = '';
      this.modalName = 'Customer';
    },
    pickItem(){
      this.modalButtons = [{ label: 'Choose', icon: 'touch_app', color: 'accent', click: 'on-pick-item'}];
      this.modalTitle = 'ITEM LIST';
      this.modalIcon = 'business_center';
      this.showModal = true;
      this.modalData = this.ActiveItems;
      this.modalColumns = this.itemColumns;
      this.modalSelectionType = 'multiple';
      this.modalSelected = [];
      this.showFilter = true;
      this.modalFilter = '';
      this.modalName = 'Item'
    },
    onRowDblClick(selected:any) {
      if(this.modalName.toLowerCase().includes('customer')){
        this.onPickCustomer(selected);
      }
      else if(this.modalName.toLowerCase().includes('item')){
        this.onPickItem(selected);
      }
    },
    onCloseModal(){
      this.modalButtons = [];
      this.modalTitle = '';
      this.modalIcon = '';
      this.showModal = false;
      this.modalData = [];
      this.modalColumns = [];
      this.modalVisibleColumns = [];
      this.modalSelectionType = '';
      this.modalSelected = [];
      this.showFilter = false;
      this.modalFilter = '';
    },
    onModalFilterChange(value:any){
      this.modalFilter = value;
    },
    onModalSelectedChange(value: any){
      this.modalSelected = value;
    },
    generateTotal(){
      var _total = 0;
      this.formData.Details.forEach(x => {
        _total += x.Quantity * x.Price;
      });
      this.formData.Total = _total;
    }
    
  },
  computed: {
    ...mapGetters('item_store',['ActiveItems']),
    ...mapGetters('customer_store',['ActiveCustomers'])
  },
  async mounted(){
    if(this.$route.params.ID !== undefined){
      var _id = this.$route.params.ID;
      var model = await orderService.getById(Number(_id));
      if(model){
        // if(!model.Total.toString().includes('.')){
        //   model.Total = Number(`${model.Total}00`)
        // }
        this.formData = model;
        this.formData.CustomerName = `${model.Customer.Firstname} ${model.Customer.Middlename} ${model.Customer.Lastname}`;
        this.PaymentMethodType = {
          label: Utils.GetPaymentType(this.formData.PaymentMethod),
          value: Number(this.formData.PaymentMethod)
        }
        //show delete button
        this.showButtons();
      }
      else{
        this.$router.push('/order').catch(err => {});
      }
    }
  },
  beforeMount(){
    this.getItems();
    this.getCustomers();
    if(this.$route.params.ID !== undefined){
      this.getById(this.$route.params.ID);
    }
  }
})
</script>

