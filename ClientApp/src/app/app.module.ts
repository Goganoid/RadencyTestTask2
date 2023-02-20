import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BookListComponent } from './modules/book-list/book-list.component';
import { BookListItemComponent } from './modules/book-list-item/book-list-item.component';
import { EditBookComponent } from './modules/edit-book/edit-book.component';
import { ViewBookComponent } from './modules/view-book/view-book.component';

import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { NgxMatFileInputModule } from '@angular-material-components/file-input';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog'; 
import { MatSelectModule } from '@angular/material/select'; 
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatFormFieldModule} from '@angular/material/form-field';
import { ErrorMessageComponent } from './modules/edit-book/components/error-message/error-message.component';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { BooksPageComponent } from './modules/books-page/books-page.component'; 
@NgModule({
  declarations: [
    AppComponent,
    BookListComponent,
    BookListItemComponent,
    EditBookComponent,
    ViewBookComponent,
    ErrorMessageComponent,
    BooksPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatDividerModule,
    NgxMatFileInputModule,
    MatButtonModule,
    MatTabsModule,
    MatCardModule,
    MatDialogModule,
    MatSelectModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    HttpClientModule,
    MatProgressSpinnerModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
