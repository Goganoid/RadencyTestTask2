import { createReducer, on } from "@ngrx/store";
import { resetId, setId } from "./new-book-id.actions";

export const initialState = 0;

export const newBookIdReducer = createReducer(
    initialState,
    on(setId, (state, props) => state = props.id),
    on(resetId,(state,props) => state=0),
)