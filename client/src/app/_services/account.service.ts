import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import { ReplaySubject } from 'rxjs';
import {User} from '../_models/User';
import { environment } from 'src/environments/environment';
import { PressenceService } from './pressence.service';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

     baseUrl = environment.baseUrl;
     private currentUserSource = new ReplaySubject<User>(1); 
     currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, 
    private presenceService: PressenceService) { }
  
  Register(model:any)
  {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
     map((user: User) => {
      if(user)
      {
        this.SetCurrentUser(user);
        this.presenceService.createHubConnection(user);
      }
      return user;
    }));
  }
  login(model: any)
  {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((Response: any) =>{

      const user = Response;
        if(user)
        {
         this.SetCurrentUser(user);
         this.presenceService.createHubConnection(user);
        }     
      }));
  }

 SetCurrentUser(user: User)
 {
  user.roles = [];
  const roles = this.getDecodedToken(user.token).role;
  Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
  localStorage.setItem('user', JSON.stringify(user));
  this.currentUserSource.next(user);
 }

logout()
{
  localStorage.removeItem('user');
  this.currentUserSource.next(null);
  this.presenceService.stopHubConnection();
}
getDecodedToken(token)
{
  return JSON.parse(atob(token.split('.')[1]));
}
}

