import { Review } from "./Review";

export interface BookDetails {
    bookId: number;
    title: string;
    cover: string;
    content: string;
    author: string;
    genre: string;
    rating: number;
    reviews: Review[];
}