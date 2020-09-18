import { authHeader } from "src/core/axios/auth-header";
import http from "src/core/axios/http";
import { Customer } from "../model/customer";

export default class CustomerService {
    async getAll () : Promise<Customer[]> {
      
      var res = await http.get('customer/get-all', authHeader());
      
      return res.data as Customer[];
  
    }

    async getById (id: Number) : Promise<Customer> {
      
      var res = await http.get(`customer/get-customer-by-id?id=${id}`, authHeader());
      
      return res.data as Customer;
  
    }

    async addOrUpdate(data: Customer) : Promise<any> {
      try {
        var res = await http.post('customer/add-or-update', data, authHeader());

        return res.data;

      } catch (error) {
        return `An error occur while processing your request. ${error}`;
      }
    }

    async delete (id: Number) : Promise<String> {
      try {

        var res = await http.get(`customer/delete?id=${id}`, authHeader());
        return res.data as String;

      } catch (error) {
        return `An error occur while processing your request. ${error}`;
      }
      
    }
  }
  
  // A singleton instance
  export const customerService = new CustomerService();