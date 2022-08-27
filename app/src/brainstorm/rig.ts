import { firstValueFrom, map, tap } from 'rxjs';
import { ajax } from 'rxjs/ajax';

import {
    Note,
    Topic
} from './app/models';

export default class Rig {
    private baseUrl: string = 'http://localhost:5001/api/rig/';

    getConnectionString = async (): Promise<string> => (await fetch(`${this.baseUrl}getConnectionString`)).text();
}
