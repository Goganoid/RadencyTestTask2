import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { Meta, moduleMetadata, Story } from '@storybook/angular';
import { book64 } from 'src/assets/bookImage64';
import { BookListItemComponent } from './book-list-item.component';
export default {
    component: BookListItemComponent,
    title: 'Book list item',
    decorators: [
        moduleMetadata({
            imports: [MatDialogModule, MatSnackBarModule, MatCardModule, MatButtonModule],
        })
    ]
} as Meta;
const Template: Story = args => ({
    props: {
        ...args,
    },
});

export const Default = Template.bind({});
Default.args = {
    book: {
        id: 1,
        title: "title",
        cover: book64,
        author: "Author",
        rating: 5,
        reviews: 20
    },
};
