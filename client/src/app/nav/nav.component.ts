import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
   model: any = {};
   loggedIn = false;
   constructor(private accountService:AccountService){}
   login(){
    //=>return type'i Observable old. subscribe olduk;
      this.accountService.login(this.model).subscribe({
         next: response => {
             console.log(response);
             this.loggedIn = true;
         },
         error: error => console.log(error)
      });
   }
   logout(){
      this.loggedIn = false;
   }
}
