import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HomeComponent } from './components/home/home.component';
import { RoomListComponent } from './components/room-list/room-list.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthInterceptor } from 'src/utils/auth-interceptor';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule } from '@angular/material/dialog';
import { RoomDetailsComponent } from './components/room-details/room-details.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ImageViewerComponent } from './components/image-viewer/image-viewer.component';
import { FooterComponent } from './components/footer/footer.component';
import { CopyrightComponent } from './components/copyright/copyright.component';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { ErrorComponent } from './components/error/error.component';
import { ReservationForRoomComponent } from './components/reservation-for-room/reservation-for-room.component';
import { BlogComponent } from './components/blog/blog.component';
import { CreatePostComponent } from './components/create-post/create-post.component';
import { CreateEventComponent } from './components/create-event/create-event.component';
import { EventListComponent } from './components/event-list/event-list.component';
import { PersonalComponent } from './components/personal/personal.component';
import { ReservationListComponent } from './components/reservation-list/reservation-list.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { UpdateEventComponent } from './components/update-event/update-event.component';
import { UpdateUserComponent } from './components/update-user/update-user.component';
import { AdminComponent } from './components/admin/admin.component';
import { RoomManagerComponent } from './components/room-manager/room-manager.component';
import { CreateRoomComponent } from './components/create-room/create-room.component';
import { UpdateImageComponent } from './components/update-image/update-image.component';
import { DeleteImageComponent } from './components/delete-image/delete-image.component';
import { MatSelectModule } from '@angular/material/select';
import { SplitPipe } from '../utils/split.pipe';
import { ReservationForUserComponent } from './components/reservation-for-user/reservation-for-user.component';
import { OperatorComponent } from './components/operator/operator.component';
import { UserManagerComponent } from './components/user-manager/user-manager.component';
import { UpdateUserAsAdmin } from './components/update-user-as-admin/update-user-as-admin.component';
import { PostManagerComponent } from './components/post-manager/post-manager.component';
import { EquipmentManagerComponent } from './components/equipment-manager/equipment-manager.component';
import { CreateEquipmentComponent } from './components/create-equipment/create-equipment.component';
import { DeleteEquipmentComponent } from './components/delete-equipment/delete-equipment.component';
import { AddEquipmentToRoomComponent } from './components/add-equipment-to-room/add-equipment-to-room.component';
import { RemoveEquipmentFromRoomComponent } from './components/remove-equipment-from-room/remove-equipment-from-room.component';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { PatientComponent } from './components/patient/patient.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    RoomListComponent,
    RegistrationComponent,
    RoomListComponent,
    LoginComponent,
    RoomDetailsComponent,
    ImageViewerComponent,
    FooterComponent,
    CopyrightComponent,
    ErrorComponent,
    ReservationForRoomComponent,
    BlogComponent,
    PersonalComponent,
    ReservationListComponent,
    CreatePostComponent,
    CreateEventComponent,
    EventListComponent,
    ConfirmationComponent,
    UpdateEventComponent,
    UpdateUserComponent,
    AdminComponent,
    RoomManagerComponent,
    CreateRoomComponent,
    UpdateImageComponent,
    DeleteImageComponent,
    SplitPipe,
    ReservationForUserComponent,
    OperatorComponent,
    PostManagerComponent,
    EquipmentManagerComponent,
    CreateEquipmentComponent,
    DeleteEquipmentComponent,
    AddEquipmentToRoomComponent,
    RemoveEquipmentFromRoomComponent,
    UserManagerComponent,
    UpdateUserAsAdmin,
    EmailConfirmationComponent,
    PatientComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    BrowserAnimationsModule,
    MatSelectModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi:true},
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false}},
    {provide: LocationStrategy, useClass: HashLocationStrategy}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
