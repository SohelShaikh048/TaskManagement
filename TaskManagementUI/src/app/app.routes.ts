import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'Projects',
        loadComponent: () =>
            import('./Components/projects/projects').then(d => d.Projects)
    },
    {
        path: 'Boards',
        loadComponent: () =>
            import('./Components/boards/boards').then(d => d.Boards)
    },
    {
        path: 'TaskItems',
        loadComponent: () =>
            import('./Components/task-items/task-items').then(d => d.TaskItems)
    }
];
