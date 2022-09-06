import {
    Component,
    Input
} from '@angular/core';

import { Topic } from '../../../models';

@Component({
    selector: 'topic-card',
    templateUrl: 'topic-card.component.html'
})
export class TopicCardComponent {
    @Input() topic: Topic;
}
