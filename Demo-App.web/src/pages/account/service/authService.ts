import http from "src/core/axios/http";
import { SignInModel } from "../model/SignInModel";

export default class AuthService {
    async login (model: SignInModel) : Promise<any> {
        try {
          
          var res = await http.post('account/login', model);

          return res.data;
    
        } catch (error) {
          return "An error occur while processing your request.";
        }
    }
  }
  
  // A singleton instance
  export const authService = new AuthService();