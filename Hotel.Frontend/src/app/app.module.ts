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
    ConfirmationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    BrowserAnimationsModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi:true},
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false}},
    {provide: LocationStrategy, useClass: HashLocationStrategy}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
