import { ValidationError } from "./ValidationError";

export interface Response<T>{
    response: T | null,
    errors: ValidationError[] | null,
}