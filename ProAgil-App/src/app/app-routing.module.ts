import { EventoEditComponent } from './eventos/eventoEdit/eventoEdit.component';
import { DashbordComponent } from './dashbord/dashbord.component';
import { ContatosComponent } from './contatos/contatos.component';
import { PalestrantesComponent } from './palestrantes/palestrantes.component';
import { EventosComponent } from './eventos/eventos.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthGuard } from './auth/auth.guard';


const routes: Routes = [
  { path: 'user', component: UserComponent, 
  children: [
    { path: 'login', component: LoginComponent },
    { path: 'registration', component: RegistrationComponent },
  ] },

  { path: 'eventos', component: EventosComponent , canActivate: [AuthGuard]},
  { path: 'eventos/:id/edit', component: EventoEditComponent , canActivate: [AuthGuard]},
  { path: 'palestrantes', component: PalestrantesComponent , canActivate: [AuthGuard]},
  { path: 'contatos', component: ContatosComponent, canActivate: [AuthGuard] },
  { path: 'dashboard', component: DashbordComponent , canActivate: [AuthGuard]},
  { path: '',  redirectTo : 'dashboard', pathMatch: 'full' },
  { path: '**',  redirectTo : 'dashboard', pathMatch: 'full' },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
