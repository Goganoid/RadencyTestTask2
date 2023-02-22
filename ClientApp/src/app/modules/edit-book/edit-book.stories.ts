import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { genreOptions } from 'src/app/shared/constants/constants';
import { MatSelectModule } from '@angular/material/select';
import { ErrorMessageComponent } from './components/error-message/error-message.component';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { BookService } from './../../shared/services/book.service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { Meta, moduleMetadata, Story } from "@storybook/angular";
import { EditBookComponent } from './edit-book.component';
import { MatDividerModule } from '@angular/material/divider';
import { MatInputModule } from '@angular/material/input';
import { MaxSizeValidator } from '@angular-material-components/file-input';

export default {
    component: EditBookComponent,
    title: 'Edit book',
    decorators: [
        moduleMetadata({
            imports: [MatSnackBarModule, MatButtonModule, FormsModule,
                ReactiveFormsModule, MatFormFieldModule, MatInputModule,
                MatDividerModule, MatSelectModule, BrowserAnimationsModule, NoopAnimationsModule],
            providers: [BookService, HttpClient, HttpHandler],
            declarations: [ErrorMessageComponent]
        })
    ]
} as Meta;
const formGroup = new FormGroup({
    'title': new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
    'genre': new FormControl<string | null>(null, [Validators.required]),
    'author': new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
    'content': new FormControl('', [Validators.required, Validators.minLength(25), Validators.maxLength(3000)]),
    'file': new FormControl<any | null>(null, [
        Validators.required,
        // 2 MB
        MaxSizeValidator(2 * Math.pow(10, 6))
    ])
});
const Template: Story = args => ({
    props: {
        ...args,
    },
});

export const Default = Template.bind({});
Default.args = {
    editId: undefined,
    formGroup: formGroup,
    genres: genreOptions
};
