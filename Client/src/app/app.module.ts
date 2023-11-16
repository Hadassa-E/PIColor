import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PaintPixlesComponent } from './components/paint-pixels/paint-pixles.component';
import { ChooseImageComponent } from './components/choose-image/choose-image.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MainTemplateComponent } from './components/main-template/main-template.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    AppComponent,
    PaintPixlesComponent,
    ChooseImageComponent,
    MainTemplateComponent,
    HomePageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
