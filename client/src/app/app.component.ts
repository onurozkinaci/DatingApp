import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { HomeComponent } from "./home/home.component";

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet, NavComponent, HomeComponent]
})
export class AppComponent implements OnInit{
  title = 'Dating app'; //title: string => automatically assigns the type.

  //=>dependency injection in Angular;
  constructor(private accountService:AccountService){}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  //=>Uyg. ayaga kalktiginda localStorage'i kontrol edip bu dogrultuda kalici olarak verileri saklayacak olan
  //ve AccountService'te tanimladigimiz Observable'imiza setleme yapilacak;
  setCurrentUser(){
     const userString = localStorage.getItem('user');
     if(!userString) return; //nullsa asagidaki gibi ekleme yapilacak, localstorage'da user bilgisi varsa ekleme yapilmayacak.
     const user:User = JSON.parse(userString);
     this.accountService.setCurrentUser(user);
  }

}

