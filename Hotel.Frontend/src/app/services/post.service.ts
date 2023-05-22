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

  async getAllPosts(): Promise<Array<PostDetailsModel>> {
    return await firstValueFrom(this.http.get<Array<PostDetailsModel>>(`${environment.apiUrl}/${this.BASE_URL}/getAllPosts`))
  }
}
