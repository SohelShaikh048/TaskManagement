import { Routes } from '@angular/router';
import { authGuard } from './Guards/auth-guard';

export const routes: Routes = [
    {
        path: 'Projects',
        loadComponent: () =>
            import('./Components/projects/projects').then(d => d.Projects),
        canActivate: [authGuard]
    },
    {
        path: 'Boards',
        loadComponent: () =>
            import('./Components/boards/boards').then(d => d.Boards),
        canActivate: [authGuard]
    },
    {
        path: 'TaskItems',
        loadComponent: () =>
            import('./Components/task-items/task-items').then(d => d.TaskItems),
        canActivate: [authGuard]
    },
    {
        path: 'Auth/login',
        loadComponent: () =>
            import('./Components/Auth/login/login').then(d => d.Login)
    }
];
