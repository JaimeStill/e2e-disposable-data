import { EntityBase } from './entity-base';
import { Note } from './note';

export interface Topic extends EntityBase {
    name: string;
    description: string;

    notes?: Note[];
}
