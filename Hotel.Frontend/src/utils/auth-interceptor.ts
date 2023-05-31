import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { NavigationExtras, Router } from "@angular/router";
import { Observable, map, tap } from "rxjs";
import { LoadingDialogComponent } from "src/app/components/loading-dialog/loading-dialog.component";

@Injectable({
  providedIn: 'root'
})

export class AuthInterceptor implements HttpInterceptor {

  constructor(private dialog: MatDialog, private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let dialogBoxSettings = {
      disableClose: true,
      hasBackdrop: true,
    };
    let dialogref = this.dialog.open(LoadingDialogComponent, dialogBoxSettings)

    if (localStorage.getItem('accessToken')) {
      const newRequest = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${localStorage.getItem('accessToken')}`)
      })
      return next.handle(newRequest).pipe(tap(async (event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          dialogref.close()
        }
      },
        (err: any) => {
          if(err.status !== 400) {
            const navigationExtras: NavigationExtras = {
              state: {message: err.error, status: err.status,  statusText: err.statusText},
            };
            this.router.navigate(['error'], navigationExtras)
          }
          dialogref.close()
        }));
    }

    return next.handle(req).pipe(tap(async (event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        dialogref.close()
      }
    },
      (err: any) => {
        if(err.status !== 400) {
          const navigationExtras: NavigationExtras = {
            state: {message: err.error, status: err.status, statusText: err.statusText},
          };
          this.router.navigate(['error'], navigationExtras)
        }
        dialogref.close()
      }));
  }
}
