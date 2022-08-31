import {
    Component,
    EventEmitter,
    Input,
    Output
} from '@angular/core';

@Component({
    selector: 'button-block',
    templateUrl: 'button-block.component.html'
})
export class ButtonBlockComponent {
    @Input() label: string = 'Action';
    @Input() width: number = 360;
    @Input() disabled: boolean = false;
    @Output() action: EventEmitter<any> = new EventEmitter();
}
