import {
    Note,
    Topic
} from '../../app/models';

export const topic: Topic = {
    id: 0,
    url: null,
    name: 'Topic Test',
    description: 'Cypress testing topic',
    notes: [
        {
            id: 0,
            url: null,
            title: 'Note A',
            body: 'Note A generated for Cypress test',
            priority: 3
        } as Note,
        {
            id: 0,
            url: null,
            title: 'Note B',
            body: 'Note B generated for Cypress test',
            priority: 2
        } as Note
    ]
}
