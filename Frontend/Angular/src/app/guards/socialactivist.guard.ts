import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { States } from '../constants';
import { UserType } from '../enums';

@Injectable({
    providedIn: 'root'
})
export class SocialActivistGuard implements CanActivate {
    constructor(private router: Router) { }

    public canActivate(): boolean {
        if ((localStorage.getItem('token')) && (localStorage.getItem('userTypeId') == String(UserType.SocialActivist))) {
            return true;
        }
        this.router.navigate([`/${States.login}`]);
        return false;
    }
}
