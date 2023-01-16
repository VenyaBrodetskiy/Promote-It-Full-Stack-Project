import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { States } from 'src/app/constants';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'bo-menu-np',
  templateUrl: './menu-np.component.html',
  styleUrls: ['./menu-np.component.less']
})
export class MenuNpComponent {
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
