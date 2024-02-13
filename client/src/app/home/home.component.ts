import { Component, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
    imports: [RegisterComponent]
})
export class HomeComponent implements OnInit{
  registerMode = false;
  users:any;

  constructor(private http:HttpClient){}

  ngOnInit(): void {
    this.getUsers();
  }
  
  registerToggle(){
    this.registerMode = !this.registerMode;
  }
  getUsers(){
    //=>url is the endpoint which is defined on the API project(BE side);
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed.')
    }); //=>subscribe to observe an Observable which is the return type of 
    //data from the HttpRequest.
  }

  cancelRegisterMode(event:boolean){
    this.registerMode = event;
  }
}
