import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogRoutingModule } from './blog-routing.module';
import { MaterialModule } from '../material/material.module';
import { BlogAppComponent } from './blog-app.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { PostService } from './services/post.service';
import { PostListComponent } from './components/post-list/post-list.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthorizationHeaderInterceptor } from '../shared/oidc/authorizationHeaderInterceptor';
import { PostCardComponent } from './components/post-card/post-card.component';
import { WritePostComponent } from './components/write-post/write-post.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditorModule } from '@tinymce/tinymce-angular';
@NgModule({
  declarations: [BlogAppComponent, SidenavComponent, ToolbarComponent, PostListComponent, PostCardComponent, WritePostComponent],
  imports: [
    CommonModule,
    BlogRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    EditorModule,
  ],
  providers: [
    PostService
  ]
})
export class BlogModule { }
