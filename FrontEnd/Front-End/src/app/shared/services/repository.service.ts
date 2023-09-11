import { Company } from './../../_interfaces/company.model';
import { Document } from './../../_interfaces/document.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { EnvironmentUrlService } from './environment-url.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getData = (route: string) => {
    return this.http.get<Company[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public getUploadedFiles = (route: string) => {
    return this.http.get<Document[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  getAllFiles(headers: HttpHeaders): Observable<any> {
    return this.http.get(`${this.envUrl.urlAddress}/api/documents/allfiles`, { headers });
  }

  downloadFile(headers: HttpHeaders, fileGuid: string): Observable<HttpResponse<Blob>> {
    return this.http.get<Blob>(`${this.envUrl.urlAddress}/api/documents/download/${fileGuid}`, {
      observe: 'response',
      responseType: 'blob' as 'json',
      headers: headers
    });
  }

  downloadMultipleFiles(headers: HttpHeaders, fileGuids: string[]): Observable<any> {
    return this.http.post<Blob>(`${this.envUrl.urlAddress}/api/documents/download/multiple`, fileGuids, {
      responseType: 'blob' as 'json',
      headers: headers
    });
  }

  generateShareAbleLink(headers: HttpHeaders, documentId: string, days: number, hours: number): Observable<any> {
    return this.http.post<Document>(`${this.envUrl.urlAddress}/api/documents/makelink`, { documentId, days, hours }, {
      headers: headers
    });
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}