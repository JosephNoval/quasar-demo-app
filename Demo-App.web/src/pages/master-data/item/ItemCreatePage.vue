<template>
  <q-page padding>
    <div class="q-pa-md no-border my-card" style="margin-top: -27px;">

      <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-xs q-pb-md">

        <q-separator />

        <div class="text-black text-left text-h5">Item Setup</div>

        <q-separator />

        <q-input
          outlined
          v-show="false"
          dense
          v-model="formData.ID"
          label="Hidden Id"
        />

        <q-input
          outlined
          dense
          ref="Name"
          v-model="formData.Name"
          label="Item Name"
          lazy-rules
          :rules="[ val => !!val || 'Please type Item Name']"
        />

        <q-input
          outlined
          dense
          v-model="formData.Description"
          label="Description"
          lazy-rules
          :rules="[ val => !!val || 'Please type Description']"
        />

        <q-input
          dense
          outlined
          ref="Price"
          v-model="formData.Price"
          label="Price"
          input-class="text-left"
          lazy-rules
          :rules="[
          val => val !== null && val !== '' || 'Please input Price',
          val => val > 0 || 'Please type a real Price'
        ]"
        />

        <q-select
          outlined
          :disable="formData.ID == 0"
          v-model="RecordStatusType"
          :options="RecordStatusTypeOptions"
          label="Record Status"
          dense
        />

        <app-buttons
          :showDeleteButton="showDeleteButton"
          :mclass="'q-gutter-xs q-pt-md'"
          :buttons="buttons"
          @on-confirm-dialog="onShowConfirmDeleteDialog" 
          @on-find="onFind"/>
        
      </q-form>
  
    </div>

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
import itemStore from 'pages/master-data/item/store/itemStore';
import { Utils } from 'src/core/utils/utils';
import { Item } from './model/item';
import { itemService } from './service/itemService';

export default Vue.extend({
  // name: 'PageName',
  components: { 
      'app-confirm-dialog': AppConfirmDialogComponent,
      'app-buttons': AppButtonComponent
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
      formData: { ID:0 } as Item,
      RecordStatusType: { label: 'Active', value: 0},
      RecordStatusTypeOptions: [
        {
          label: 'Active',
          value: 0
        },
        {
          label: 'Deleted',
          value: 1
        }
      ],
      // for buttons
      buttons: [
        { stretch: false, no_caps: true, icon: 'save', color: 'secondary', label: 'Save', type: 'submit' },
        { stretch: false, no_caps: true, icon: 'search', color: 'info', label: 'Find', click: 'on-find'},
        { stretch: false, no_caps: true, icon: 'clear', color: 'accent', label: 'Clear', type: 'reset' },
        { stretch: false, no_caps: true, icon: 'delete', color: 'negative', label: 'Delete', click: 'on-confirm-dialog' }
      ]
    }
  },
  methods: {
    ...mapActions('item_store',['addOrUpdate','delete','getById']),
    onSubmit() {
      this.formData.RecordStatus = this.RecordStatusType?.value;
      //console.log(this.formData)
      if(this.formData.ID == 0){
        this.addOrUpdate(this.formData).then(res => {
          if(res != 0){
            this.$router.push('/item/' + res).catch(err => {});
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
      this.formData = { ID:0 } as Item;
      this.RecordStatusType = { label: 'Active', value: 0};
      this.showDeleteButton = false;
      if (this.$route.params.ID !== undefined) {
        this.$router.push('/item').catch(err => {});
      }
      (this.$refs["Name"] as HTMLElement).focus()
    },
    onDelete(){
      if(this.formData.ID > 0){
          if(this.delete(this.formData.ID)){
            this.formData.RecordStatus = 1;
            this.RecordStatusType = {
              label: 'Deleted',
              value: 1
            }
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
      if(this.RecordStatusType?.value == 0){
        this.showDeleteButton = true;
      }
      else{
        this.showDeleteButton = false;
      }
    },
    onFind(){
      this.$router.push('/item-list').catch(err => {});
    }
  },
  async mounted(){
    if(this.$route.params.ID !== undefined){
      var _id = this.$route.params.ID;
      var model = await itemService.getById(Number(_id));
      if(model){
        this.formData = model;
        this.RecordStatusType = {
          label: Utils.GetStatus(this.formData.RecordStatus),
          value: Number(this.formData.RecordStatus)
        }
        //show delete button
        this.showButtons();
      }
      else{
        this.$router.push('/item').catch(err => {});
      }
    }
    (this.$refs["Name"] as HTMLElement).focus()
  },
  beforeMount(){
    if(this.$route.params.ID !== undefined){
      this.getById(this.$route.params.ID);
    }
  }
})
</script>

