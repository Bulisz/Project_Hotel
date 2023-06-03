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
import { PersonalComponent } from './components/personal/personal.component';
import { AdminComponent } from './components/admin/admin.component';
import { OperatorComponent } from './components/operator/operator.component';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { PatientComponent } from './components/patient/patient.component';
import { AdminGuardService } from './services/admin-guard.service';
import { OperatorGuardService } from './services/operator-guard.service';
import { UserGuardService } from './services/user-guard.service';
import { AboutUsComponent } from './components/about-us/about-us.component';
import { StatisticsComponent } from './components/statistics/statistics.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'room-list', component: RoomListComponent},
  { path: 'room-details/:id', component: RoomDetailsComponent},
  { path: 'event-list', component: EventListComponent},
  { path: 'blog', component: BlogComponent},
  { path: 'admin', component: AdminComponent, canActivate: [AdminGuardService]},
  { path: 'operator', component: OperatorComponent, canActivate: [OperatorGuardService]},
  { path: 'registration', component: RegistrationComponent},
  { path: 'login', component: LoginComponent},
  { path: 'patient', component: PatientComponent},
  { path: 'confirmEmail', component: EmailConfirmationComponent},
  { path: 'personal', component: PersonalComponent, canActivate: [UserGuardService]},
  { path: 'about-us', component: AboutUsComponent},
  { path: 'statistics', component: StatisticsComponent},
  { path: '**', component: ErrorComponent }
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
