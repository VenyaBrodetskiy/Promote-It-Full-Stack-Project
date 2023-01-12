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
import { DonateProductComponent } from './components/donate-product/donate-product.component';
import { SelectComponent } from './components/select/select.component';
import { SelectProductComponent } from './components/select-product/select-product.component';
import { OrderComponent } from './components/order/order.component';
import { OrderPageComponent } from './pages/order-page/order-page.component';

const routes: Routes = [
  { path: States.campaigns, component: CampaignPageComponent },
  { path: States.donateNewProduct, component: CreateProductPageComponent },
  { path: States.donateToCampaign, component: DonateProductComponent },
  { path: States.orders, component: OrderPageComponent },

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
    DonateProductComponent,
    SelectComponent,
    SelectProductComponent,
    OrderComponent,
    OrderPageComponent
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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
