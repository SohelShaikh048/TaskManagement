export interface TaskItemDto {
    id: string;
    title: string;
    description: string;
    status: TaskItemStatus;
    priority: number;
    dueDate?: Date;
    boardId: string;
}

export enum TaskItemStatus {
    ToDo = 'ToDo',
    InProgress = 'InProgress',
    Done = 'Done'
}