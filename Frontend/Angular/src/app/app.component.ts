import { Component } from '@angular/core';
import { UserType } from './enums';
import { BusinessOwnerGuard } from './guards/businessowner.guard';
import { AuthService } from './services/auth.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.less']
})
export class AppComponent {
    public isLoggedIn = false;
    public title = 'promote-it-project';
    public userTypeId: string;
    public UserType = UserType;
    public BusinessOwner: string = String(UserType.BusinessOwner);
    public SocialActivist: string = String(UserType.SocialActivist);
    public NonprofitOrganization: string = String(UserType.NonprofitOrganization);
    public ProlobbyOwner: string = String(UserType.ProlobbyOwner);
    public System: string = String(UserType.System);

    constructor(public authService: AuthService
    ) { }

    ngOnInit() {
        this.isLoggedIn = (localStorage.getItem('isLoggedIn') === 'true');
        this.authService.isLoggedIn$.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
        this.authService.userTypeId$.subscribe(userTypeId => this.userTypeId = userTypeId);
        window.addEventListener("beforeunload", (e) => this.authService.logout());
    }

}
