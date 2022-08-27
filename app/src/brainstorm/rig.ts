import {
    Note,
    Topic
} from './app/models';

export default class Rig {
    protected readonly baseUrl: string;

    constructor(
        protected url: string = `http://localhost:5001/api/rig/`
    ) {
        this.baseUrl = url ?? `http://localhost:5001/api/rig/`;
    }

    protected

    getConnectionString = async (): Promise<string> => (await fetch(`${this.baseUrl}getConnectionString`)).text();

    initializeDatabase = async (): Promise<boolean> => (await fetch(`${this.baseUrl}initializeDatabase`)).json();

    startProcess = async(): Promise<boolean> => (await fetch(`${this.baseUrl}startProcess`)).json();
}
