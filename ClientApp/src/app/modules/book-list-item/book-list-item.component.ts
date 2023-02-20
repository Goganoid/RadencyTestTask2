import { BookListItem } from './../../shared/models/BookListItems';
import { BookListComponent } from './../book-list/book-list.component';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ViewBookComponent } from '../view-book/view-book.component';


export interface ModalData{
  book: string;
}

@Component({
  selector: 'app-book-list-item',
  templateUrl: './book-list-item.component.html',
  styleUrls: ['./book-list-item.component.css']
})
export class BookListItemComponent {

  @Output() idEmitter: EventEmitter<number> = new EventEmitter<number>;
  @Input() book:BookListItem | undefined;

  constructor(public dialog: MatDialog) {}

  setEditId(id: number) {
    console.log("Click");
    this.idEmitter.emit(id);
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(ViewBookComponent, {
      data: {book: this.book},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
}
