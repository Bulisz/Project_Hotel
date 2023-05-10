import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { RoomListComponent } from './components/room-list/room-list.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { RoomDetailsComponent } from './components/room-details/room-details.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'room-list', component: RoomListComponent},
  { path: 'room-details/:id', component: RoomDetailsComponent},
  { path: 'registration', component: RegistrationComponent},
  { path: 'login', component: LoginComponent},
  { path: '**', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
