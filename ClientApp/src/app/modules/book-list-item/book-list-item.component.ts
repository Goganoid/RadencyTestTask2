import { BookListItem } from './../../shared/models/BookListItems';
import { BookListComponent } from './../book-list/book-list.component';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ViewBookComponent } from '../view-book/view-book.component';
import { ImageSanitizerService } from 'src/app/shared/services/image-sanitizer.service';


export interface ModalData{
  bookId:number
}

@Component({
  selector: 'app-book-list-item',
  templateUrl: './book-list-item.component.html',
  styleUrls: ['./book-list-item.component.css']
})
export class BookListItemComponent {

  @Output() idEmitter: EventEmitter<number> = new EventEmitter<number>;
  @Input() book:BookListItem | undefined;

  constructor(public dialog: MatDialog, public imageSanitizer: ImageSanitizerService) {}

  setEditId(id: number) {
    this.idEmitter.emit(id);
  }

  openDialog(): void {
    this.dialog.open(ViewBookComponent, {
      data: {bookId: this.book?.id},
    });
  }
}
