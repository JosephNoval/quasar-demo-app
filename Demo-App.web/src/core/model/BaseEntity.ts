import { RecordStatusType } from "src/core/enum/enum";

export class BaseEntity{
    ID: number = 0;
    RecordStatus: RecordStatusType | string = RecordStatusType.Active;
    CreatedOn: Date = new Date();
    CreatedBy: string = '';
    UpdatedOn: Date = new Date();
    UpdatedBy: string = '';
    Remarks: string = '';
}