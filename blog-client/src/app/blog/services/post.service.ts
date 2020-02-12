import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/shared/base.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PostParameters } from '../models/post-parameters';
import { Post } from '../models/post';
import { PostAdd } from '../models/post-add';

@Injectable({
  providedIn: 'root'
})
export class PostService extends BaseService {
  constructor(private httpClient: HttpClient) {
    super();
   }
   getPagedPosts(postParameters?: any | PostParameters) {
     return this.httpClient.get(`${this.apiUrlBase}/posts`, {
       headers: new HttpHeaders({
         Accept: 'application/json'
       }),
       observe: 'response',
       params: postParameters
     });
   }
   addPost(post: PostAdd) {
     const httpOptions = {
       headers: new HttpHeaders({
        Accept: 'application/json',
        'Content-Type': 'application/json'
       })
     };
     return this.httpClient.post<Post>(`${this.apiUrlBase}/posts`, post, httpOptions);
   }
}
