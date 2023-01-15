import { Component } from '@angular/core';
import { UserType } from './enums';
import { AuthGuard } from './services/auth.guard';
import { AuthService } from './services/auth.service';
import { AuthStateService } from './services/authstate.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.less']
})
export class AppComponent {
    public isLoggedIn = false;
    public title = 'business-owner';
    public userTypeId: string;
    public UserType = UserType;
    public BusinessOwner: string = String(UserType.BusinessOwner);
    public SocialActivist: string = String(UserType.SocialActivist);
    public NonprofitOrganization: string = String(UserType.NonprofitOrganization);
    public ProlobbyOwner: string = String(UserType.ProlobbyOwner);
    public System: string = String(UserType.System);

    constructor(private authGuard: AuthGuard,
        public authService: AuthService
    ) {
        this.isLoggedIn = this.authGuard.canActivate();
        this.authService.isLoggedIn$.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
        this.authService.userTypeId$.subscribe(userTypeId => this.userTypeId = userTypeId);
    }

    ngOnInit() {
        this.authService.isLoggedIn$.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
        this.authService.userTypeId$.subscribe(userTypeId => this.userTypeId = userTypeId);
    }

}
