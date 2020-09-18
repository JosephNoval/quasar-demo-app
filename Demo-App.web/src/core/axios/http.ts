import axios from 'axios';
export default axios.create({
    // baseURL: 'http://localhost:49327/api/',
    baseURL: 'http://192.168.0.100:1200/api/',
    headers: { 
        'Content-Type': 'application/json'
       }
  });