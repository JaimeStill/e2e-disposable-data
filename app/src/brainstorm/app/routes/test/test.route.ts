import { Component } from '@angular/core';
import Rig from '../../../rig';

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

    connection: string | null = null;
    initialized: boolean = false;
    loading: boolean = false;
    processStarted: boolean = false;

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

    getConnection = async () => {
        this.loading = true;
        this.connection = await this.rig.getConnectionString();
        console.log('Connection', this.connection);
        this.loading = false;
    }

    initializeDatabase = async () => {
        this.loading = true;
        this.initialized = await this.rig.initializeDatabase();
        console.log('Initialized', this.initialized);
        this.loading = false;
    }

    startProcess = async () => {
        this.loading = true;
        this.processStarted = await this.rig.startProcess();
        console.log('Process Started', this.processStarted);
        this.loading = false;
    }

    seedTopic = async () => {
        this.loading = true;
        this.topic = await this.rig.seed(this.topic, 'seedTopic');
        console.table(this.topic);
        this.loading = false;
    }
}
