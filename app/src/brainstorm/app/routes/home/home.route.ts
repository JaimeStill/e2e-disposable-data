import {
    Component,
    OnDestroy,
    OnInit
} from '@angular/core';

import {
    Note,
    QuerySource,
    Topic
} from '../../models';

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

    refresh = () => this.topicSrc.refresh(true);
}
