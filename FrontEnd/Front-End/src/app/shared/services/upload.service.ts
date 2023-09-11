import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'; 
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class UploadService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public uploadFile = (route: string, formData : FormData) => {
    return this.http.post (this.createCompleteRoute(route, this.envUrl.urlAddress), formData , {reportProgress :true});
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}