import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { IUser } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { AuthStateService } from 'src/app/services/authstate.service';
import { ErrorService } from 'src/app/services/error.service';


@Component({
    selector: 'bo-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.less']
})
export class LoginComponent {
    private unsubscribe$ = new Subject();
    public user: IUser;
    public toggle: boolean = true;
    public isLoggedIn: boolean = false;

    constructor(
        private authService: AuthService,
        private errorService: ErrorService,
        private router: Router,
        private authStateService: AuthStateService
    ) {
        this.user = {} as any;
        this.initialize();
    }

    public onLoginClick(): void {
        this.errorService.clear();
        this.authService.login(this.user).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe(
                response => {
                    if (response.status === 200) {
                        this.toggle = !this.toggle;
                        this.isLoggedIn = true;
                        this.authStateService.setIsLoggedIn(this.isLoggedIn);
                        console.log("Logged in");
                        this.authService.getUserType().pipe(
                            takeUntil(this.unsubscribe$)
                        )
                            .subscribe(response => {
                                console.log(response);
                            });
                    } else {
                        console.log("Error: ", response.status);
                    }
                },
                error => {
                    console.log("Error: ", error);
                }
            );
    }

    onContinue() {
        if (this.isLoggedIn) {
            this.router.navigate(['/campaigns']);
        }
    }

    private initialize(): void {
        this.user = {
            username: "",
            password: ""
        };
    }
}
