import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-books-page',
  templateUrl: './books-page.component.html',
  styleUrls: ['./books-page.component.css']
})
export class BooksPageComponent {
  id: number | undefined;
  setEmitId(id: number) {
    console.log(`Received id ${id}`)
    this.id = id;
  }

}
