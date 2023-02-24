import { setId } from './../../store/new-book-id.actions';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { ImageSanitizerService } from 'src/app/shared/services/image-sanitizer.service';
import { ViewBookComponent } from '../view-book/view-book.component';
import { BookListItem } from './../../shared/models/BookListItems';

// type of data passed to a modal window
export interface ModalData {
  bookId: number
}

@Component({
  selector: 'app-book-list-item',
  templateUrl: './book-list-item.component.html',
  styleUrls: ['./book-list-item.component.css']
})
export class BookListItemComponent {

  @Input() book: BookListItem | undefined;

  constructor(public dialog: MatDialog,
    public imageSanitizer: ImageSanitizerService,
    private store:Store<{id:number}>) { }

  public setEditId(id: number) {
    this.store.dispatch(setId({id}))
  }

  public openDialog(): void {
    this.dialog.open(ViewBookComponent, {
      data: { bookId: this.book?.bookId } as ModalData,
    });
  }
}
