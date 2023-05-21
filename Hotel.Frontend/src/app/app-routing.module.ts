import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { RoomListComponent } from './components/room-list/room-list.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { RoomDetailsComponent } from './components/room-details/room-details.component';
import { ErrorComponent } from './components/error/error.component';
import { BlogComponent } from './components/blog/blog.component';
import { EventListComponent } from './components/event-list/event-list.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'room-list', component: RoomListComponent},
  { path: 'room-details/:id', component: RoomDetailsComponent},
  { path: 'blog', component: BlogComponent},
  { path: 'event-list', component: EventListComponent},
  { path: 'registration', component: RegistrationComponent},
  { path: 'login', component: LoginComponent},
  { path: 'error', component: ErrorComponent},
  { path: '**', component: ErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
