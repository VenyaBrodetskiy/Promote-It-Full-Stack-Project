import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CampaignComponent } from './components/campaign/campaign.component';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ErrorComponent } from './components/error/error.component';
import { CreateProductPageComponent } from './pages/create-product-page/create-product-page.component';
import { MenuBoComponent } from './components/menu-bo/menu-bo.component';
import { States } from './constants';
import { RouterModule, Routes } from '@angular/router';
import { CampaignPageComponent } from './pages/campaign-page/campaign-page.component';
import { CreateProductComponent } from './components/create-product/create-product.component';
import { CreateCampaignComponent } from './components/create-campaign/create-campaign.component';
import { OrderComponent } from './components/order/order.component';
import { OrderPageComponent } from './pages/order-page/order-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { BusinessOwnerGuard } from './guards/businessowner.guard';
import { DonateToCampaignPageComponent } from './pages/donate-to-campaign-page/donate-to-campaign-page.component';
import { AuthGuard } from './guards/auth.guard';
import { MenuNpComponent } from './components/menu-np/menu-np.component';
import { CreateCampaignPageComponent } from './pages/create-campaign-page/create-campaign-page.component';
import { NonprofitOrganizationGuard } from './guards/nonprofitorganization.guard';
import { CampaignNpPageComponent } from './pages/campaign-np-page/campaign-np-page.component';
import { MenuSaComponent } from './components/menu-sa/menu-sa.component';
import { TableLineComponent } from './components/table-line/table-line.component';
import { ProductsOfCampaignComponent } from './components/products-of-campaign/products-of-campaign.component';
import { SocialActivistGuard } from './guards/socialactivist.guard';
import { BalancePageComponent } from './pages/balance-page/balance-page.component';
import { SignUpPageComponent } from './pages/sign-up-page/sign-up-page.component';
import { SuccessComponent } from './components/success/success.component';


const routes: Routes = [
    { path: '', component: AppComponent, canActivate: [AuthGuard] },
    { path: States.login, component: LoginPageComponent },
    { path: States.signUp, component: SignUpPageComponent },
    { path: States.campaigns, component: CampaignPageComponent, canActivate: [BusinessOwnerGuard] },
    { path: States.donateNewProduct, component: CreateProductPageComponent, canActivate: [BusinessOwnerGuard] },
    { path: States.donateToCampaign, component: DonateToCampaignPageComponent, canActivate: [BusinessOwnerGuard] },
    { path: States.orders, component: OrderPageComponent, canActivate: [BusinessOwnerGuard] },

    { path: States.createCampaign, component: CreateCampaignPageComponent, canActivate: [NonprofitOrganizationGuard] },
    { path: States.npCampaigns, component: CampaignNpPageComponent, canActivate: [NonprofitOrganizationGuard] },

    { path: States.balance, component: BalancePageComponent, canActivate: [SocialActivistGuard] },

    { path: "**", component: LoginPageComponent }

]

@NgModule({
    declarations: [
        AppComponent,
        CampaignComponent,
        ErrorComponent,
        CreateProductPageComponent,
        MenuBoComponent,
        CampaignPageComponent,
        CreateProductComponent,
        CreateCampaignComponent,
        OrderComponent,
        OrderPageComponent,
        LoginPageComponent,
        DonateToCampaignPageComponent,
        SignUpPageComponent,
        SuccessComponent,
        MenuNpComponent,
        CreateCampaignPageComponent,
        CampaignNpPageComponent,
        MenuSaComponent,
        TableLineComponent,
        ProductsOfCampaignComponent,
        BalancePageComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        HttpClientModule,
        NgbModule,
        RouterModule.forChild(routes),
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true,
        },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
