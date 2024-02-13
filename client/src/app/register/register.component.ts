import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  @Input() usersFromHomeComponent:any;
  @Output() cancelRegister = new EventEmitter(); //=>to emit something from child to parent.

   model:any = {}
   register(){
     console.log(this.model);
   }
   cancel(){
    this.cancelRegister.emit(false); //=>we want to turn off the registermode in home.component(parent component)
   }
}
