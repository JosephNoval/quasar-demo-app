<template>
  <div class="flex q-pa-md">
    <q-card class="full-width">
      
      <q-tab-panels
        v-model="panel"
        animated
        swipeable
        infinite
        class="text-white shadow-2 rounded-borders fixed-center"
        style="width:500px"
      >
        <q-tab-panel name="login">

          <div class="text-black text-center">
            <h4 class="q-mb-md">1028 Business Consultancy</h4>
          </div>

          <q-separator />

          <div class="text-black text-center">
            <h5 class="q-mt-md">Login</h5>
          </div>

          <q-form @submit="onSubmit" class="q-gutter-md q-pb-md">
            <q-input
              outlined
              ref="userName"
              v-model="formData.Username"
              label="Username"
              lazy-rules
              :rules="[ val => !!val || 'Please type username']" 
              @keyup.esc="clearUserName">
              <template v-slot:append>
                <q-icon
                  v-if="formData.Username !== ''"
                  name="close"
                  @click="clearUserName"
                  class="cursor-pointer"
                  title="Press ESC to Clear" />
              </template>
            </q-input>

            <q-input
              v-model="formData.Password"
              outlined
              ref="password"
              label="Password"
              :type="isPwd ? 'password' : 'text'"
              lazy-rules
              :rules="[ val => !!val || 'Please type password']" 
              @keyup.esc="clearPassword">
              <template v-slot:append>
                <q-icon
                  v-if="formData.Password !== ''"
                  name="close"
                  @click="clearPassword"
                  class="cursor-pointer"
                  title="Press ESC to Clear" />
                <q-icon
                  :name="isPwd ? 'visibility_off' : 'visibility'"
                  class="cursor-pointer"
                  @click="isPwd = !isPwd"
                  :title="isPwd ? 'Show' : 'Hide'"
                />
              </template>
            </q-input>
            <div>
              <q-btn
                no-caps
                icon="input"
                label="Sign In"
                title="Sign In"
                type="submit"
                color="secondary" />
            </div>
          </q-form>

          <q-separator />

          <div class="text-black text-center text-caption q-pt-md">
            Powered by 1028 Business Consultancy Inc.
          </div>

        </q-tab-panel>
      </q-tab-panels>
    </q-card>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { mapActions } from "vuex";

export default Vue.extend({
  name: "LoginPage",
  data() {
    return {
      panel: "login",
      isPwd: true,
      formData: {
        Username: '',
        Password: ''
      }
    };
  },

  methods: {
    ...mapActions('auth_store',['loginUser']),
    onSubmit() {
      this.loginUser(this.formData).then(response => {
        if(response){
          this.$q.notify({
            color: "positive",
            textColor: "white",
            icon: "cloud_done",
            position: "top-right",
            message: "Successfully Login"
          });
          this.$router.replace('/').catch(err => {});
        }
        else{
          this.$q.notify({
            color: "negative",
            textColor: "white",
            icon: "warning",
            position: "top-right",
            message: "Incorrect Username and Password"
          });
          this.$router.replace('/auth').catch(err => {});
          (this.$refs["userName"] as HTMLElement).focus();
        }
      })
      .catch(error => {
        console.log(error.message)
      })
    },
    clearUserName(){
      this.formData.Username = '';
      (this.$refs["userName"] as HTMLElement).focus()
    },
    clearPassword(){
      this.formData.Password = '';
      (this.$refs["password"] as HTMLElement).focus()
    }
  },
  mounted() {
    (this.$refs["userName"] as HTMLElement).focus()
  }
});
</script>