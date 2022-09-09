import {
    firstValueFrom,
    BehaviorSubject,
    Observable,
    Subscription
} from 'rxjs';

import {
    EntityBase,
    QuerySource
} from '../../../models';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { QueryGeneratorService } from '../../core';

export abstract class EntityBaseApi<T extends EntityBase> {
    private setEndpoint = (endpoint: string): string =>
        endpoint.endsWith('/')
            ? endpoint
            : `${endpoint}/`;

    protected api: string;
    protected entity = new BehaviorSubject<T>(null);

    constructor(
        protected endpoint: string,
        protected generator: QueryGeneratorService,
        protected http: HttpClient
    ) {
        this.endpoint = this.setEndpoint(endpoint);
        this.api = `${environment.api}${this.endpoint}`;
    }

    entity$ = this.entity.asObservable();

    query = (): QuerySource<T> =>
        this.generator.generateSource<T>(
            `${this.api}query`
        );

    getById$ = (id: number): Observable<T> =>
        this.http.get<T>(`${this.api}getById/${id}`);

    getById = (id: number): Subscription =>
        this.getById$(id)
            .subscribe({
                next: (data: T) => this.entity.next(data),
                error: (err: any) => { throw new Error(err) }
            });

    getByIdAsync = (id: number): Promise<T> =>
        firstValueFrom(this.getById$(id));

    getByUrl$ = (url: string): Observable<T> =>
        this.http.get<T>(`${this.api}getByUrl/${url}`);

    getByUrl = (url: string): Subscription =>
        this.getByUrl$(url)
            .subscribe({
                next: (data: T) => this.entity.next(data),
                error: (err: any) => { throw new Error(err); }
            })

    getByUrlAsync = (url: string): Promise<T> =>
        firstValueFrom(this.getByUrl$(url));

    validate = (entity: T): Promise<boolean> =>
        firstValueFrom(
            this.http.post<boolean>(`${this.api}validate`, entity)
        );

    addOrUpdate = (entity: T): Promise<T> =>
        firstValueFrom(
            this.http.post<T>(`${this.api}addOrUpdate`, entity)
        );

    remove = (entity: T): Promise<boolean> =>
        firstValueFrom(
            this.http.delete<boolean>(`${this.api}remove`, { body: entity })
        );
}
