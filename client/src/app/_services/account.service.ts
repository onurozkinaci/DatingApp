import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;

  //=>This is defined to store user's login info persisent with a special Observable;
  private currentUserSource = new BehaviorSubject<User|null>(null);
  currentUser$ = this.currentUserSource.asObservable(); //observable definition with $ ends of it as convention.

  constructor(private http: HttpClient) { }
  login(model:any){
    //=>http.post metoduna map'ten donecegin response tipini ipucu olarak (<User> gibi) vermen gerekiyor;
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map((response:User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user); //=>to keep the user info persistent
        }
      })
    )
  }
  
  register(model:any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
       map(user => {
          if(user){
            this.setCurrentUser(user); //=>to keep the user info persistent
          }
       })
    )
  }

  //=>In order to keep the user info persistent
  setCurrentUser(user:User){
    localStorage.setItem('user',JSON.stringify(user)); //the login state of the user will be stored when he/she logs in or registered.
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user'); //the login state of the user will be removed when he/she logs out.
    this.currentUserSource.next(null); //=>removing the user info to it's initial value(null).
}
}
