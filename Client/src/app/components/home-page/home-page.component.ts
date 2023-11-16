import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  public d: number = 0
  public mat: Array<Array<string>> = new Array<Array<string>>(19)
  public x: boolean = true
  public colors: Array<string> = ['#ff0000', '#ff7300', '#fffb00', '#48ff00', '#00ffd5', '#002bff', '#7a00ff', '#ff00c8', '#ff0000']
  constructor(public r: Router) { }
  ngOnInit(): void {
    for (let index = 0; index < this.mat.length; index++)
      this.mat[index] = new Array<string>(38)
    for (let index = 0; index < 99999; index++) {
      let rnd = Math.floor(Math.random() * 10);
      let i = Math.floor(Math.random() * 19);
      let j = Math.floor(Math.random() * 38);
      setTimeout(() => {
        this.mat[i][j] = this.colors[rnd]
      }, this.d + 100);
      this.d += 100
      if (i == 99998)
        i = 0;
    }
  }

  f() {
    this.x = false
    this.r.navigate(['./העלאת תמונה']);
  }
}
