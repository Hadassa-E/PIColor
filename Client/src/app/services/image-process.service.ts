import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { divColor } from '../divColor';
import { SafeUrl } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class ImageProcessService {
// public img:string="../assets/FTM-0843.jpg";
public img:string|SafeUrl='';
url:string='https://localhost:7280/api/Picture/'

constructor(public http:HttpClient) { }
public mat: Array<Array<divColor>> = new Array<Array<divColor>>();
//serviceקריאות שרת ב
getPixelPicture(image: any): Observable<any> {
  return this.http.post<string>(this.url + 'GetPixelPicture', image)
}
}
