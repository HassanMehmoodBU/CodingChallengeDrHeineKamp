import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public homeText: string;

  constructor(private router: Router) { }

  ngOnInit(): void {
    var token = localStorage.getItem("token");

    if (!token) {
      this.router.navigate(["authentication/login"]);
    }
    this.homeText = "Welcome to Dr. Heinkamp Coding Challenge : File Server App"
  }

}
