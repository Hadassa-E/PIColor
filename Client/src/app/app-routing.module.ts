import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaintPixlesComponent } from './components/paint-pixels/paint-pixles.component';
import { ChooseImageComponent } from './components/choose-image/choose-image.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { MainTemplateComponent } from './components/main-template/main-template.component';

const routes: Routes = [
  {path: "דף הבית",component:HomePageComponent},
  {path: "צביעה לפי מספר",component:PaintPixlesComponent},
  {path: "העלאת תמונה",component:ChooseImageComponent},
  {path: "main",component:MainTemplateComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
 }
