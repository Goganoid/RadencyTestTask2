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
  
  public setEmitId(id: number) {
    this.id = id;
  }
  public resetEditId() {
    this.id = undefined;
  }
  ngOnInit(): void {
    this.updateLists();
  }

  updateLists(): void{
    console.log("Updating the books list");
    this.books$ = this.bookService.getBooks();
    this.recommendedBooks$ = this.bookService.getRecommendedBooks();
  }
}
