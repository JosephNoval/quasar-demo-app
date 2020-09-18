import { SessionStorage } from 'quasar';
import { Auth } from 'src/pages/account/model/auth';

export function authHeader(): any {
    // return authorization header with jwt token
    let auth = SessionStorage.getItem('auth') as Auth;
    if (auth != null && auth.Token != null) {
        //return `Bearer ${auth.Token}`;
        return { headers: {'Authorization': 'Bearer ' + auth.Token} }
    } else {
        return {};
    }
}