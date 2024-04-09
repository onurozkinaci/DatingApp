import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  constructor(private http:HttpClient) { }
  
  //=>To get the member list from the API endpoint(from UsersController);
  getMembers(){
    //Member[] => type safely member array will return from the API call;
    return this.http.get<Member[]>(this.baseUrl + 'users')
  }

  //=>To get the specific member from the API endpoint(from UsersController);
  getMember(username: string){
    //Member => type safely member type result will return from the API call;
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }

  updateMember(member:Member){
     return this.http.put(this.baseUrl + 'users', member)
  }

}
