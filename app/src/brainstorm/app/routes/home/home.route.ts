import {
    Component,
    OnDestroy,
    OnInit
} from '@angular/core';

import {
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
    topicSrc: QuerySource<Topic>;

    constructor(
         public topicApi: TopicApi
    ) { }

    ngOnInit(): void {
        this.topicSrc = this.topicApi.query();
    }

    ngOnDestroy(): void {
        this.topicSrc.unsubscribe();
    }
}
