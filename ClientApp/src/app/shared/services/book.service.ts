import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, Observable, throwError } from 'rxjs';
import { SnackBarError } from '../config/snackbar.config';
import { BookDetails } from '../models/BookDetails';
import { IdResponse } from '../models/IdResponse';
import { ValidationError } from '../models/ValidationError';
import { BookListItem } from './../models/BookListItems';
import { SaveBook } from './../models/SaveBook';
@Injectable({
  providedIn: 'root'
})
export class BookService {

  private baseApiUri = 'http://localhost:5000/api/';
  constructor(private httpClient: HttpClient, private snackBar: MatSnackBar) { }
  getBooks(): Observable<BookListItem[]> {
    return this.httpClient
      .get<BookListItem[]>(this.baseApiUri + 'books?order=title')
      .pipe(catchError((err, _) => {
        return this.handleError(err, this.defaultErrorMessage(err.status));
      }));
  }
  getRecommendedBooks(): Observable<BookListItem[]> {
    return this.httpClient
      .get<BookListItem[]>(this.baseApiUri + 'books/recommended')
      .pipe(catchError((err, _) => {
        return this.handleError(err,this.defaultErrorMessage(err.status));
      }));
  }
  getBook(id: number): Observable<BookDetails> {
    return this.httpClient
      .get<BookDetails>(this.baseApiUri + `books/${id}`)
      .pipe(catchError((err, _) => {
        return this.handleError(err, this.defaultErrorMessage(err.status));
      }));
  }
  saveBook(bookModel: SaveBook): Observable<IdResponse> {
    console.log(JSON.stringify(bookModel));
    return this.httpClient
      .post<IdResponse>(this.baseApiUri + "books/save", JSON.stringify(bookModel), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })})
      .pipe(catchError((err, _) => {
        const errors = err?.error as ValidationError[];
        const errorMessage = (errors != null && errors.length > 0)
          ? errors[0].errorMessage
          : this.defaultErrorMessage(err.status);
        return this.handleError(err, errorMessage);
      }));
  }
  private handleError(err: any, message: string): Observable<never> {
    console.error(err);
    this.snackBar.open(message, 'Close', SnackBarError);
    return throwError(() => new Error(message))
  }
  private defaultErrorMessage = (status: any) => `There was an error getting data. Status code:${status}`;
}
