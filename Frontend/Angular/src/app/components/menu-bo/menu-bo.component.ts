import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { States } from "../../constants";

@Component({
    selector: 'bo-menu-bo',
    templateUrl: './menu-bo.component.html',
    styleUrls: ['./menu-bo.component.less']
})
export class MenuBoComponent {
    public States = States;

    constructor(
        private authService: AuthService,
        private router: Router
    ) { }

    public onLogoutClick() {
        this.authService.logout();
        this.router.navigate([`/${States.login}`]);
    }
}
