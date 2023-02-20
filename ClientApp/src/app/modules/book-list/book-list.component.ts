import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BookListItem } from './../../shared/models/BookListItems';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent {

  @Output() idEmitter: EventEmitter<number> = new EventEmitter<number>;
  @Input() books$: Observable<BookListItem[]> | undefined;
  @Input() recommendedBooks$: Observable<BookListItem[]> | undefined;

  public setEditId(id: number) {
    this.idEmitter.emit(id);
  }
}
