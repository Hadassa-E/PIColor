import { Component, OnInit } from '@angular/core';
import { divColor } from 'src/app/divColor';
import { ImageProcessService } from 'src/app/services/image-process.service';
import { NgZone } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-paint-pixels',
  templateUrl: './paint-pixels.component.html',
  styleUrls: ['./paint-pixels.component.css']
})

export class PaintPixlesComponent implements OnInit {
  constructor(public si: ImageProcessService,private cdr: ChangeDetectorRef) { }
  public inPaint:boolean=false//עבור צפייה בתמונה צבועה
  public inPrint:boolean=false//עבור ההדפסה
  public shapes: boolean = false//עיגולים או ריבועים
  public num: number = 1;
  public colorForPaint: string = ""
  public thecolor: string = "black"
  public thePointer = 'url(../../../assets/pointers_of_color/black.png),auto'
  public mat: Array<Array<divColor>> = new Array<Array<divColor>>();
  public colorsArray: Array<string> = new Array<string>("black", "white", "gray", "red", "#ff98b9", "#6c4e02", "orange", "yellow", "#74d050", "darkgreen", "#80beff", "#151a79", "#66067e");
  ngOnInit(): void {
  }
  f(numColor: number) {
    return this.colorsArray[numColor];
  }

  PaintPic() {
    this.inPaint=true
    for (let i = 0; i < this.si.mat.length; i++) {
      for (let j = 0; j < this.si.mat[0].length; j++) {
        this.si.mat[i][j].tempColor = this.si.mat[i][j].color
        this.si.mat[i][j].color = this.colorsArray[this.si.mat[i][j].num]
      }
    }

    setTimeout(() => {
      for (let i = 0; i < this.si.mat.length; i++)
        for (let j = 0; j < this.si.mat[0].length; j++)
          this.si.mat[i][j].color = this.si.mat[i][j].tempColor

      this.inPaint=false
    }, 1000);
  }

  PrintPic() {
    this.inPrint=true
    for (let i = 0; i < this.si.mat.length; i++) {
      for (let j = 0; j < this.si.mat[0].length; j++) {
        this.si.mat[i][j].tempColor = this.si.mat[i][j].color
        this.si.mat[i][j].color = ''
      }
    }
    debugger
    this.colorForPaint=this.thecolor
    this.thecolor = ''
    this.cdr.detectChanges();//שמירת שינויים בעיצוב המסך
      window.print()
      for (let i = 0; i < this.si.mat.length; i++)
        for (let j = 0; j < this.si.mat[0].length; j++)
          this.si.mat[i][j].color = this.si.mat[i][j].tempColor
      this.thecolor=this.colorForPaint
      this.colorForPaint=''
      this.inPrint=false
  }
  over(i: number, j: number) {
    if (this.colorForPaint != '')
      this.si.mat[i][j].color = this.colorForPaint
  }

  down(i: number, j: number) {
    this.colorForPaint = this.thecolor
    this.over(i, j)
  }
  chooseColor(x: string)//מצביע בלוח הצבעים
  {
    this.thecolor = x
    switch (x) {
      case 'black':
        this.thePointer = 'url(../../../assets/pointers_of_color/black.png),auto'
        break;
      case 'white':
        this.thePointer = "url(../../../assets/pointers_of_color/white.png),auto"
        break;
      case '#6c4e02':
        this.thePointer = "url(../../../assets/pointers_of_color/brown.png),auto"
        break;
      case '#151a79':
        this.thePointer = "url(../../../assets/pointers_of_color/darkblue.png),auto"
        break;
      case 'darkgreen':
        this.thePointer = "url(../../../assets/pointers_of_color/darkgreen.png),auto"
        break;
      case 'gray':
        this.thePointer = "url(../../../assets/pointers_of_color/gray.png),auto"
        break;
      case '#80beff':
        this.thePointer = "url(../../../assets/pointers_of_color/lightblue.png),auto"
        break;
      case '#74d050':
        this.thePointer = "url(../../../assets/pointers_of_color/lightgreen.png),auto"
        break;
      case 'orange':
        this.thePointer = "url(../../../assets/pointers_of_color/orange.png),auto"
        break;
      case '#ff98b9':
        this.thePointer = "url(../../../assets/pointers_of_color/pink.png),auto"
        break;
      case '#66067e':
        this.thePointer = "url(../../../assets/pointers_of_color/purple.png),auto"
        break;
      case 'red':
        this.thePointer = "url(../../../assets/pointers_of_color/red.png),auto"
        break;
      case 'yellow':
        this.thePointer = "url(../../../assets/pointers_of_color/yellow.png),auto"
        break;

    }
  }
  TextColor(i: number, j: number) {
    if (this.si.mat[i][j].color == 'black') {
      if (this.si.mat[i][j].num != 0)
        return 'white'
      else
        return ''
    }
    if (this.si.mat[i][j].color != this.colorsArray[this.si.mat[i][j].num] && this.si.mat[i][j].color != '')
      return 'black'
    else
      return this.si.mat[i][j].color
  }
}
