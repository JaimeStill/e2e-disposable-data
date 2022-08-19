import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { QueryGeneratorService } from '../core';
import { EntityBaseApi } from './base';
import { Topic } from '../../models';

@Injectable({
    providedIn: 'root'
})
export class TopicApi extends EntityBaseApi<Topic> {
    constructor(
        protected http: HttpClient,
        protected generator: QueryGeneratorService
    ) {
        super('topic', generator, http);
    }

    validateName = (topic: Topic): Promise<boolean> =>
        firstValueFrom(
            this.http.post<boolean>(`${this.api}validateName`, topic)
        );
}
