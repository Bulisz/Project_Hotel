import { Component, OnInit } from '@angular/core';
import { PostDetailsModel } from 'src/app/models/post-details-model';
import { DialogService } from 'src/app/services/dialog.service';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-post-manager',
  templateUrl: './post-manager.component.html',
  styleUrls: ['./post-manager.component.css']
})
export class PostManagerComponent implements OnInit{

  nonConfirmedPosts: Array<PostDetailsModel> = [];

  constructor(private ps: PostService, private ds: DialogService) {}

  async ngOnInit() {
    await this.refreshPosts();
  }

  async refreshPosts() {
    await this.ps.getNonConfirmedPosts()
      .then(res => this.nonConfirmedPosts = res)
  }

  async confirmPost(id: number) {
    await this.ps.confirmPost(id)
    .then(async () => await this.refreshPosts())
  }

  async deletePost(id: number) {
    let result = await this.ds.confirmationDialog("Biztos törlöd a hozzászólást?")
    if(result === 'agree'){
      await this.ps.deletePost(id)
        .then(async () => await this.refreshPosts())
    }
  }
}
