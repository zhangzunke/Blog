import { Component, OnInit } from '@angular/core';
import { PostService } from '../../services/post.service';
import { PostParameters } from '../../models/post-parameters';
import { PageMeta } from 'src/app/shared/page-meta';
import { Post } from '../../models/post';
import { OpenIdConnectService } from 'src/app/shared/oidc/open-id-connect.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {
  postParameter = new PostParameters({ orderBy: 'id desc', pageSize: 10, pageIndex: 0 });
  pageMeta: PageMeta;
  posts: Post[];
  constructor(private postService: PostService, private openIdConnectService: OpenIdConnectService) { }

  ngOnInit() {
    this.getPosts();
  }

  getPosts() {
    this.postService.getPagedPosts(this.postParameter).subscribe(resp => {
      this.pageMeta = JSON.parse(resp.headers.get('X-Pagination')) as PageMeta;
      this.posts = resp.body as Post[];
      console.log(this.posts);
    });
  }

}
