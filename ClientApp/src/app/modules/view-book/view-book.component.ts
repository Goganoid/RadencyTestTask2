import { BookDetails } from './../../shared/models/BookDetails';
import { BookService } from 'src/app/shared/services/book.service';
import { ModalData } from './../book-list-item/book-list-item.component';
import { Component, Inject, Input, OnInit, Sanitizer } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BookListItem } from 'src/app/shared/models/BookListItems';
import { Observable } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import { ImageSanitizerService } from 'src/app/shared/services/image-sanitizer.service';

@Component({
  selector: 'app-view-book',
  templateUrl: './view-book.component.html',
  styleUrls: ['./view-book.component.css']
})
export class ViewBookComponent {
  public book$: Observable<BookDetails> | undefined;
  constructor(
    private bookService: BookService,
    public imageSanitizer: ImageSanitizerService,
    public dialogRef: MatDialogRef<ViewBookComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ModalData,
  ) {
    this.book$ = this.bookService.getBook(this.data.bookId);
  }
  onCloseClick(): void {
    this.dialogRef.close();
  }
}
