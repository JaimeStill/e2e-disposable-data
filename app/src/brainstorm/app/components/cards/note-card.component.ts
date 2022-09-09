import {
    Component,
    Input
} from '@angular/core';

import { Note } from '../../models';

@Component({
    selector: 'note-card',
    templateUrl: 'note-card.component.html'
})
export class NoteCardComponent {
    @Input() note: Note;
}
