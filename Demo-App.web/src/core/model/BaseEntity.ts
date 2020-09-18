import { RecordStatusType } from "src/core/enum/enum";

export interface BaseEntity{
    ID: number;
    RecordStatus: RecordStatusType | string;
    CreatedOn: Date;
    CreatedBy: string;
    UpdatedOn: Date;
    UpdatedBy: string;
    Remarks: string;
}