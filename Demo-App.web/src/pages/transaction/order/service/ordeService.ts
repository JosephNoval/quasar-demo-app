import { authHeader } from "src/core/axios/auth-header";
import http from "src/core/axios/http";
import { Order } from "../model/order";

export default class OrderService {
    async getAll () : Promise<Order[]> {
      
      var res = await http.get('order/get-all', authHeader());
      
      return res.data as Order[];
  
    }

    async getById (id: Number) : Promise<Order> {
      
      var res = await http.get(`order/get-order-by-id?id=${id}`, authHeader());
      
      return res.data as Order;
  
    }

    async addOrUpdate(data: Order) : Promise<any> {
      try {
        var res = await http.post('order/add-or-update', data, authHeader());

        return res.data;

      } catch (error) {
        return `An error occur while processing your request. ${error}`;
      }
    }

    async delete (id: Number) : Promise<String> {
      try {

        var res = await http.get(`order/delete?id=${id}`, authHeader());
        return res.data as String;

      } catch (error) {
        return `An error occur while processing your request. ${error}`;
      }
      
    }
  }
  
  // A singleton instance
  export const orderService = new OrderService();