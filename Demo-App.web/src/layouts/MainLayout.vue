<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="leftDrawerOpen = !leftDrawerOpen"
          :title="leftDrawerOpen ? 'Hide Drawer' : 'Show Drawer'"
        />

        <q-toolbar-title class="absolute-center">
          <q-icon style="font-size: 28px; padding-bottom: 6px;" :name="icon" />
          {{ title }}
        </q-toolbar-title>

        <q-btn
          v-if="!$q.platform.is.cordova"
          dense
          flat
          round
          class="q-ma-xs"
          @click="goFullscreen"
          :icon="fullscreen ? 'fullscreen_exit' : 'fullscreen'"
          :title="fullscreen ? 'Exit Fullscreen' : 'Go Fullscreen'"
        />

        <q-btn
          @click="logout"
          dense
          flat
          label="Logout"
          title="Logout"
          no-caps
          class="absolute-right q-pr-sm"
          icon="exit_to_app" />
      </q-toolbar>
    </q-header>

    <q-footer elevated v-if="($route.name != 'Order') || ($route.path.toString().includes('order-list'))">
      <q-toolbar>
        <q-toolbar-title class="text-caption">Powered by: 1028 Business Consultancy</q-toolbar-title>
      </q-toolbar>
    </q-footer>

    <q-drawer
        v-model="leftDrawerOpen"
        show-if-above
        :width="250"
        :breakpoint="1100"
      >
        <q-scroll-area style="height: calc(100% - 150px); margin-top: 150px; border-right: 1px solid #ddd">
          <q-list padding>
            <!-- master data -->
            <q-expansion-item expand-separator label="Master Data" icon="storage" :content-inset-level=".5">
              <q-item to="/customer-list" clickable v-ripple>
                <q-item-section avatar>
                  <q-icon name="people" />
                </q-item-section>
                <q-item-section>
                  Customer
                </q-item-section>
              </q-item>

              <q-item to="/item-list" clickable v-ripple>
                <q-item-section avatar>
                  <q-icon name="business_center" />
                </q-item-section>
                <q-item-section>
                  Item
                </q-item-section>
              </q-item>
            </q-expansion-item>
            <!-- transaction -->
            <q-expansion-item expand-separator label="Transaction" icon="construction" :content-inset-level=".5">
              <q-item to="/order-list" clickable v-ripple>
                <q-item-section avatar>
                  <q-icon name="shopping_basket" />
                </q-item-section>
                <q-item-section>
                  Order
                </q-item-section>
              </q-item>
            </q-expansion-item>
          </q-list>
        </q-scroll-area>

        <q-img class="absolute-top" src="https://cdn.quasar.dev/img/material.png" style="height: 150px">
          <div class="absolute-bottom bg-transparent">
            <q-avatar size="56px" class="q-mb-sm">
              <img src="https://cdn.quasar.dev/img/boy-avatar.png">
            </q-avatar>
            <div class="text-weight-bold">Razvan Stoenescu</div>
            <div>@rstoenescu</div>
          </div>
        </q-img>
      </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script lang="ts">
//import EssentialLink from 'components/EssentialLink.vue'

import { ref } from '@vue/composition-api';
import { AppFullscreen } from 'quasar';
import { mapActions, mapState } from "vuex";
import { Route } from 'vue-router';
import Vue from 'vue';

export default Vue.extend({
  name: 'MainLayout',
  //components: { EssentialLink },
  data () {
    return {  
      leftDrawerOpen: false,
      title: '' as String | null | undefined,
      icon: '',
      fullscreen: false
    }
  },
  watch: {
    //change title when route change
    '$route': {
      handler: function(to: Route): void {
        // Do something here.
        this.setTitleIcon(to);
      },
      immediate: true,
    },
    // change fullscreen status when fullscreen status change
    '$q.fullscreen.isActive':function(val) {
        this.fullscreen = val;
    }
  },
  methods:{
    ...mapActions('auth_store',['logoutUser']),
    setTitleIcon(to: Route){
      this.title = to.name;
      //for icon setting
      switch(this.title?.toLowerCase()){
            //master-data
            case "customer":
                this.icon = 'people'
                break;
            case "item":
                this.icon = 'business_center'
                break;
            //transaction
            case "order":
                this.icon = 'shopping_basket'
                break;
            default:
                this.icon = ''
                break;
        }
    },
    logout(){
      this.logoutUser();
      // @ts-ignore
      this.$router.replace('/auth').catch(err => {})
    },
    goFullscreen(){
      if(this.fullscreen){
        // Exiting fullscreen modes
        AppFullscreen.exit()
          .then(() => {})
          .catch(err => {})
      }
      else{
        AppFullscreen.request()
        .then(() => {})
        .catch(err => {})
      }
    }
  }
});
</script>
