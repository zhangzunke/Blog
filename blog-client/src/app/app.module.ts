import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatCheckboxModule } from '@angular/material';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SigninOidcComponent } from './shared/oidc/signin-oidc/signin-oidc.component';
import { RedirectSilentRenewComponent } from './shared/oidc/redirect-silent-renew/redirect-silent-renew.component';
import { RequireAuthenticatedUserRouteGuard } from './shared/oidc/require-authenticated-user-route.guard';
import { AuthorizationHeaderInterceptor } from './shared/oidc/authorizationHeaderInterceptor';

@NgModule({
  declarations: [
    AppComponent,
    SigninOidcComponent,
    RedirectSilentRenewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatCheckboxModule,
    HttpClientModule
  ],
  providers: [ RequireAuthenticatedUserRouteGuard,
    {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthorizationHeaderInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
