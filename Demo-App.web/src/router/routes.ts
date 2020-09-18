import { RouteConfig } from 'vue-router';

const routes: RouteConfig[] = [
  //login
  { path: '/auth', name: 'Login', component: () => import('pages/account/LoginPage.vue')},
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    meta: {
      requiresAuth: true
    },
    children: [
      { path: '', component: () => import('pages/Index.vue') },
      { path: 'customer-list', name: 'customer', component: () => import('pages/master-data/customer/CustomerListPage.vue') },
      { path: 'customer/:ID?', name: 'customer', component: () => import('pages/master-data/customer/CustomerCreatePage.vue') },
      { path: 'item-list', name: 'item', component: () => import('pages/master-data/item/ItemListPage.vue') },
      { path: 'item/:ID?', name: 'item', component: () => import('pages/master-data/item/ItemCreatePage.vue') },
      { path: 'order-list', name: 'order', component: () => import('pages/transaction/order/OrderListPage.vue') },
      { path: 'order/:ID?', name: 'order', component: () => import('pages/transaction/order/OrderCreatePage.vue') }
    ]
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '*',
    component: () => import('pages/Error404.vue')
  }
];

export default routes;
