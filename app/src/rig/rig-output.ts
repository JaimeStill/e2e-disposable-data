import { RigMessage } from './rig-message';
import { RigState } from './rig-state';

export interface RigOutput {
    state: RigState;
    output: RigMessage;
    exiting: boolean;
}
