import {
    Component,
    EventEmitter,
    Input,
    Output
} from '@angular/core';

@Component({
    selector: 'toggle-block',
    templateUrl: 'toggle-block.component.html'
})
export class ToggleBlockComponent {
    @Input() activeLabel: string = 'Deactivate';
    @Input() inactiveLabel: string = 'Activate';
    @Input() active: boolean = false;
    @Input() disabled: boolean = false;
    @Input() width: number = 360;
    @Output() slide = new EventEmitter();
}
