import {
    Component,
    EventEmitter,
    Input,
    Output
} from '@angular/core';

@Component({
    selector: 'status-button',
    templateUrl: 'status-button.component.html'
})
export class StatusButtonComponent {
    @Input() status: boolean = false;
    @Input() label: string = 'Action';
    @Input() width: number = 360;
    @Input() disabled: boolean = false;
    @Input() layout: string = 'row';
    @Input() alignment: string = 'space-between center';
    @Output() action: EventEmitter<any> = new EventEmitter();
}
