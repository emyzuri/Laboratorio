import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Api {
  private urlApi = 'http://localhost:5243/api/v1/Usuario'
  constructor (private http: HttpClient) { }
  public getData() : Observable<any> {
  return this.http.get<any>(this.urlApi);
  }
}
