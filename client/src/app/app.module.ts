import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component'; // komponenta za navigaciju ng g c --skip-tests
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // za ukljucivanje forme
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component'; // sa angular boostrapa za dropdown meni
import { SharedModule } from './_modules/shared.module';
import { MemberCardComponent } from './member/member-card/member-card.component';
import { JtwInterceptor } from './_interceptors/jtw.interceptor';
import { MemberEditComponent } from './member/member-edit/member-edit.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { PhotoEditorComponent } from './member/photo-editor/photo-editor.component';
import { InputTextComponent } from './_forms/input-text/input-text.component';
import { DateInputComponent } from './_forms/date-input/date-input.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    ListsComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    InputTextComponent,
    DateInputComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    NgxSpinnerModule,
    
  ],
  providers: [
    {provide :HTTP_INTERCEPTORS, useClass: JtwInterceptor, multi: true},
    {provide :HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}

    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
