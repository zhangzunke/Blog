import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PostService } from '../../services/post.service';
import { TinymceService } from '../../services/tinymce.service';
import { MatSnackBar } from '@angular/material';
import { Post } from '../../models/post';

@Component({
  selector: 'app-write-post',
  templateUrl: './write-post.component.html',
  styleUrls: ['./write-post.component.scss']
})
export class WritePostComponent implements OnInit {
  editorSettings;
  postForm: FormGroup;
  constructor(
    private router: Router,
    // private drawrService: DrawerService,
    private postService: PostService,
    private tinymec: TinymceService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.postForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(100)]],
      body: ['', [Validators.required, Validators.minLength(10)]]
    });
    this.editorSettings = this.tinymec.getSettings();
  }

  submit() {
    if (this.postForm.dirty && this.postForm.valid) {
      this.postService.addPost(this.postForm.value).subscribe((post: any) => {
        this.router.navigate(['/blog/posts/'], post.id);
      },
      validationResult => {
        this.snackBar.open('There are validation errors!', 'Close', { duration: 3000 });
      });
    }
  }

}
