import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Observable, map } from "rxjs";
import { LoadingDialogComponent } from "src/app/components/loading-dialog/loading-dialog.component";

@Injectable({
  providedIn: 'root'
})

export class AuthInterceptor implements HttpInterceptor {

  constructor(private dialog: MatDialog){}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let dialogBoxSettings = {
      disableClose: true,
      hasBackdrop: true,
    };
    let dialogref = this.dialog.open(LoadingDialogComponent,dialogBoxSettings)

    if(localStorage.getItem('accessToken')){
      const newRequest = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${localStorage.getItem('accessToken')}`)
      })
      return next.handle(newRequest)
      .pipe(map<HttpEvent<any>, any>((evt: HttpEvent<any>) => {
        if (evt instanceof HttpResponse) {
          dialogref.close()
       }
        return evt;
      }));
    }
    return next.handle(req)
      .pipe(map<HttpEvent<any>, any>((evt: HttpEvent<any>) => {
        if (evt instanceof HttpResponse) {
          dialogref.close()
       }
        return evt;
      }));
  }
}
