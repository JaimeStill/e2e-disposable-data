import {
    Rig
} from '../../../rig';

import { topic } from '../fixtures';

export default class Home {
    private static selectField = (field: string) =>
        cy.get('topic-form')
            .contains(field)
            .parent()
            .parent()
            .click({ force: true });

    private static inputField = (field: string, input: string) =>
        this.selectField(field)
            .clear()
            .type(input);

    private static selectTopic = (name: string) =>
        cy.get('async-source topic-card')
            .contains(name)
            .parentsUntil('topic-card');

    private static clickTopicButton = (name: string, idx: number) =>
        this.selectTopic(name)
            .find('button')
            .eq(idx)
            .click();

    private static save = () =>
        cy.get('topic-editor')
            .contains('Save')
            .click({ force: true });

    private static add = (name: string, description: string) =>
        it(`Add Topic ${name}`, () => {
            this.inputField('Name', name);
            this.inputField('Description', description);

            this.save();

            cy.wait(1000);

            this.clickTopicButton(name, 0);
        })

    private static edit = (name: string, update: string) =>
        it(`Edit Topic ${name} -> ${update}`, () => {
            this.clickTopicButton(name, 0);

            this.inputField('Name', update);

            this.save();

            cy.wait(1000);
        })

    private static delete = (name: string) =>
        it(`Delete Topic ${name}`, () => {
            this.clickTopicButton(name, 1);

            cy.get('mat-dialog-actions')
              .find('button')
              .eq(1)
              .click();
        })

    private static init = (rig: Rig) => {
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
        });
    }

    static test = () =>
        describe('Brainstorm Test', () => {
            const rig = new Rig();

            this.init(rig);

            it('Navigates to Home', () => {
                cy.visit('/')
                    .contains('Brainstorm');

                cy.wait(2400);
            });

            this.add('Reminder', 'Things that you should remember');
            this.edit('Reminder', 'Remember');
            this.delete('Remember');

            it ('Captures Home', () => {
                cy.screenshot('brainstorm', {
                    capture: 'fullPage'
                });
            });
        });
}
