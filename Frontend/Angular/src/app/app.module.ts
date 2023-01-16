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
import { LoginComponent } from './components/login/login.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { BusinessOwnerGuard } from './guards/businessowner.guard';
import { DonateToCampaignPageComponent } from './pages/donate-to-campaign-page/donate-to-campaign-page.component';
import { AuthGuard } from './guards/auth.guard';
import { MenuNpComponent } from './components/menu-np/menu-np.component';
import { CreateCampaignPageComponent } from './pages/create-campaign-page/create-campaign-page.component';
import { NonprofitOrganizationGuard } from './guards/nonprofitorganization.guard';
import { CampaignNpPageComponent } from './pages/campaign-np-page/campaign-np-page.component';


const routes: Routes = [
    { path: '', component: AppComponent, canActivate: [AuthGuard] },
    { path: States.login, component: LoginComponent },

    { path: States.campaigns, component: CampaignPageComponent, canActivate: [BusinessOwnerGuard] },
    { path: States.donateNewProduct, component: CreateProductPageComponent, canActivate: [BusinessOwnerGuard] },
    { path: States.donateToCampaign, component: DonateToCampaignPageComponent, canActivate: [BusinessOwnerGuard] },
    { path: States.orders, component: OrderPageComponent, canActivate: [BusinessOwnerGuard] },

    { path: States.createCampaign, component: CreateCampaignPageComponent, canActivate: [NonprofitOrganizationGuard] },
    { path: States.npCampaigns, component: CampaignNpPageComponent, canActivate: [NonprofitOrganizationGuard] },


    { path: "**", component: LoginComponent }

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
        LoginComponent,
        DonateToCampaignPageComponent,
        MenuNpComponent,
        CreateCampaignPageComponent,
        CampaignNpPageComponent
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
