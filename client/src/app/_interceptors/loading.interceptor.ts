import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BusyService } from '../_services/busy.service';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  var busyService = inject(BusyService);
  busyService.busy(); //=>its gonna increment the busy request count.

  return next(req).pipe(
    delay(1000), //to apply fake delay to show loading indicators by considering the request time for live app.
    finalize(() => {
      busyService.idle(); //to turn off the loading spinner once the request has been completed.
    })
  );
};
