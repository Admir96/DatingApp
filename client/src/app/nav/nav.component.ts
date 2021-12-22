import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import {User} from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any = {};  // two way binding, u html nav forma prima podatke i stornira u model

  constructor(public accountServices: AccountService, private router: Router, private toastr:ToastrService) { }

  ngOnInit(): void {
  }
login()
  {
    this.accountServices.login(this.model).subscribe(
      response => {
     this.router.navigateByUrl('/members'); // da se pri login otvori members page
      console.log(response);

    },

    error =>{   
    console.log(error); 
    this.toastr.error(error.error);
   
   })
  }
  logout()
  {
   this.router.navigateByUrl('/');// da se pri login otvori home page
    this.accountServices.logout();
  }

}
