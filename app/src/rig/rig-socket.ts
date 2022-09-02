import {
    HttpTransportType,
    HubConnection,
    HubConnectionBuilder
} from '@microsoft/signalr';

import { BehaviorSubject } from 'rxjs';
import { RigOutput } from './rig-output';
import { environment } from '../brainstorm/environments/environment';

interface SocketState {
    connected: boolean,
    error: any | null
}

export class RigSocket {
    private endpoint = `${environment.rig}rig-socket`;
    private connection: HubConnection;
    private output = new Array<RigOutput>();
    private socket = new BehaviorSubject<SocketState>({
        connected: false,
        error: null
    });

    socket$ = this.socket.asObservable();

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl(this.endpoint)
            .build();

        this.connection.on(
            'output',
            (data: RigOutput) => {
                console.table(data);
                this.output.push(data);
            }
        );

        this.connection
            .start()
            .then(() => this.socket.next({
                connected: true,
                error: null
            }))
            .catch((error: any) => this.socket.next({
                connected: false,
                error
            }));
    }
}
