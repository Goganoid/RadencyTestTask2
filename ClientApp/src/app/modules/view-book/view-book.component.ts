import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { BookService } from 'src/app/shared/services/book.service';
import { ImageSanitizerService } from 'src/app/shared/services/image-sanitizer.service';
import { BookDetails } from './../../shared/models/BookDetails';
import { ModalData } from './../book-list-item/book-list-item.component';

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
  public onCloseClick(): void {
    this.dialogRef.close();
  }
}
