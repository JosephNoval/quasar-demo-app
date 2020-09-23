import { BaseEntity } from "src/core/model/BaseEntity";
import { Customer } from "src/pages/master-data/customer/model/customer";
import { Item } from "src/pages/master-data/item/model/item";

export interface Order extends BaseEntity{
    OrderNo: string;
    CustomerId: number;
    CustomerName: string;
    PaymentMethod: PaymentType;
    Total: number;

    Details: Array<OrderDetails>;
    Customer: Customer;
}

export interface OrderDetails extends BaseEntity{
    OrderId: number;
    ItemId: number;
    ItemName: string;
    Quantity: number;
    Price: number;

    Item: Item;
}

export enum PaymentType {
    Cash = 0,
    Credit = 1
}