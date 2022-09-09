import {
    Component,
    OnDestroy,
    OnInit
} from '@angular/core';

import {
    QuerySource,
    Topic
} from '../../models';

import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialog } from '../../dialogs';
import { TopicApi } from '../../services';

@Component({
    selector: 'home-route',
    templateUrl: 'home.route.html',
    providers: [TopicApi]
})
export class HomeRoute implements OnInit, OnDestroy {
    topic: Topic;
    topicSrc: QuerySource<Topic>;

    constructor(
        private dialog: MatDialog,
        public topicApi: TopicApi
    ) { }

    private new = () => {
        return {
            id: 0,
            name: '',
            url: '',
            description: ''
        } as Topic;
    }

    ngOnInit(): void {
        this.topic = this.new();
        this.topicSrc = this.topicApi.query();
    }

    ngOnDestroy(): void {
        this.topicSrc.unsubscribe();
    }

    refresh = (topic: Topic) => {
        this.topic = topic;
        this.topicSrc.refresh(true);
    }

    selected = (topic: Topic) => topic.id === this.topic.id;

    selectTopic = (topic: Topic) =>
        this.topic = this.selected(topic)
            ? this.new()
            : topic;

    removeTopic = (topic: Topic) => this.dialog.open(ConfirmDialog, {
        data: {
            title: 'Remove Topic',
            content: `Are you sure you want to remove Topic ${topic.name}?`
        },
        autoFocus: false,
        disableClose: true
    })
    .afterClosed()
    .subscribe(async (result: boolean) => {
        if (result) {
            const res = await this.topicApi.remove(topic);

            if (res) {
                this.selected(topic)
                    ? this.refresh(this.new())
                    : this.topicSrc.refresh(true);
            }
        }
    })
}
