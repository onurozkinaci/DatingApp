import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  //@Input() usersFromHomeComponent:any;
  @Output() cancelRegister = new EventEmitter(); //=>to emit something from child to parent.
   constructor(private accountService:AccountService, private toaster: ToastrService){
   }
   model:any = {}
   register(){
     //=>the register operation will be triggered via our service;
     this.accountService.register(this.model).subscribe({
         next:() => {
            this.cancel(); //=>to close the register form after the operation is completed.
         },
         error:error => {
          this.toaster.error(error.error); //=>the error message will be given with toaster. 
          console.log(error);
         }
     });
   }
   cancel(){
    this.cancelRegister.emit(false); //=>we want to turn off the registermode in home.component(parent component)
   }
}
