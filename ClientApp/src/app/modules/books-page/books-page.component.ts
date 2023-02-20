import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BookService } from 'src/app/shared/services/book.service';
import { BookListItem } from './../../shared/models/BookListItems';

@Component({
  selector: 'app-books-page',
  templateUrl: './books-page.component.html',
  styleUrls: ['./books-page.component.css']
})
export class BooksPageComponent implements OnInit {
  public id: number | undefined;

  public books$: Observable<BookListItem[]> | undefined;
  public recommendedBooks$: Observable<BookListItem[]> | undefined;

  constructor(private bookService: BookService) { }
  
  setEmitId(id: number) {
    this.id = id;
  }
  ngOnInit(): void {
    this.books$ = this.bookService.getBooks();
    this.recommendedBooks$ = this.bookService.getRecommendedBooks();
  }

  updateList(): void{
    console.log("Updating the books list");
    this.books$ = this.bookService.getBooks();
  }
}
