import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-about',
  imports: [RouterOutlet, RouterModule, FormsModule, HttpClientModule, CommonModule],
  templateUrl: './about.html',
  styleUrl: './about.css'
})
export class About {
constructor(public router: Router, public http: HttpClient){
  
}
isLogged() {
    if (sessionStorage.getItem("token") != null) {
      return true;
    }
    return false
  }

  getUsername() {
    return sessionStorage.getItem("username");
  }

  async logout() {
    sessionStorage.clear();
    this.router.navigate(["/login"]);
  }
}
