import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';

export const routes: Routes = [
    {path:'', component:HomeComponent}, //=>bos route icin HomeComponent yuklenecek.
    {path:'',  //*=>"dummy route";
     runGuardsAndResolvers:'always',
     canActivate:[authGuard],
     children:[
        {path:'members', component:MemberListComponent},
        {path:'members/:username', component:MemberDetailComponent}, //=>:username is a route param as 'members/christina'
        {path:'member/edit',component:MemberEditComponent, canDeactivate:[preventUnsavedChangesGuard]},
        {path:'lists', component:ListsComponent},
        {path:'messages', component:MessagesComponent},
        {path:'messages', component:MessagesComponent},
     ]
    },
    {path:'errors', component:TestErrorComponent},
    {path:'not-found', component:NotFoundComponent},
    {path:'server-error', component:ServerErrorComponent},
    //*=>'/nonsense(herhangi bir deger)' icin bu "wildcard" tanimi ile invalid route icin de NotFoundComponent'e yonlenecek;
    {path:'**', component:NotFoundComponent, pathMatch:'full'}
];
