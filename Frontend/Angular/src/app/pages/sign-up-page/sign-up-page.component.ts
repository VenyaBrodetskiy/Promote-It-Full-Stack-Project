import { SuccessService } from './../../services/success.service';
import { UserService } from './../../services/user.service';
import { UserType } from './../../enums';
import { IBusinessOwner, ISocialActivist, INonProfitOrg } from './../../models/user';
import { IUser } from 'src/app/models/user';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { States } from 'src/app/constants';
import { LoadingService } from 'src/app/services/loading.service';
import { NGXLogger } from 'ngx-logger';
import { Subject, takeUntil } from 'rxjs';
import { ErrorService } from 'src/app/services/error.service';

@Component({
    selector: 'bo-sign-up-page',
    templateUrl: './sign-up-page.component.html',
    styleUrls: ['./sign-up-page.component.less'],
})
export class SignUpPageComponent {
    public user: IUser = {
        login: "",
        password: ""
    };
    public businessOwner: IBusinessOwner = {
        userTypeId: UserType.BusinessOwner,
        twitterHandle: '',
        name: '',
        email: '',
        login: '',
        password: ''
    };
    public socialActivist: ISocialActivist = {
        userTypeId: UserType.SocialActivist,
        twitterHandle: '',
        email: '',
        address: '',
        phoneNumber: '',
        login: '',
        password: ''
    };
    public nonProfitOrg: INonProfitOrg = {
        userTypeId: UserType.NonprofitOrganization,
        name: '',
        email: '',
        website: '',
        login: '',
        password: ''
    };
    public userTypeSelected: UserType;
    public UserType = UserType;
    public userTypes: UserType[] = [
        UserType.BusinessOwner,
        UserType.SocialActivist,
        UserType.NonprofitOrganization
    ];

    public JSON = JSON;
    private unsubscribe$ = new Subject();

    constructor(
        private userService: UserService,
        private router: Router,
        private errorService: ErrorService,
        private successService: SuccessService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) {
    }

    public onSignUpBusinessOwner() {
        this.loadingService.loadingOn();
        this.errorService.clear();
        this.businessOwner.login = this.user.login;
        this.businessOwner.password = this.user.password;
        this.userService.addBusinessOwner(this.businessOwner).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: (response) => {
                    if (response.status === 200) {
                        this.logger.info(`Added user: ${response.body}`);
                        this.successService.handle("Signed up sucessfully. Please Log in", 3000);
                        this.router.navigate([States.login]);
                    } else {
                        this.logger.error(`Did not add business owner: ${response.status}, ${response.body}`, response);
                    }
                    this.loadingService.loadingOff();
                },
                error: (error) => {
                    this.logger.error(`Did not add business owner: ${error.message}`, error);
                    this.loadingService.loadingOff();
                }
            })
    }

    public onSignUpSocialActivist() {
        this.loadingService.loadingOn();
        this.errorService.clear();
        this.socialActivist.login = this.user.login;
        this.socialActivist.password = this.user.password;
        this.userService.addSocialActivist(this.socialActivist).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: (response) => {
                    if (response.status === 200) {
                        this.logger.info(`Added user: ${response.body}`);
                        this.successService.handle("Signed up sucessfully. Please Log in", 3000);
                        this.router.navigate([States.login]);
                    } else {
                        this.logger.error(`Did not add social activist: ${response.status}, ${response.body}`, response);
                    }
                    this.loadingService.loadingOff();
                },
                error: (error) => {
                    this.logger.error(`Did not add social activist: ${error.message}`, error);
                    this.loadingService.loadingOff();
                }
            })
    }

    public onSignUpNonProfit() {
        this.loadingService.loadingOn();
        this.errorService.clear();
        this.nonProfitOrg.login = this.user.login;
        this.nonProfitOrg.password = this.user.password;
        this.userService.addNonProfitUser(this.nonProfitOrg).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: (response) => {
                    if (response.status === 200) {
                        this.logger.info(`Added user: ${response.body}`);
                        this.successService.handle("Signed up sucessfully. Please Log in", 3000);
                        this.router.navigate([States.login]);
                    } else {
                        this.logger.error(`Did not add non profit organization: ${response.status}, ${response.body}`, response);
                    }
                    this.loadingService.loadingOff();
                },
                error: (error) => {
                    this.logger.error(`Did not add non profit organization: ${error.message}`, error);
                    this.loadingService.loadingOff();
                }
            })
    }
}
