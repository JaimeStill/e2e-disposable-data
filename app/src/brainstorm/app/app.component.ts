import { Component } from '@angular/core';
import Rig from '../rig';

import {
    ThemeService
} from './services';

import {
    Note,
    Topic
} from './models';

@Component({
    selector: 'app-root',
    templateUrl: 'app.component.html'
})
export class AppComponent {
    private rig: Rig = new Rig();

    connection: string | null = null;
    initialized: boolean = false;
    processStarted: boolean = false;

    topic: Topic = {
        id: 0,
        url: null,
        name: 'TypeScript Rig',
        description: 'Posted from TypeScript RigService client',
        notes: null
    };

    constructor(
        public themer: ThemeService
    ) { }

    getConnection = async () => {
        this.connection = await this.rig.getConnectionString();
        console.log('Connection', this.connection);
    }

    initializeDatabase = async () => {
        this.initialized = await this.rig.initializeDatabase();
        console.log('Initialized', this.initialized);
    }

    startProcess = async () => {
        this.processStarted = await this.rig.startProcess();
        console.log('Process Started', this.processStarted);
    }

    seedTopic = async () => {
        this.topic = await this.rig.seed(this.topic, 'seedTopic');
        console.table(this.topic);
    }
}
