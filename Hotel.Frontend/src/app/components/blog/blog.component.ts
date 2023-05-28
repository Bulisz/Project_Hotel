import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PostDetailsModel } from 'src/app/models/post-details-model';
import { AccountService } from 'src/app/services/account.service';
import { CreatePostComponent } from '../create-post/create-post.component';
import { UserModel } from 'src/app/models/user-model';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent implements OnInit{

  currentUser: UserModel | null = null;
  posts!: Array<PostDetailsModel>;

  constructor(
    private as: AccountService,
    private postService: PostService,
    private dialog: MatDialog){
  }

  async ngOnInit() {

    this.as.user.subscribe({
      next: res => this.currentUser = res
    })

    await this.getAllPosts();
  }

  async getAllPosts(){
    await this.postService.getConfirmedPosts()
      .then((res) => this.posts = res)
      .catch((err) => console.log(err))
  }

  addNewPost(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'},
      data: { userName: this.currentUser?.userName, role: this.currentUser?.role }
    };
    let dialog = this.dialog.open(CreatePostComponent,dialogBoxSettings);

    dialog.afterClosed().subscribe({
      next: () => this.getAllPosts()
    })
  }
}
