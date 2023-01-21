import { Component } from '@angular/core';
import { NGXLogger } from 'ngx-logger';
import { Subject, takeUntil } from 'rxjs';
import { UserType } from './enums';
import { AuthService } from './services/auth.service';
import { ErrorService } from './services/error.service';
import { LoadingService } from './services/loading.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.less']
})
export class AppComponent {

    private unsubscribe$ = new Subject();
    public isLoggedIn = false;
    public title = 'promote-it-project';
    public userTypeId: string;
    public UserType = UserType;
    public BusinessOwner: string = String(UserType.BusinessOwner);
    public SocialActivist: string = String(UserType.SocialActivist);
    public NonprofitOrganization: string = String(UserType.NonprofitOrganization);
    public ProlobbyOwner: string = String(UserType.ProlobbyOwner);
    public System: string = String(UserType.System);

    constructor(
        public authService: AuthService,
        private errorService: ErrorService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) { }

    ngOnInit() {
        this.isLoggedIn = (localStorage.getItem('isLoggedIn') === 'true');
        this.authService.isLoggedIn$.pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: isLoggedIn => {
                    this.isLoggedIn = isLoggedIn;
                    this.logger.info('isLoggedIn in app component updated by auth');
                },
                error: error => {
                    this.logger.error(`Error during getting isLoggedIn for app`, error.message, error);
                }
            });
        this.authService.userTypeId$.pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: userTypeId => {
                    this.userTypeId = userTypeId;
                    this.logger.info('userTypeId in app component updated by auth');
                },
                error: error => {
                    this.logger.error(`Error during getting userTypeId for app`, error.message, error);
                }
            });
        // this.authService.isLoggedIn$.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
        // this.authService.userTypeId$.subscribe(userTypeId => this.userTypeId = userTypeId);
        window.addEventListener("beforeunload", (e) => this.authService.logout());
    }

}
