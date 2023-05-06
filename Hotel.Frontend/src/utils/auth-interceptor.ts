import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class AuthInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    if(localStorage.getItem('accessToken')){
      const newRequest = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${localStorage.getItem('accessToken')}`)
      })
      return next.handle(newRequest);
    }
    return next.handle(req)
  }
}
