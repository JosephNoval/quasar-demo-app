import { BaseEntity } from "src/core/model/BaseEntity";

export class Customer extends BaseEntity{
    Firstname: string = '';
    Middlename: string = '';
    Lastname: string = '';
}