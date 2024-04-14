import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = []; //=>to store the state of members

  constructor(private http:HttpClient) { }
  
  //=>To get the member list from the API endpoint(from UsersController) if its not loaded yet;
  getMembers(){
    //=>'of' from rxjs is used to return an 'Observable' to be subscribed in another place(e.g. member-list) if its directly returned with it's state without calling the API;
    if(this.members.length>0) return of(this.members);

    //Member[] => type safely member array will return from the API call;
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
       map(members => {
          this.members = members; //in order to load the members inside of members array if it hasnt any members yet to keep the state of it for the other requests.
          return members; 
       })
    )
  }

  //=>To get the specific member from the API endpoint(from UsersController) if its not assigned yet;
  getMember(username: string){
    const member = this.members.find(x => x.userName === username);
    if(member) return of(member); //=>if member is found already, its gonna be returned from it's state without calling the API.

    //Member => type safely member type result will return from the API call;
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }

  updateMember(member:Member){
     return this.http.put(this.baseUrl + 'users', member).pipe(
        map(() => {
           const index = this.members.indexOf(member);
           //=>updated member props will be assigned to the found member's props(elements) to update the member;
           this.members[index] = {...this.members[index], ...member} //spread operator('...')
        })
     )
  }

}
