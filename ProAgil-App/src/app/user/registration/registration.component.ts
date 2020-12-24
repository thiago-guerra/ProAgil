import { Router } from '@angular/router';
import { AuthService } from './../../_services/auth.service';
import { User } from './../../_models/User';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registerForm: FormGroup;
  user: User;

  constructor(public fb: FormBuilder,
              private authService: AuthService,
              private router: Router,
              ) { }

  ngOnInit() {
    this.validation();
  }

  validation() {
    this.registerForm = this.fb.group({
       fullName: ['', Validators.required],
       email: ['', [Validators.required]],
       userName: ['', Validators.required],
       passwords: this.fb.group({
         password: ['', [Validators.required, Validators.minLength(4)]],
         confirmPassword: ['', Validators.required]
       }, {validator: this.compararSenhas})
    });
  }

  compararSenhas(fb: FormGroup){
    const confirmSenhaCtrl = fb.get('confirmPassword');
    if(confirmSenhaCtrl.errors == null || 'missmatch' in confirmSenhaCtrl.errors){
      if (fb.get('password').value !== confirmSenhaCtrl.value) {
        confirmSenhaCtrl.setErrors({mismatch:  true});
      } else {
        confirmSenhaCtrl.setErrors(null);
      }
    }

  }

  cadastrarUsuario() {
    if (this.registerForm.valid) {
      this.user = Object.assign({password: this.registerForm.get('passwords.password').value}, this.registerForm.value);
      this.authService.register(this.user).subscribe(
        () => {
            this.router.navigate(['/user/login']);
            console.log('Cadastro realizado');
        }, error => {
           const erro = error.error;
          erro.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                console.log("Cadastro Duplicado");
                break;
            
              default:
                console.log(`Erro no Cadastro! CODE: ${element.code}`);
                break;
            }
          });
        }

       )
    }
  }


}
