import { Utils } from "src/core/utils/utils";
import { Item } from "../model/item";
import { itemService } from "../service/itemService";

const state =  {
    Items:  [] as Item[]
}
const mutations = {
    setState(state: any, payload: Item[]) {
        for(var x=0;x<payload.length;x++){
            payload[x].RecordStatus = Utils.GetStatus(payload[x].RecordStatus);
        }
        state.Items = payload;
    },
    addNew(state: any, payload: Item){
        payload.RecordStatus = Utils.GetStatus(payload.RecordStatus);
        state.Items.push(payload);
    },
    update(state: any, payload: Item){

        payload.RecordStatus = Utils.GetStatus(payload.RecordStatus);

        state.Items.forEach((val:any, key:any) => {
            if(val.ID == payload.ID){
                Object.assign(state.Items[key], payload);
            }
        })
    },
    delete(state: any, payload: number){

        state.Items.forEach((val:any, key:any) => {
            if(val.ID == payload){
                var newVal = val;
                newVal.RecordStatus = 'Deleted';
                Object.assign(state.Items[key], newVal);
            }
        })
    }
}
const actions = {
    // @ts-ignore
    async get({ commit }) {
        
        var res = await itemService.getAll();
        commit('setState', res)
    },
    // @ts-ignore
    async addOrUpdate({ commit }, payload: Item){
        var res = await itemService.addOrUpdate(payload);
        if(res != 0){
            if(payload.ID == 0){
                payload.ID = res;
                commit('addNew', payload)
                return res;
            }
            else{
                commit('update', payload)
                return res;
            }
        }
        return 0;
    },
    // @ts-ignore
    async getById({ commit }, payload: Number){
        var res = await itemService.getById(payload);
        if(res != null){
            commit('update', res)
            return true;
        }
        return false;
    },
    // @ts-ignore
    async delete({ commit }, payload: number){
        var res = await itemService.delete(payload);
        if(res == 'Success'){
            commit('delete', payload)
            return true;
        }
        return false;
    }
}
const getters = {
    Items: (state: any) => {
        return state.Items as Item[];
    },
    Item: (state: any, payload : any) => {
        var res = {} as Item;
        state.Items.forEach((val:Item, key:any) => {
            if(val.ID == payload){
                res = val;
            }
        });
        return res;
    }
}
export default {
    namespaced: true,
    state,
    mutations,
    actions,
    getters
}