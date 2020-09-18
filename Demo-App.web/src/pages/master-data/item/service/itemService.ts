import { authHeader } from "src/core/axios/auth-header";
import http from "src/core/axios/http";
import { Item } from "../model/item";

export default class ItemService {
    async getAll () : Promise<Item[]> {
      
      var res = await http.get('item/get-all', authHeader());
      
      return res.data as Item[];
  
    }

    async getById (id: Number) : Promise<Item> {
      
      var res = await http.get(`item/get-item-by-id?id=${id}`, authHeader());
      
      return res.data as Item;
  
    }

    async addOrUpdate(data: Item) : Promise<any> {
      try {
        var res = await http.post('item/add-or-update', data, authHeader());

        return res.data;

      } catch (error) {
        return `An error occur while processing your request. ${error}`;
      }
    }

    async delete (id: Number) : Promise<String> {
      try {

        var res = await http.get(`item/delete?id=${id}`, authHeader());
        return res.data as String;

      } catch (error) {
        return `An error occur while processing your request. ${error}`;
      }
      
    }
  }
  
  // A singleton instance
  export const itemService = new ItemService();