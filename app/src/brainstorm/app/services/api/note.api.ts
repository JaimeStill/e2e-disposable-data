import {
    Note,
    QuerySource
} from '../../models';

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { QueryGeneratorService } from '../core';
import { EntityBaseApi } from './base';

@Injectable({
    providedIn: 'root'
})
export class NoteApi extends EntityBaseApi<Note> {
    constructor(
        protected http: HttpClient,
        protected generator: QueryGeneratorService
    ) {
        super('note', generator, http);
    }

    queryByTopic = (topicId: number, sortProperty: string = 'id'): QuerySource<Note> =>
        this.generator.generateSource(
            `${this.api}queryByTopic/${topicId}`,
            sortProperty
        );

    validateTitle = (note: Note): Promise<boolean> =>
        firstValueFrom(
            this.http.post<boolean>(`${this.api}validateTitle`, note)
        );
}
