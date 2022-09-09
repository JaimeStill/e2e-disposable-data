import {
    Component,
    EventEmitter,
    Input,
    Output
} from '@angular/core';

import { Topic } from '../../models';

@Component({
    selector: 'topic-card',
    templateUrl: 'topic-card.component.html'
})
export class TopicCardComponent {
    @Input() topic: Topic;
    @Input() expanded: boolean = false;
    @Input() selected: boolean = false;

    @Output() edit = new EventEmitter<Topic>();
    @Output() remove = new EventEmitter<Topic>();

    toggleExpanded = () => this.expanded = !this.expanded;
}
