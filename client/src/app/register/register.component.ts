import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { TextInputComponent } from "../_forms/text-input/text-input.component";

@Component({
    selector: 'app-register',
    standalone: true,
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    imports: [FormsModule, ReactiveFormsModule, CommonModule, TextInputComponent]
})
export class RegisterComponent implements OnInit{
  //@Input() usersFromHomeComponent:any;
  @Output() cancelRegister = new EventEmitter(); //=>to emit something from child to parent.
   constructor(private accountService:AccountService, private toaster: ToastrService, 
      private fb: FormBuilder){
   }

   model:any = {}
   registerForm: FormGroup = new FormGroup({});

   ngOnInit(): void {
      this.initializeForm();
   }

   initializeForm(){
    this.registerForm = this.fb.group({
      gender: ['male'], //initial value is assigned to validate this radio button
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
       next:() => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
   }

   matchValues(matchTo:string):ValidatorFn{
       return(control:AbstractControl) => {
          return control.value === control.parent?.get(matchTo)?.value? null : {notMatching:true}
       }
   }
   

   register(){
    console.log(this.registerForm?.value);
     //=>the register operation will be triggered via our service;
    //  this.accountService.register(this.model).subscribe({
    //      next:() => {
    //         this.cancel(); //=>to close the register form after the operation is completed.
    //      },
    //      error:error => {
    //       this.toaster.error(error.error); //=>the error message will be given with toaster. 
    //       console.log(error);
    //      }
    //  });
   }
   cancel(){
    this.cancelRegister.emit(false); //=>we want to turn off the registermode in home.component(parent component)
   }
}
