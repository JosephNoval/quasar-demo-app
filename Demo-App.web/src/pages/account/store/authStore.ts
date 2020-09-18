import { SessionStorage } from "quasar";
import { User } from "src/pages/master-data/user/model/User";
import { Auth } from "../model/auth";
import { authService } from "../service/authService";

let value = SessionStorage.getItem('auth') as Auth
const state = {
    auth: {
        isLogIn: value ? value.isLogIn : false,
        Token: value ? value.Token : '',
        User: value ? value.User : {} as User
      } as Auth
}
const mutations = {
    setAuthState(state: any, payload: Auth) {
      state.auth = payload;
    },
    clearAuthState(state: any){
      state.auth.isLogIn = false;
      state.auth.Token = '';
      state.auth.User = {} as User;
      SessionStorage.remove('auth');
    }
}
const actions = {
    // @ts-ignore
    async loginUser({ commit }, payload: any) {
      var res = await authService.login(payload)
      if(res.Message == 'Success'){
        var authState = {
          isLogIn: true,
          Token: res.Token,
          User: res.User
        };
        commit('setAuthState', authState)
        SessionStorage.set('auth', authState);
        
        return true;
      }
      else{
        return false;
      }
    },
    // @ts-ignore
    logoutUser({ commit }) {
      commit('clearAuthState')
    }
}
const getters = {
    auth: (state: any) => {
        return state.auth
      }
}
export default {
    namespaced: true,
    state,
    mutations,
    actions,
    getters
}