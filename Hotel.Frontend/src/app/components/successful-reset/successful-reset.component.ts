import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-successful-reset',
  templateUrl: './successful-reset.component.html',
  styleUrls: ['./successful-reset.component.css']
})
export class SuccessfulResetComponent implements OnInit {
  
  constructor(private router: Router){}
  
  ngOnInit(): void {
    this.navigateToHome();
  }

  navigateToHome(){
    setTimeout(() => this.router.navigate(['']), 3000)
  }


}
