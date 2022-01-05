import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
@Output() CancelRegister = new EventEmitter(); // output iz child u perent sa var koja je evenemiter iz biblioteke @angular
registerForm: FormGroup;
maxDate:Date;
validationErrors: string[] = [];


  constructor(private accountService:AccountService,  private toastr:ToastrService, 
    private fb: FormBuilder, private router:Router) { }

  ngOnInit(): void {
   this.initializeForm();
   this.maxDate = new Date();
   this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm()
  {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      gender: ['male'],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required,
        Validators.minLength(4),
         Validators.maxLength(8)]],
      confirmPassword: ['',[Validators.required, this.matchValue('password')]]
    })
  }

matchValue(matchTo: string): ValidatorFn { //napravio validacuju i gore pozvao
  return (control: AbstractControl) => {
    return control?.value === control?.parent?.controls[matchTo].value 
    ? null : {isMatching: true}
  }
}

 register()
 {
  this.accountService.Register(this.registerForm.value).subscribe(Response =>{ 
    this.router.navigateByUrl('/members');
  },
  error =>{
  this.validationErrors = error;
  });
 }

 cancel()
 {
  this.CancelRegister.emit(false); // ovim emitujemo output
 }

}
