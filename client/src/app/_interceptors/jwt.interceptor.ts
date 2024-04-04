import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  var accountService = inject(AccountService);
  //=>`pipe(take(1))` ile subcsribe olunan currentUser$ obersvable'indan donus aldiktan sonra islemi tamamla demis oluyorsun ve app'teki kaynaklar daha fazla tuketilmemis oluyor.Boylece unsubscribe olmak gerekmiyor;
  accountService.currentUser$.pipe(take(1)).subscribe({
    next: user => {
      if(user){
        req = req.clone({
          setHeaders:{
             Authorization: `Bearer ${user.token}`
          }
        })
      }
    }
  })
  return next(req);
};
