import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { divColor } from 'src/app/divColor';
import { ImageProcessService } from 'src/app/services/image-process.service';
@Component({
  selector: 'app-choose-image',
  templateUrl: './choose-image.component.html',
  styleUrls: ['./choose-image.component.css']
})
export class ChooseImageComponent implements OnInit {
  public fileInput: HTMLInputElement | undefined;
  public picture: string = "";
  public notloading:boolean=true
  constructor(public si: ImageProcessService, public r: Router, public http: HttpClient, public sanitizer: DomSanitizer) { }
  ngOnInit(): void {
  }

  selectedFile: File | undefined;

  imgSrc: string | SafeUrl = '/assets/no-image-icon-6.png'
  onFileSelected(event: any) {
      this.imgSrc = this.sanitizer.bypassSecurityTrustUrl(
      window.URL.createObjectURL(event.target.files[0]))
      console.log(URL.createObjectURL(event.target.files[0]))
      this.selectedFile = <File>event.target.files[0];
      this.si.img=this.imgSrc
      }
      PixelPicture()
      {
        this.notloading=false
        const fd = new FormData();
        fd.append('image', this.selectedFile!, this.selectedFile!.name);
        this.si.getPixelPicture(fd).subscribe(
        (matrix: any[]) => {
          if (matrix != null) {
            this.si.mat = new Array<Array<divColor>>()
            console.log(matrix);
            for (let i = 0; i < matrix.length; i++) {
              var arr = new Array<divColor>();
              for (let j = 0; j < matrix[0].length; j++) {
                arr.push(new divColor(matrix[i][j]))
              }

              this.si.mat.push(arr);
            }
            debugger
            this.notloading=true
            this.r.navigate(['./צביעה לפי מספר'])
          }
          else
          { this.notloading=true
            alert("התמונה שהעלת אינה מכילה עצמים.");
          }
            
        },
        (error: any) => {
          this.notloading=true
         alert(error.message);
        }
      );
    }
 }


