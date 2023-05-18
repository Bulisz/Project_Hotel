import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PostDetailsModel } from 'src/app/models/post-details-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { CreateBlogComponentComponent } from '../create-blog-component/create-blog-component.component';

@Component({
  selector: 'app-blog-component',
  templateUrl: './blog-component.component.html',
  styleUrls: ['./blog-component.component.css']
})
export class BlogComponentComponent implements OnInit{

  currentUser: any;
  posts!: Array<PostDetailsModel>;

  constructor(private as: AccountService, private rs: ReservationService, private dialog: MatDialog,){
  }

  ngOnInit(): void {
    this.as.user.subscribe({
      next: (res) => this.currentUser=res
    })

    this.getAllPosts();
  }

  async getAllPosts(){
    await this.rs.getAllPosts()
      .then((res) => this.posts = res)
      .catch((err) => console.log(err))
  }

  addNewPost(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'}
    };

    let dialog = this.dialog.open(CreateBlogComponentComponent,dialogBoxSettings);

    dialog.afterClosed().subscribe({
      next: () => this.getAllPosts()
    })
  }
}
