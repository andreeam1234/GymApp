import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { SessionList } from './features/sessions/session-list/session-list';
import { MyBookings } from './features/sessions/my-bookings/my-bookings';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: '/sessions', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'sessions', component: SessionList },
  { path: 'my-bookings', component: MyBookings, canActivate: [authGuard] },
  { path: '**', redirectTo: '/sessions' }
];
