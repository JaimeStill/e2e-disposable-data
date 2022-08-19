import { EntityBase } from './entity-base';
import { Topic } from './topic';

export interface Note extends EntityBase {
    topicId: number;
    title: string;
    body: string;
    priority: number;

    topic?: Topic;
}
