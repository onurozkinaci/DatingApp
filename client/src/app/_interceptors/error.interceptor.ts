import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  var router = inject(Router);
  var toastr = inject(ToastrService);

  return next(req).pipe(
    catchError((error:HttpErrorResponse)=> {
      if(error){
        switch (error.status) {
          case 400:
            if(error.error.errors) //as 400 validation error returns (from the api/register endpoint)
            {
              const modelStateErrors = [];
              for(const key in error.error.errors){
                if(error.error.errors[key]){
                  modelStateErrors.push(error.error.errors[key]);
                } 
              }
              throw modelStateErrors;
            }
            else{ //normal bad request is taken
              toastr.error(error.error,error.status.toString());
            }
            break;
          case 401:
            toastr.error('Unauthorized!',error.status.toString());
            break;
          case 404:
              router.navigateByUrl('/not-found'); //=>404 error alinirsa kullanici farkli bir sayfaya(olusturulduktan sonra not-found component'ine) yonlendirilecek.
              break;
          case 500:
             //=>router is capable of receiving states and here it is the API exception that is returned from the server;
              const navigationExtras:NavigationExtras = {
                 state:{error:error.error}
              }
              router.navigateByUrl('/server-error',navigationExtras);
              break;
          default:
              toastr.error('Something unexpected went wrong!');
              console.log(error);
              break;
        }
      }
      throw error;
    })
  )
};
