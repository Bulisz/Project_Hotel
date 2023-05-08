import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { RoomListComponent } from './components/room-list/room-list.component';
import { RegistrationComponent } from './components/registration/registration.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'room-list', component: RoomListComponent},
  { path: 'registration', component: RegistrationComponent},
  { path: '**', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
