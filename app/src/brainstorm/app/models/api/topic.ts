import {
    FormBuilder,
    FormGroup,
    Validators
} from '@angular/forms';

import { EntityBase } from './entity-base';
import { Note } from './note';

export interface Topic extends EntityBase {
    name: string;
    description: string;

    notes?: Note[];
}

export const GenerateTopicForm = (topic: Topic, fb: FormBuilder): FormGroup =>
    fb.group({
        id: [topic?.id],
        name: [topic?.name, Validators.required],
        description: [topic?.description],
        notes: fb.array([])
    })
