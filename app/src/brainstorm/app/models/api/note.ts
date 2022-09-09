import {
    FormBuilder,
    FormGroup,
    Validators
} from '@angular/forms';

import { EntityBase } from './entity-base';
import { Topic } from './topic';

export interface Note extends EntityBase {
    topicId: number;
    title: string;
    body: string;
    priority: number;

    topic?: Topic;
}

export const GenerateNoteForm = (note: Note, fb: FormBuilder): FormGroup =>
    fb.group({
        id: [note?.id],
        topicId: [note?.topicId],
        title: [note?.title, Validators.required],
        body: [note?.body],
        priority: [note?.priority, [ Validators.min(0), Validators.max(5) ]]
    });
