import { newBookIdReducer } from './store/new-book-id.reducer';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BookListItemComponent } from './modules/book-list-item/book-list-item.component';
import { BookListComponent } from './modules/book-list/book-list.component';
import { EditBookComponent } from './modules/edit-book/edit-book.component';
import { ViewBookComponent } from './modules/view-book/view-book.component';

import { NgxMatFileInputModule } from '@angular-material-components/file-input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { BooksPageComponent } from './modules/books-page/books-page.component';
import {MatSnackBarModule} from '@angular/material/snack-bar'; 
import { ErrorMessageComponent } from './modules/edit-book/components/error-message/error-message.component';
import { StoreModule } from '@ngrx/store';
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
    MatProgressSpinnerModule,
    MatSnackBarModule,
    StoreModule.forRoot({}, {}),
    StoreModule.forRoot({id:newBookIdReducer})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
