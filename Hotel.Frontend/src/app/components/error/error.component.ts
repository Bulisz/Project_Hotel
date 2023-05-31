import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit{

  errorMessage = '';
  errorStatus = 0;
  errorStatusText = '';

  ngOnInit(): void {
    this.errorMessage = history.state['message'];
    this.errorStatus = history.state['status'];
    this.errorStatusText = history.state['statusText'];
  }

}
