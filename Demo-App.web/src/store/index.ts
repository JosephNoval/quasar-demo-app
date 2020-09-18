import { store } from 'quasar/wrappers';
import Vuex from 'vuex';
import auth_store from '../pages/account/store/authStore';
import customer_store from '../pages/master-data/customer/store/customerStore';
import item_store from '../pages/master-data/item/store/itemStore';
import order_store from '../pages/transaction/order/store/orderStore';

// import example from './module-example';
// import { ExampleStateInterface } from './module-example/state';

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */

export interface StateInterface {
  // Define your own store structure, using submodules if needed
  // example: ExampleStateInterface;
  // Declared as unknown to avoid linting issue. Best to strongly type as per the line above.
  example: unknown;
}

export default store(function ({ Vue }) {
  Vue.use(Vuex);

  const Store = new Vuex.Store<StateInterface>({
    modules: {
      // example
      auth_store,
      customer_store,
      item_store,
      order_store
    },

    // enable strict mode (adds overhead!)
    // for dev mode only
    strict: !!process.env.DEV
  });

  return Store;
});
