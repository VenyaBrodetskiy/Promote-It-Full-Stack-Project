import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { States } from '../constants';
import { UserType } from '../enums';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(private router: Router) { }

    public canActivate(): boolean {
        if (localStorage.getItem('token')) {
            let userTypeId = localStorage.getItem('userTypeId');
            switch (userTypeId) {
                case String(UserType.BusinessOwner):
                    this.router.navigate([`/${States.campaigns}`]);
                    break;
                case String(UserType.SocialActivist):
                    this.router.navigate([`/${States.balance}`]);
                    break;
                case String(UserType.NonprofitOrganization):
                    this.router.navigate([`/${States.npCampaigns}`]);
                    break;
                case String(UserType.ProlobbyOwner):
                    this.router.navigate([`/${States.login}`]);
                    break;
                default:
                    this.router.navigate([`/${States.login}`]);
                    break;
            }
            return true;
        }
        this.router.navigate([`/${States.login}`]);
        return false;
    }

}
