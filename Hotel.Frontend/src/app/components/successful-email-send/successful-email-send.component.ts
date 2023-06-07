import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-successful-email-send',
  templateUrl: './successful-email-send.component.html',
  styleUrls: ['./successful-email-send.component.css']
})
export class SuccessfulEmailSendComponent implements OnInit {
  
  constructor(private router: Router){}

  ngOnInit(): void {
    this.navigateToHome();
  }

  navigateToHome(){
    setTimeout(() => this.router.navigate(['']), 7000)
  }


}
