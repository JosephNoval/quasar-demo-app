import { BaseEntity } from "src/core/model/BaseEntity";

export interface Item extends BaseEntity{
    Name: string;
    Description: string;
    Price: number;
}