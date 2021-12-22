import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { subscribeOn } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  RegisterMode = false;
  
  constructor() { }

  ngOnInit(): void {
  }
  RegisterToggle()
 {
  this.RegisterMode = !this.RegisterMode;
 }

 CancelRegisterMode(event: boolean)
 {
  this.RegisterMode = event;
 }
}
