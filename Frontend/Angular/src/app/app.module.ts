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
import { MenuComponent } from './components/menu/menu.component';
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
import { AuthGuard } from './services/auth.guard';
import { DonateToCampaignPageComponent } from './pages/donate-to-campaign-page/donate-to-campaign-page.component';


const routes: Routes = [
    { path: '', redirectTo: States.login, pathMatch: 'full' },
    { path: States.login, component: LoginComponent },
    { path: States.campaigns, component: CampaignPageComponent, canActivate: [AuthGuard] },
    { path: States.donateNewProduct, component: CreateProductPageComponent, canActivate: [AuthGuard] },
    { path: States.donateToCampaign, component: DonateToCampaignPageComponent, canActivate: [AuthGuard] },
    { path: States.orders, component: OrderPageComponent, canActivate: [AuthGuard] },
    { path: "**", component: LoginComponent }

]

@NgModule({
    declarations: [
        AppComponent,
        CampaignComponent,
        ErrorComponent,
        CreateProductPageComponent,
        MenuComponent,
        CampaignPageComponent,
        CreateProductComponent,
        CreateCampaignComponent,
        OrderComponent,
        OrderPageComponent,
        LoginComponent,
        DonateToCampaignPageComponent
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
