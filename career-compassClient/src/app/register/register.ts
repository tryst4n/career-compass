import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { lastValueFrom } from 'rxjs';

const domain = "http://localhost:5063/";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterOutlet, RouterModule, CommonModule, FormsModule, HttpClientModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  registerUsername: string = "";
  registerEmail: string = "";
  registerPassword: string = "";
  registerPasswordConfirm: string = "";

  constructor(public http: HttpClient, public router: Router) { }

  async register(): Promise<void> {
    try {
      let registerDTO = {
        username: this.registerUsername,
        email: this.registerEmail,
        password: this.registerPassword,
        passwordConfirm: this.registerPasswordConfirm
      };
      let x = await lastValueFrom(this.http.post<any>(domain + "api/Users/Register", registerDTO));
      console.log(x);

      let loginDTO = {
        username: this.registerUsername,
        password: this.registerPassword
      };

      let y = await lastValueFrom(this.http.post<any>(domain + "api/Users/Login", loginDTO));
      console.log(y);
      sessionStorage.setItem("token", y.token);
      sessionStorage.setItem("username", y.username);

      this.router.navigate(['/home']).then(() => {
        window.location.reload();
      });
    } catch (error) {
      console.error("Registration failed", error);
      return;
    }
  }
}
