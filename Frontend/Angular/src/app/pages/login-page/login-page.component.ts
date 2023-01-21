import { States } from 'src/app/constants';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { IUser } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { AuthStateService } from 'src/app/services/authstate.service';
import { ErrorService } from 'src/app/services/error.service';
import { SuccessService } from 'src/app/services/success.service';
import { LoadingService } from 'src/app/services/loading.service';
import { NGXLogger } from 'ngx-logger';


@Component({
    selector: 'bo-login-page',
    templateUrl: './login-page.component.html',
    styleUrls: ['./login-page.component.less']
})
export class LoginPageComponent {
    private unsubscribe$ = new Subject();
    public user: IUser;
    public isLoggedIn: boolean = false;
    public States = States;

    constructor(
        private authService: AuthService,
        private errorService: ErrorService,
        private router: Router,
        private authStateService: AuthStateService,
        private successService: SuccessService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) {
        this.user = {} as any;
        this.initialize();
    }

    public ngOnInit() {
        this.authService.isLoggedIn$.pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: isLoggedIn => {
                    this.isLoggedIn = isLoggedIn;
                    this.logger.info('isLoggedIn in login page updated by auth');
                },
                error: error => {
                    this.logger.error(`Error during getting isLoggedIn for login page`, error.message, error);
                }
            });
    }

    public onLoginClick(): void {
        this.loadingService.loadingOn();
        this.errorService.clear();
        this.authService.login(this.user).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: response => {
                    if (response.status === 200) {
                        this.isLoggedIn = true;
                        this.authStateService.setIsLoggedIn(this.isLoggedIn);
                        this.successService.handle("Logged in sucessfully", 2000);
                        this.logger.info('Logged in sucessfully');
                        this.authService.getUserType().pipe(
                            takeUntil(this.unsubscribe$)
                        )
                            .subscribe(response => {
                                this.logger.info('Got user type sucessfully', response);
                            });
                    } else {
                        this.logger.error(`Did not get user type: `, response.status, response.body, response);
                    }
                    this.loadingService.loadingOff();
                },
                error: error => {
                    this.logger.error(`Did not get user type: ${error.message}`, error);
                    this.loadingService.loadingOff();
                }
            });
    }

    public onSignUpClick(): void {
        this.logger.info(`Navigating to sign Up page`);
        this.router.navigate([States.signUp]);
    }

    public ngOnDestroy() {
        this.unsubscribe$.next(null);
        this.unsubscribe$.complete();
    }

    private initialize(): void {
        this.logger.info(`Initilizing the user with empty string`);
        this.user = {
            login: "",
            password: ""
        };
    }
}
