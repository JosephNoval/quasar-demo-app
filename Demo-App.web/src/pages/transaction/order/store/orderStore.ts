import { Utils } from "src/core/utils/utils";
import { Order } from "../model/order";
import { orderService } from "../service/ordeService";

const state =  {
    Orders:  [] as Order[]
}
const mutations = {
    setState(state: any, payload: Order[]) {
        for(var x=0;x<payload.length;x++){
            payload[x].CustomerName = `${payload[x].Customer?.Firstname} ${payload[x].Customer?.Middlename} ${payload[x].Customer?.Lastname}`;
            payload[x].RecordStatus = Utils.GetStatus(payload[x].RecordStatus);
            payload[x].PaymentMethod = Utils.GetPaymentType(payload[x].PaymentMethod);
        }
        state.Orders = payload;
    },
    addNew(state: any, payload: Order){
        payload.CustomerName = `${payload.Customer?.Firstname} ${payload.Customer?.Middlename} ${payload.Customer?.Lastname}`;
        payload.RecordStatus = Utils.GetStatus(payload.RecordStatus);
        payload.PaymentMethod = Utils.GetPaymentType(payload.PaymentMethod);
        state.Orders.push(payload);
    },
    update(state: any, payload: Order){
        payload.CustomerName = `${payload.Customer?.Firstname} ${payload.Customer?.Middlename} ${payload.Customer?.Lastname}`;
        payload.RecordStatus = Utils.GetStatus(payload.RecordStatus);
        payload.PaymentMethod = Utils.GetPaymentType(payload.PaymentMethod);

        state.Orders.forEach((val:any, key:any) => {
            if(val.ID == payload.ID){
                Object.assign(state.Orders[key], payload);
            }
        })
    },
    delete(state: any, payload: number){

        state.Orders.forEach((val:any, key:any) => {
            if(val.ID == payload){
                var newVal = val;
                newVal.RecordStatus = 'Deleted';
                Object.assign(state.Orders[key], newVal);
            }
        })
    }
}
const actions = {
    // @ts-ignore
    async get({ commit }) {
        
        var res = await orderService.getAll();
        commit('setState', res)
    },
    // @ts-ignore
    async addOrUpdate({ commit }, payload: Order){
        var res = await orderService.addOrUpdate(payload);
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
        var res = await orderService.getById(payload);
        if(res != null){
            commit('update', res)
            return true;
        }
        return false;
    },
    // @ts-ignore
    async delete({ commit }, payload: number){
        var res = await orderService.delete(payload);
        if(res == 'Success'){
            commit('delete', payload)
            return true;
        }
        return false;
    }
}
const getters = {
    Orders: (state: any) => {
        return state.Orders as Order[];
    },
    Order: (state: any, payload : any) => {
        var res = {} as Order;
        state.Orders.forEach((val:Order, key:any) => {
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