import { Route } from '@angular/router';
import { HomeRoute } from './home';
import { TestRoute } from './test';

export const RouteComponents = [
    HomeRoute,
    TestRoute
]

export const Routes: Route[] = [
    { path: 'test', component: TestRoute },
    { path: '', component: HomeRoute },
    { path: '**', redirectTo: '', pathMatch: 'full' }
]
