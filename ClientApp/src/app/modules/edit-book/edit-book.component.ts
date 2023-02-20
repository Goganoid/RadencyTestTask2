import { MaxSizeValidator } from '@angular-material-components/file-input';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BookService } from 'src/app/shared/services/book.service';
import { genreOptions } from 'src/assets/constants';
import { BookDetails } from './../../shared/models/BookDetails';
import { SaveBook } from './../../shared/models/SaveBook';

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.css']
})
export class EditBookComponent implements OnChanges {

  @Input() editId: number | undefined;
  @Output() updateListEmitter = new EventEmitter();
  public genres = genreOptions;
  public formGroup = new FormGroup({
    'title': new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
    'genre': new FormControl<string | null>(null, [Validators.required]),
    'author': new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
    'content': new FormControl('', [Validators.required, Validators.minLength(25), Validators.maxLength(1000)]),
    'file': new FormControl<any | null>(null, [
      Validators.required,
      // 2 MB
      MaxSizeValidator(2 * Math.pow(10, 6))
    ])
  });
  
  constructor(private bookService: BookService) { }
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['editId'].isFirstChange() || changes['editId'].currentValue == undefined) return;
    this.bookService.getBook(this.editId!).subscribe(bookDetails => {
      this.setBook(bookDetails);
    });
  }

  public imgInputChange(fileInputEvent: any) {
    this.formGroup.controls['file'].markAllAsTouched();
    this.formGroup.controls['file'].setValue(fileInputEvent.target.files[0]);
    console.log(fileInputEvent.target.files[0]);
    console.log(this.formGroup.controls['file'].errors);
  }

  public submit() {
    this.formGroup.markAllAsTouched();
    if (this.formGroup.valid) {
      console.log(this.formGroup);
      let reader = new FileReader();
      // send request after converting the image to base64
      reader.onloadend = () => {
        const base64Img = reader.result! as string;
        const book = this.constructSaveBookModel(base64Img);
        console.log(book);
        this.bookService.saveBook(book).subscribe(newBookId => {
          console.log(newBookId);
          this.updateListEmitter.emit();
          this.resetForm();
        })
      }
      reader.readAsDataURL(this.formGroup.controls['file'].value);
    }
  }
  private constructSaveBookModel(base64Img: string): SaveBook {
    return {
      author: this.formGroup.controls.author.value,
      content: this.formGroup.controls.content.value,
      genre: this.formGroup.controls.genre.value,
      title: this.formGroup.controls.title.value,
      cover: base64Img,
      id: this.editId
    } as SaveBook
  }
  private resetForm() {
    this.formGroup.reset();
    this.editId = undefined;
  }
  private setBook(bookDetails:BookDetails) {
    this.formGroup.patchValue({
      'title': bookDetails.title,
      'author': bookDetails.author,
      'content': bookDetails.content,
      'genre': bookDetails.genre,
    });
    fetch(bookDetails.cover)
      .then(res => res.blob())
      .then(blob => {
        const extension = bookDetails.cover.includes("png") ? "png" : "jpeg";
        const file = new File([blob], `cover.${extension}`, { type: `image/${extension}` });
        this.formGroup.patchValue({
          'file': file
        });
      })
  }
}
