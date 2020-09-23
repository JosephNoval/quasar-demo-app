import { Utils } from "src/core/utils/utils";
import { Customer } from "../model/customer";
import { customerService } from "../service/customerService";

const state =  {
    Customers:  [] as Customer[]
}
const mutations = {
    setState(state: any, payload: Customer[]) {
        for(var x=0;x<payload.length;x++){
            payload[x].RecordStatus = Utils.GetStatus(payload[x].RecordStatus);
        }
        state.Customers = payload;
    },
    addNew(state: any, payload: Customer){
        payload.RecordStatus = Utils.GetStatus(payload.RecordStatus);
        state.Customers.push(payload);
    },
    update(state: any, payload: Customer){

        payload.RecordStatus = Utils.GetStatus(payload.RecordStatus);

        state.Customers.forEach((val:any, key:any) => {
            if(val.ID == payload.ID){
                Object.assign(state.Customers[key], payload);
            }
        })
    },
    delete(state: any, payload: number){

        state.Customers.forEach((val:any, key:any) => {
            if(val.ID == payload){
                var newVal = val;
                newVal.RecordStatus = 'Deleted';
                Object.assign(state.Customers[key], newVal);
            }
        })
    }
}
const actions = {
    // @ts-ignore
    async getCustomers({ commit }) {
        
        var res = await customerService.getAll();
        commit('setState', res)
    },
    // @ts-ignore
    async addOrUpdate({ commit }, payload: Customer){
        var res = await customerService.addOrUpdate(payload);
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
        var res = await customerService.getById(payload);
        if(res != null){
            commit('update', res)
            return true;
        }
        return false;
    },
    // @ts-ignore
    async delete({ commit }, payload: number){
        var res = await customerService.delete(payload);
        if(res == 'Success'){
            commit('delete', payload)
            return true;
        }
        return false;
    }
}
const getters = {
    Customers: (state: any) => {
        return state.Customers as Customer[];
    },
    Customer: (state: any, payload : any) => {
        var res = {} as Customer;
        state.Customers.forEach((val:Customer, key:any) => {
            if(val.ID == payload){
                res = val;
            }
        });
        return res;
    },
    ActiveCustomers: (state: any) => {
        var modelFiltered = [] as  Customer[];
        state.Customers.forEach((val:any, key:any) => {
            if(val.RecordStatus == 'Active'){
                modelFiltered.push(val);
            }
        });
        return modelFiltered;
    }
}
export default {
    namespaced: true,
    state,
    mutations,
    actions,
    getters
}