import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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

  @Output() idEmitter: EventEmitter<number> = new EventEmitter<number>;
  @Input() book: BookListItem | undefined;

  constructor(public dialog: MatDialog, public imageSanitizer: ImageSanitizerService) { }

  public setEditId(id: number) {
    this.idEmitter.emit(id);
  }

  public openDialog(): void {
    this.dialog.open(ViewBookComponent, {
      data: { bookId: this.book?.id } as ModalData,
    });
  }
}
