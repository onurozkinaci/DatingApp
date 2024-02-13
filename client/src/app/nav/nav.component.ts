import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { CommonModule } from '@angular/common';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, CommonModule, NgbDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
   model: any = {};
   //loggedIn = false;
   //currentUser$:Observable<User|null> = of(null);

   constructor(public accountService:AccountService){}
   ngOnInit():void{
      //this.getCurrentUser();
      //this.currentUser$ = this.accountService.currentUser$;
   }

   //=>Current user info will be assigned to the observable in this class directly in the constructor instead of below method;
   //**=>To check if the user currently logged in or not (by checking the persistent data on Observable that we created);
   /*getCurrentUser(){
      this.accountService.currentUser$.subscribe({
         next: user => this.loggedIn = !!user, //!! ile user'i booleana donusturur(degeri varsa true, null ise false doner)
         error: error => console.log(error)
      })
   }*/
   
   login(){
    //=>return type'i Observable old. subscribe olduk;
      this.accountService.login(this.model).subscribe({
         next: response => {
             console.log(response);
             //this.loggedIn = true;
         },
         error: error => console.log(error)
      });
   }
   logout(){
      this.accountService.logout();
      //this.loggedIn = false;
   }
}
