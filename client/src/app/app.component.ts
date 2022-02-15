import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/User';
import { AccountService } from './_services/account.service';
import { PressenceService } from './_services/pressence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingApp';
  user:any;

  constructor(private accountService : AccountService,
     private presenceService: PressenceService)
   {

   }

  ngOnInit(){

    this.SetCurrentUser();
  }
  SetCurrentUser()
  {
    const user: User = JSON.parse(localStorage.getItem('user'));

    if(user){
     this.accountService.SetCurrentUser(user);
     this.presenceService.createHubConnection(user);
    }
  }
}
