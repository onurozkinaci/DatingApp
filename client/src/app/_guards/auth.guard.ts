import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService); //=>class olmadigi icin ctor yok ama Angular ile gelen inject kullanilabilir!
  const toastr = inject(ToastrService);
  return accountService.currentUser$.pipe(
    map(user => {
      if(user) return true;
      else{ //the user not logged in yet
         toastr.error('You shall not pass!');
         return false;
      }
    })
  )
};
