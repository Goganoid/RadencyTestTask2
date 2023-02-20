import { SaveBook } from './../models/SaveBook';
import { BookListItem } from './../models/BookListItems';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookDetails } from '../models/BookDetails';
import { IdResponse } from '../models/IdResponse';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  private baseApiUri = 'http://localhost:5000/api/';
  constructor(private httpClient: HttpClient) { }
  getBooks(): Observable<BookListItem[]> {
    return this.httpClient.get<BookListItem[]>(this.baseApiUri + 'books');
  }
  getRecommendedBooks(): Observable<BookListItem[]> {
    return this.httpClient.get<BookListItem[]>(this.baseApiUri + 'books/recommended');
  }
  getBook(id:number): Observable<BookDetails>{
    return this.httpClient.get<BookDetails>(this.baseApiUri + `books/${id}`);
  }
  saveBook(bookModel: SaveBook): Observable<IdResponse> {
    console.log(JSON.stringify(bookModel));
    return this.httpClient.post<IdResponse>(this.baseApiUri + "books/save", JSON.stringify(bookModel), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    });
  }
}
