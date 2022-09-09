import {
    Component,
    Input
} from '@angular/core';

@Component({
    selector: 'card',
    templateUrl: 'card.component.html'
})
export class CardComponent {
    @Input() width: number | string = 360;
    @Input() alignment = 'start stretch';
    @Input() cardStyle = 'card-outline-accent rounded';
    @Input() padding: number = 8;
    @Input() margin: number = 4;
}
