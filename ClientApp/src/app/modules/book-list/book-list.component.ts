import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BookListItem } from './../../shared/models/BookListItems';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent {

  @Input() books$: Observable<BookListItem[]> | undefined;
  @Input() recommendedBooks$: Observable<BookListItem[]> | undefined;
}
