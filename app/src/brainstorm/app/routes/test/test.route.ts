import { Component } from '@angular/core';

import {
    Rig,
    RigState
} from '../../../rig';

import {
    Note,
    Topic
} from '../../models';

@Component({
    selector: 'test-route',
    templateUrl: 'test.route.html'
})
export class TestRoute {
    private rig: Rig = new Rig();
    state: RigState;
    loading: boolean = true;

    private init = async () => {
        this.state = await this.rig.getState();
        console.table(this.state);
        this.loading = false;
    }

    constructor() {
        this.init();
    }

    topic: Topic = {
        id: 0,
        url: null,
        name: 'TypeScript Rig',
        description: 'Posted from TypeScript Rig client service',
        notes: [
            {
                title: 'Rig Note',
                body: 'Note generated from within a Topic'
            } as Note
        ]
    }

    private initializeDatabase = async () => {
        this.loading = true;
        this.state = await this.rig.initializeDatabase();
        console.table(this.state);
        this.loading = false;
    }

    private destroyDatabase = async () => {
        this.loading = true;
        this.state = await this.rig.destroyDatabase();
        console.table(this.state);
        this.loading = false;
    }

    private startProcess = async () => {
        this.loading = true;
        this.state = await this.rig.startProcess();
        console.table(this.state);
        this.loading = false;
    }

    private killProcess = async () => {
        this.loading = true;
        this.state = await this.rig.killProcess();
        console.table(this.state);
        this.loading = false;
    }

    toggleDatabase = () => this.state.databaseCreated
        ? this.destroyDatabase()
        : this.initializeDatabase();

    toggleProcess = () => this.state.processRunning
        ? this.killProcess()
        : this.startProcess();

    seedTopic = async () => {
        this.loading = true;
        this.topic = await this.rig.seed(this.topic, 'seedTopic');
        console.table(this.topic);
        this.loading = false;
    }
}
