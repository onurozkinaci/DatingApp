import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatePickerComponent } from "../_forms/date-picker/date-picker.component";
import { Router } from '@angular/router';

@Component({
    selector: 'app-register',
    standalone: true,
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    imports: [FormsModule, ReactiveFormsModule, CommonModule, TextInputComponent, DatePickerComponent]
})
export class RegisterComponent implements OnInit{
  //@Input() usersFromHomeComponent:any;
  @Output() cancelRegister = new EventEmitter(); //=>to emit something from child to parent.
   constructor(private accountService:AccountService, private toaster: ToastrService, 
      private fb: FormBuilder, private router: Router){}

   registerForm: FormGroup = new FormGroup({});
   maxDate : Date = new Date();
   validationErrors: string[] | undefined;

   ngOnInit(): void {
      this.initializeForm();
      //=>to prevent the registarion of users under 18 years old.
      this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
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
      const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
      const values = {...this.registerForm.value, dateOfBirth:dob};

     //=>the register operation will be triggered via our service;
     this.accountService.register(values).subscribe({
         next:() => {
            //this.cancel(); //=>to close the register form after the operation is completed.
            this.router.navigateByUrl('/members');
         },
         error:error => {
            this.validationErrors = error;
         }
     });
   }
   cancel(){
    this.cancelRegister.emit(false); //=>we want to turn off the registermode in home.component(parent component)
   }

   private getDateOnly(dob: string | undefined){
      if(!dob) return;
      let theDob = new Date(dob);
      //=>slice(0,10) to get the date portion of ISOString;
      return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())).toISOString().slice(0,10);
   }
}
