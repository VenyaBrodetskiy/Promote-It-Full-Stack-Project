import { SuccessService } from './../../services/success.service';
import { UserService } from './../../services/user.service';
import { UserType } from './../../enums';
import { IBusinessOwner, ISocialActivist, INonProfitOrg } from './../../models/user';
import { IUser } from 'src/app/models/user';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { States } from 'src/app/constants';

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

    constructor(
        private userService: UserService,
        private router: Router,
        private successService: SuccessService
    ) {
    }

    public onSignUpBusinessOwner() {
        this.businessOwner.login = this.user.login;
        this.businessOwner.password = this.user.password;

        this.userService.addBusinessOwner(this.businessOwner)
            .subscribe({
                next: (response) => {
                    if (response.status === 200) {
                        console.log(response);
                        console.log("Added user: ", response.body);
                        this.successService.handle("Signed up sucessfully. Please Log in");
                        this.router.navigate([States.login]);
                    } else {
                        console.log("Error: ", response.status, response.body);
                    }
                },
                error: (error) => {
                    console.log("Error: ", error);
                }
            })
    }

    public onSignUpSocialActivist() {
        this.socialActivist.login = this.user.login;
        this.socialActivist.password = this.user.password;

        this.userService.addSocialActivist(this.socialActivist)
            .subscribe({
                next: (response) => {
                    if (response.status === 200) {
                        console.log(response);
                        console.log("Added user: ", response.body);
                        this.successService.handle("Signed up sucessfully. Please Log in");
                        this.router.navigate([States.login]);
                    } else {
                        console.log("Error: ", response.status, response.body);
                    }
                },
                error: (error) => {
                    console.log("Error: ", error);
                }
            })
    }

    public onSignUpNonProfit() {
        this.nonProfitOrg.login = this.user.login;
        this.nonProfitOrg.password = this.user.password;

        this.userService.addNonProfitUser(this.nonProfitOrg)
            .subscribe({
                next: (response) => {
                    if (response.status === 200) {
                        console.log(response);
                        console.log("Added user: ", response.body);
                        this.successService.handle("Signed up sucessfully. Please Log in");
                        this.router.navigate([States.login]);
                    } else {
                        console.log("Error: ", response.status, response.body);
                    }
                },
                error: (error) => {
                    console.log("Error: ", error);
                }
            })
    }
}
