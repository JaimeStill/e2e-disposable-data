import {
    Component,
    EventEmitter,
    Input,
    OnDestroy,
    Output
} from '@angular/core';

import {
    FormArray,
    FormGroup
} from '@angular/forms';

import {
    ApiValidator,
    TopicApi
} from '../services';

import { Subscription } from 'rxjs';

@Component({
    selector: 'topic-form',
    templateUrl: 'topic.form.html',
    providers: [
        ApiValidator,
        TopicApi
    ]
})
export class TopicForm implements OnDestroy {
    private sub: Subscription;
    form: FormGroup;

    get name() { return this.form?.get('name') }
    get notes() { return this.form?.get('notes') as FormArray }

    private registerValidator = async () => {
        this.sub?.unsubscribe();

        this.sub = await this.validator.registerValidator(
            this.topicApi.validateName,
            this.form,
            this.name
        );
    }

    @Input() set data (data: FormGroup) {
        this.form = data;
        this.registerValidator();

        this.notes
            .valueChanges
            .subscribe(() => this.form.controls.notes = this.notes);
    }

    @Output() add = new EventEmitter();
    @Output() remove = new EventEmitter<number>();

    constructor(
        private validator: ApiValidator,
        private topicApi: TopicApi
    ) { }

    ngOnDestroy(): void {
        this.sub?.unsubscribe();
    }
}
