import { User } from "src/pages/master-data/user/model/User";

export interface Auth {
    isLogIn: boolean;
    Token: string;
    User: User;
}