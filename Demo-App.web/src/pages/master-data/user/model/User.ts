import { BaseEntity } from "src/core/model/BaseEntity";

export interface User extends BaseEntity{
    DisplayName: string;
    UserName: string;
    Password: string;
}