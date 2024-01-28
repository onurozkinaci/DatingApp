import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Dating app'; //title: string => automatically assigns the type.
  users: any; 

  //=>dependency injection in Angular;
  constructor(private http:HttpClient){}

  ngOnInit(): void {
    //=>url is the endpoint which is defined on the API project(BE side);
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed.')
    }); //=>subscribe to observe an Observable which is the return type of 
    //data from the HttpRequest.
  }

}

