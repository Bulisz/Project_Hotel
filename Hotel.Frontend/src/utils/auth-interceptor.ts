import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { NavigationExtras, Router } from "@angular/router";
import { Observable, tap } from "rxjs";
import { LoadingDialogComponent } from "src/app/components/loading-dialog/loading-dialog.component";
import { AccountService } from "src/app/services/account.service";

@Injectable({
  providedIn: 'root'
})

export class AuthInterceptor implements HttpInterceptor {

  constructor(private dialog: MatDialog, private router: Router, private as: AccountService) { }

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
      return next.handle(newRequest).pipe(
        tap(event => {
          if (event instanceof HttpResponse) {
            dialogref.close()
          }
        },
        async err => {
          if (err.status === 401) {
            dialogref.close()
            return await this.handleRefresh(newRequest, next)
          } else {
            dialogref.close()
            this.navigateToErrorPage(err)
          }
        })
        );
    }

    return next.handle(req).pipe(
      tap(event => {
        if (event instanceof HttpResponse) {
          dialogref.close()
        }
      },
        err => {
          dialogref.close()
          this.navigateToErrorPage(err)
        }
      )
    );
  }

  async handleRefresh(req: HttpRequest<any>, next: HttpHandler): Promise<any> {
    return await this.as.refresh()
      .then(td => {
        const newRequest = req.clone({
          headers: req.headers.set('Authorization', `Bearer ${td.accessToken.value}`)
        })
        return next.handle(newRequest)
      })
  }

  navigateToErrorPage(err: any) {
    if (err.status !== 400) {
      const navigationExtras: NavigationExtras = {
        state: { message: err.error, status: err.status, statusText: err.statusText },
      };
      this.router.navigate(['error'], navigationExtras)
    }
  }
}
