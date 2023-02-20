import { BookListItem } from './../../shared/models/BookListItems';
import { Component, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { BookService } from 'src/app/shared/services/book.service';
import { stringCompare } from './helpers/sortAlphabetically';

@Component({
  selector: 'app-books-page',
  templateUrl: './books-page.component.html',
  styleUrls: ['./books-page.component.css']
})
export class BooksPageComponent implements OnInit {
  id: number | undefined;

  books$: Observable<BookListItem[]> | undefined;
  recommendedBooks$: Observable<BookListItem[]> | undefined;

  constructor(private bookService: BookService) { }
  
  setEmitId(id: number) {
    console.log(`Received id ${id}`)
    this.id = id;
  }
  ngOnInit(): void {
    this.books$ = this.bookService.getBooks();
    this.recommendedBooks$ = this.bookService.getRecommendedBooks();
  }

  updateList(): void{
    console.log("received update request");
    this.books$ = this.bookService.getBooks();
  }
}
