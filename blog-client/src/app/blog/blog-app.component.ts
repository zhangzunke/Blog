import { Component, OnInit } from '@angular/core';
import { MatIconRegistry } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-blog-app',
  template: `
    <app-sidenav></app-sidenav>
  `,
  styles: []
})
export class BlogAppComponent implements OnInit {

  constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
    iconRegistry.addSvgIcon(
      'more_vert',
      sanitizer.bypassSecurityTrustResourceUrl('assets/more_vert-24px.svg'));
    iconRegistry.addSvgIcon(
      'menu',
      sanitizer.bypassSecurityTrustResourceUrl('assets/menu-24px.svg'));
    iconRegistry.addSvgIcon(
      'baseline-add',
      sanitizer.bypassSecurityTrustResourceUrl('assets/baseline-add-24px.svg'));
   }

  ngOnInit() {
  }

}
