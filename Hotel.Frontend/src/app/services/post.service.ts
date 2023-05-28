import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PostDetailsModel } from '../models/post-details-model';
import { PostModel } from '../models/post-model';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  BASE_URL = 'posts'

  constructor(private http: HttpClient) { }

  async createPost(post: PostModel): Promise<PostModel> {
    return await firstValueFrom(this.http.post<PostModel>(`${environment.apiUrl}/${this.BASE_URL}/createPost`,post))
  }

  async getConfirmedPosts(): Promise<Array<PostDetailsModel>> {
    return await firstValueFrom(this.http.get<Array<PostDetailsModel>>(`${environment.apiUrl}/${this.BASE_URL}/getConfirmedPosts`))
  }

  async getNonConfirmedPosts(): Promise<Array<PostDetailsModel>> {
    return await firstValueFrom(this.http.get<Array<PostDetailsModel>>(`${environment.apiUrl}/${this.BASE_URL}/getNonConfirmedPosts`))
  }

  async confirmPost(postId: number): Promise<any> {
    let body = {id: postId}
    return await firstValueFrom(this.http.put(`${environment.apiUrl}/${this.BASE_URL}/confirmPost`, body))
  }

  async deletePost(id: number): Promise<any> {
    return await firstValueFrom(this.http.delete(`${environment.apiUrl}/${this.BASE_URL}/deletePost/${id}`))
  }
}
