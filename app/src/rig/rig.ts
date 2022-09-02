import {
    EntityBase
} from '../brainstorm/app/models';

import { RigState } from './rig-state';

export class Rig {
    protected readonly baseUrl: string;
    protected post = async (endpoint: string, data: string): Promise<any> => (
        await fetch(endpoint, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: data
        })
    ).json();

    constructor(
        protected url: string = `http://localhost:5001/api/rig/`
    ) {
        this.baseUrl = url ?? `http://localhost:5001/api/rig/`;
    }

    getState = async (): Promise<RigState> =>
        (await fetch(`${this.baseUrl}getState`)).json();

    initializeDatabase = async (): Promise<RigState> =>
        (await fetch(`${this.baseUrl}initializeDatabase`)).json();

    destroyDatabase = async (): Promise<RigState> =>
        (await fetch(`${this.baseUrl}destroyDatabase`)).json();

    startProcess = async (): Promise<RigState> =>
        (await fetch(`${this.baseUrl}startProcess`)).json();

    killProcess = async (): Promise<RigState> =>
        (await fetch(`${this.baseUrl}killProcess`)).json();

    seed = async <T extends EntityBase>(entity: T, endpoint: string) =>
        this.post(`${this.baseUrl}${endpoint}`, JSON.stringify(entity));

    seedMany = async <T extends EntityBase>(entities: T[], endpoint: string) =>
        this.post(`${this.baseUrl}${endpoint}`, JSON.stringify(entities));
}
