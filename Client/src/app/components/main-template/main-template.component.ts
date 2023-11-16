import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-template',
  templateUrl: './main-template.component.html',
  styleUrls: ['./main-template.component.css']
})
export class MainTemplateComponent implements OnInit{
  constructor(public r:Router){}
  ngOnInit(): void {
  //  this.r.navigate(['/העלאת תמונה'])
  }

}
