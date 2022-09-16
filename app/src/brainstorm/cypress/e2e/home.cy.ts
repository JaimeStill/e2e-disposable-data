import {
    Rig
} from '../../../rig';

import { topic } from '../fixtures';

describe('Brainstorm Tests', () => {
    const rig = new Rig();

    before(() => {
        cy.then(() =>
            rig.initializeDatabase().then(() =>
                cy.log('Database initialized')
            )
        );

        cy.then(() =>
            rig.startProcess().then(() =>
                cy.log('API Process started')
            )
        );

        cy.then(() =>
            rig.seed(topic, 'seedTopic').then(() =>
                cy.log('Topic seeded')
            )
        );
    });

    after(() => {
        cy.then(() =>
            rig.killProcess().then(() =>
                cy.log('API Process disposed')
            )
        );

        cy.then(() =>
            rig.destroyDatabase().then(() =>
                cy.log('Database disposed')
            )
        );
    })

    it('Test the Brainstorm module', () => {
        cy.visit('/')
            .contains('Brainstorm');

        cy.wait(3000)
            .screenshot('brainstorm', {
                capture: "fullPage"
            });
    });
});
