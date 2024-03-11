import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { CommonModule } from '@angular/common';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, CommonModule, NgbDropdownModule, RouterModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
   model: any = {};
   constructor(public accountService:AccountService, private router:Router, private toaster: ToastrService){}

   ngOnInit():void{
   }

   login(){
    //=>return type'i Observable old. subscribe olduk;
      this.accountService.login(this.model).subscribe({
         next: _ => this.router.navigateByUrl('/members'), //=>koddan yonlendirme saglamak icin.
         //error kismi tanimlanmadi cunku interceptorda(error.interceptor.ts) handle ediliyor.
      });
   }
   logout(){
      this.accountService.logout();
      this.router.navigateByUrl('/'); //=>HomePage(home.component)'e yonlendirir.
   }
}
