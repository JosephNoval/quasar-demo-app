import { User } from "src/pages/master-data/user/model/User";

export class Auth {
    isLogIn: boolean = false;
    Token: string = '';
    User: User = new User();
}