import {
    EntityBase
} from '../brainstorm/app/models';

import { RigState } from './rig-state';

export class Rig {
    protected readonly baseUrl: string;
    protected get = async (endpoint: string): Promise<any> => (
        await fetch(endpoint, {
            credentials: 'include'
        })
    ).json();

    protected post = async (endpoint: string, data: string): Promise<any> => (
        await fetch(endpoint, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: data
        })
    ).json();

    constructor(
        protected url: string = `http://localhost:5001/api/rig/`
    ) {
        this.baseUrl = url ?? `http://localhost:5001/api/rig/`;
    }

    getState = async (): Promise<RigState> =>
        await this.get(`${this.baseUrl}getState`);

    initializeDatabase = async (): Promise<RigState> =>
        await this.get(`${this.baseUrl}initializeDatabase`);

    destroyDatabase = async (): Promise<RigState> =>
        await this.get(`${this.baseUrl}destroyDatabase`);

    startProcess = async (): Promise<RigState> =>
        await this.get(`${this.baseUrl}startProcess`);

    killProcess = async (): Promise<RigState> =>
        await this.get(`${this.baseUrl}killProcess`);

    seed = async <T extends EntityBase>(entity: T, endpoint: string) =>
        this.post(`${this.baseUrl}${endpoint}`, JSON.stringify(entity));

    seedMany = async <T extends EntityBase>(entities: T[], endpoint: string) =>
        this.post(`${this.baseUrl}${endpoint}`, JSON.stringify(entities));
}
