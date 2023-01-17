import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { States } from 'src/app/constants';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'bo-menu-sa',
  templateUrl: './menu-sa.component.html',
  styleUrls: ['./menu-sa.component.less']
})
export class MenuSaComponent {
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
