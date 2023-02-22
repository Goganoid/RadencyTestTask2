import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatDialogModule } from "@angular/material/dialog";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { Meta, moduleMetadata, Story } from "@storybook/angular";
import { of } from 'rxjs';
import { book64 } from "src/assets/bookImage64";
import { BookListItemComponent } from './../book-list-item/book-list-item.component';
import { ViewBookComponent } from './../view-book/view-book.component';
import { BookListComponent } from "./book-list.component";

export default {
    component: BookListComponent,
    title: 'Book list',
    decorators: [
        moduleMetadata({
            imports: [MatDialogModule, MatDialogModule, MatSnackBarModule, MatCardModule, MatButtonModule],
            declarations: [BookListItemComponent, ViewBookComponent]
        })
    ]
} as Meta;

const templateBook = {
    id: 1,
    title: "title",
    cover: book64,
    author: "Author",
    rating: 5,
    reviews: 20
};
const Template: Story = args => ({
    props: {
        ...args,
    },
});

export const Default = Template.bind({});
Default.args = {
    books$: of(Array.from({ length: 10 }, (_, i) => templateBook)),
    recommendedBooks$: of(Array.from({ length: 10 }, (_, i) => templateBook))
};
