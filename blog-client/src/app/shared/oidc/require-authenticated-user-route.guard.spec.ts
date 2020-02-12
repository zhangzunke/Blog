import { TestBed, async, inject } from '@angular/core/testing';

import { RequireAuthenticatedUserRouteGuard } from './require-authenticated-user-route.guard';

describe('RequireAuthenticatedUserRouteGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RequireAuthenticatedUserRouteGuard]
    });
  });

  it('should ...', inject([RequireAuthenticatedUserRouteGuard], (guard: RequireAuthenticatedUserRouteGuard) => {
    expect(guard).toBeTruthy();
  }));
});
