export class Utils{
    // get status
    public static GetStatus(args: any): any {
        var ret: any;
      
        if(typeof args == 'number'){
            if(args == 0){
                ret = 'Active';
            }
            else if(args == 1){
                ret = 'Deleted';
            }
        }
        else if(typeof args == 'string'){
            if(args == 'Active'){
                ret = 0;
            }
            else if(args == 'Deleted'){
                ret = 1;
            }
        }
        return ret;
    }
}