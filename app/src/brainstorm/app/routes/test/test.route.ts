import { Component } from '@angular/core';

import {
    Rig,
    RigSocket,
    RigState
} from '../../../../rig';

import { Topic } from '../../models';

@Component({
    selector: 'test-route',
    templateUrl: 'test.route.html'
})
export class TestRoute {
    private rig: Rig = new Rig();
    state: RigState;
    socket: RigSocket;
    loading: boolean = true;
    topic: Topic;

    topicSeed: Topic = {
        id: 0,
        url: null,
        name: 'TypeScript Rig',
        description: 'Seeded from TypeScript Rig client'
    } as Topic;

    private new = () => {
        return {
            id: 0,
            name: '',
            url: '',
            description: ''
        } as Topic;
    }

    private init = async () => {
        this.state = await this.rig.getState();
        console.table(this.state);
        this.loading = false;
    }

    constructor() {
        this.topic = this.new();
        this.init();
        this.socket = new RigSocket();
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

    seeded = () => this.topic = this.new();

    seedTopic = async () => {
        this.loading = true;
        this.topicSeed = await this.rig.seed(this.topicSeed, 'seedTopic');
        console.table(this.topicSeed);
        this.loading = false;
    }
}
