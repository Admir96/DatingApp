import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
@Output() CancelRegister = new EventEmitter(); // output iz child u perent sa var koja je evenemiter iz biblioteke @angular
model:any = {};
registerForm: FormGroup;


  constructor(private accountService:AccountService,  private toastr:ToastrService, 
    private fb: FormBuilder) { }

  ngOnInit(): void {
this.initializeForm();
  }

  initializeForm()
  {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
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
   console.log(this.registerForm.value);
  /*this.accountService.Register(this.model).subscribe(Response =>{
    console.log(Response);
    this.cancel();
  },
  error =>{
  console.error(error);
  this.toastr.error(error.error);
  });*/
 }

 cancel()
 {
  this.CancelRegister.emit(false); // ovim emitujemo output
 }

}
