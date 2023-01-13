import { Component } from '@angular/core';
import { IUser } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';


@Component({
  selector: 'bo-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent {
  public user: IUser;

  constructor(
    private authService: AuthService
  ) {
    this.user = {} as any;
    this.initialize();
  }

  public onLoginClick(): void {
      this.authService.login(this.user)
      .subscribe(() => console.log("Logged in"));
  }

  public onClearClick(): void {

  }

  private initialize(): void {
    this.user = {
      username: "",
      password: ""
    };
  }

}
