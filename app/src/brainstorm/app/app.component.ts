import { Component } from '@angular/core';
import Rig from '../rig';

import {
    ThemeService
} from './services';

@Component({
    selector: 'app-root',
    templateUrl: 'app.component.html'
})
export class AppComponent {
    constructor(
        public themer: ThemeService
    ) {
        this.init();
    }

    private init = async () => {
        const rig = new Rig();
        const connection = await rig.getConnectionString();
        const initialized = await rig.initializeDatabase();
        const processStarted = await rig.startProcess();
        console.log('Connection', connection);
        console.log('Initialized', initialized);
        console.log('Process Started', processStarted);
    }
}
