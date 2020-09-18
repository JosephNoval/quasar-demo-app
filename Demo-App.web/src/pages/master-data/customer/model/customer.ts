import { BaseEntity } from "src/core/model/BaseEntity";

export interface Customer extends BaseEntity{
    Firstname: string;
    Middlename: string;
    Lastname: string;
}