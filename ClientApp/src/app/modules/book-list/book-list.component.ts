import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BookService } from 'src/app/shared/services/book.service';
import { BookListItem } from './../../shared/models/BookListItems';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {

  @Output() idEmitter: EventEmitter<number> = new EventEmitter<number>;
  @Input() books$: Observable<BookListItem[]> | undefined;
  @Input() recommendedBooks$: Observable<BookListItem[]> | undefined;

  constructor(private bookService: BookService) { }

  public setEditId(id: number) {
    this.idEmitter.emit(id);
  }

  ngOnInit(): void {
    this.books$ = this.bookService.getBooks();
    this.recommendedBooks$ = this.bookService.getRecommendedBooks();
  }
}
