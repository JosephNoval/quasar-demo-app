import { BaseEntity } from "src/core/model/BaseEntity";

export class User extends BaseEntity{
    DisplayName: string = '';
    UserName: string = '';
    Password: string = '';
}