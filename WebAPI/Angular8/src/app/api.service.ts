import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Supplier } from './supplier.model';
import { catchError, tap, map } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({'Content-type': 'application/json'})
};
const apiUrl = 'http://localhost:5000/api/supplier';

@Injectable({
  providedIn: 'root'
})

export class ApiService {

  constructor(private http: HttpClient) { }
  
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead
  
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  getSuppliers(): Observable<Supplier[]> {
    return this.http.get<Supplier[]>(apiUrl)
    .pipe(
      tap(heroes => console.log('fetched suppliers')),
      catchError(this.handleError('getSuppliers', []))
    );
  }

  getSupplier(id: number): Observable<Supplier> {
    const url = `${apiUrl}/${id}`;
    return this.http.get<Supplier>(url).pipe(
      tap(_ => console.log(`fetched supplier id=${id}`)),
      catchError(this.handleError<Supplier>(`getSupplier id=${id}`))
      );
  }

  addSupplier(supplier: any): Observable<Supplier> {
    return this.http.post(apiUrl, supplier, httpOptions).pipe(
      tap((supplierRes: Supplier) => console.log(`added Supplier w/ id=${supplierRes.supplierId}`)),
      catchError(this.handleError<Supplier>('addSupplier'))
    );
  }

  updateSupplier(id: number, supplier: any): Observable<any> {
    const url = `${apiUrl}/${id}`;

    return this.http.put(url, supplier, httpOptions).pipe(
      tap(_ => console.log(`updated supplier id=${id}`)),
      catchError(this.handleError<any>('updateSupplier'))
    );
  }

  deleteSupplier(id: number): Observable<Supplier> {
    const url = `${apiUrl}/${id}`;

    return this.http.delete<Supplier>(url,httpOptions).pipe(
      tap(_ => console.log(`deleted Supplier id=${id}`)),
      catchError(this.handleError<Supplier>('deleteSupplier'))
    );
  }

}
