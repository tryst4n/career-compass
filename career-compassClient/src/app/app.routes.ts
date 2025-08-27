import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Register } from './register/register';

export const routes: Routes = [
    { path: "", redirectTo: "/home", pathMatch: "full" },
    { path: "home", component: Home },
    { path: "register", component: Register },
];
