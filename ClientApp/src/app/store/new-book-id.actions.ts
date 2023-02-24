import { createAction, props } from "@ngrx/store";

export const setId = createAction('[Book-Edit Component] SetId',props<{id:number}>());
export const resetId = createAction('[Book-Edit Component] ResetId');

