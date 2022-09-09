import {
    Component,
    EventEmitter,
    Input,
    Output
} from '@angular/core';

import {
    FormArray,
    FormBuilder,
    FormGroup
} from '@angular/forms';

import {
    GenerateNoteForm,
    GenerateTopicForm,
    IStorage,
    Note,
    Topic
} from '../../models';

import { TopicApi } from '../../services';

@Component({
    selector: 'topic-editor',
    templateUrl: 'topic-editor.component.html'
})
export class TopicEditorComponent {
    private topic: Topic;

    storage: IStorage<Topic>;
    form: FormGroup;

    @Input() set data(data: Topic) {
        this.topic = data;
        this.load();
    }

    @Input() label = 'Topic';
    @Input() size = 420;
    @Output() update = new EventEmitter<Topic>();

    constructor(
        private fb: FormBuilder,
        public topicApi: TopicApi
    ) { }

    get notes() { return this.form.get('notes') as FormArray }

    private initNotes = (topic: Topic) => {
        if (this.notes.length > 0)
            this.notes.clear();

        if (topic.notes?.length > 0)
            for (const note of topic.notes)
                this.notes.push(GenerateNoteForm(note, this.fb));
    }

    private load = () => {
        this.storage = this.topicApi.generateStorage(this.topic);

        const value = this.storage.hasState
            ? this.storage.get()
            : this.topic;

        this.form = GenerateTopicForm(value, this.fb);

        this.initNotes(value);

        this.form
            .valueChanges
            .subscribe((topic: Topic) => this.storage.set(topic));
    }

    clearStorage = () => {
        this.form.reset(this.topic);
        this.initNotes(this.topic);
        this.storage.clear();
    }

    addNote = () => this.notes.push(
        GenerateNoteForm(
            {
                id: 0,
                topicId: this.form?.value?.id,
                title: '',
                body: '',
                url: '',
                priority: 0
            } as Note,
            this.fb
        )
    );

    removeNote = (i: number) => this.notes.removeAt(i);

    save = async () => {
        if (this.form?.valid) {
            const res = await this.topicApi.addOrUpdate(this.form.value);

            if (res) {
                this.clearStorage();
                this.update.emit(res);
            }
        }
    }
}
