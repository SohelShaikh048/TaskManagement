import { Component, OnInit } from '@angular/core';
import { TaskItemDto, TaskItemStatus } from '../../Models/taskItem.model';
import { TaskItemService } from '../../Services/task-item-service';
import { CommonModule } from '@angular/common';
import { BehaviorSubject, catchError, map, Observable, of, switchMap } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-items',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-items.html',
  styleUrl: './task-items.css',
})
export class TaskItems implements OnInit {

  // BehaviorSubject acts as a refresh trigger
  private refresh$ = new BehaviorSubject<void>(undefined);

  TaskItems$!: Observable<TaskItemDto[]>;
  errorMessage: string = '';

  // modal state
  showModal = false;
  isEditMode = false;
  isSaving = false;
  modalError = '';

  TaskItemStatus = TaskItemStatus; // to use enum in template

  // form fields
  formId = '';
  formTitle = '';
  formDescription = '';
  formStatus: TaskItemStatus = TaskItemStatus.ToDo;
  formPriority: number = 1;
  formDueDate? = '';
  formBoardId = '';

  constructor(private service: TaskItemService) { }

  ngOnInit(): void {
    this.TaskItems$ = this.refresh$.pipe(
      switchMap(() =>
        this.service.GetAll().pipe(
          map(response => {
            if (response.isSuccess && Array.isArray(response.result)) {
              // console.log('First task status:', response.result[0].status, typeof response.result[0].status);
              this.errorMessage = '';
              return response.result;
            }
            this.errorMessage = response.message || 'Failed to load boards.';
            return [];
          }),
          catchError(() => {
            this.errorMessage = 'Something went wrong. Please try again.';
            return of([]);   // of([]) not [] — must return an Observable
          })
        )
      )
    );
  }

  private refresh() {
    this.refresh$.next();
  }

  // ── Helpers ──────────────────────────────────────────────

  getStatusLabel(status: TaskItemStatus): string {
    switch (status) {
      case TaskItemStatus.ToDo: return 'To Do';
      case TaskItemStatus.InProgress: return 'In Progress';
      case TaskItemStatus.Done: return 'Done';
      default: return 'Unknown';
    }
  }

  getStatusClass(status: TaskItemStatus): string {
    switch (status) {
      case TaskItemStatus.ToDo: return 'bg-secondary';
      case TaskItemStatus.InProgress: return 'bg-warning text-dark';
      case TaskItemStatus.Done: return 'bg-success';
      default: return 'bg-secondary';
    }
  }

  getPriorityLabel(priority: number): string {
    switch (priority) {
      case 1: return 'Low';
      case 2: return 'Medium';
      case 3: return 'High';
      default: return 'Low';
    }
  }

  getPriorityClass(priority: number): string {
    switch (priority) {
      case 1: return 'text-success';
      case 2: return 'text-warning';
      case 3: return 'text-danger';
      default: return 'text-success';
    }
  }

  openCreateModal() {
    // TODO: implement create modal
    console.log('Open create modal');
    this.isEditMode = false;

    this.formId = '';
    this.formTitle = '';
    this.formDescription = '';
    this.formStatus = TaskItemStatus.ToDo;
    this.formPriority = 1;
    this.formDueDate = undefined;
    this.formBoardId = '';

    this.modalError = '';
    this.showModal = true;
  }

  openEditModal(TaskItem: TaskItemDto) {
    // TODO: implement edit modal
    console.log('Edit TaskItem:', TaskItem);
    this.isEditMode = true;

    this.formId = TaskItem.id;
    this.formTitle = TaskItem.title;
    this.formDescription = TaskItem.description;
    this.formStatus = TaskItem.status;
    this.formPriority = TaskItem.priority;
    this.formDueDate = TaskItem.dueDate?.toString().substring(0, 10); // format for input[type=date]
    this.formBoardId = TaskItem.boardId;

    this.modalError = '';
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.modalError = '';
  }

  // ── Save (Create or Update) ──────────────────────────────

  onSave() {
    if (!this.formTitle.trim()) {
      this.modalError = 'Title is required.';
      return;
    }
    if (!this.formBoardId.trim()) {
      this.modalError = 'Board ID is required.';
      return;
    }

    this.isSaving = true;
    this.modalError = '';

    const payload: TaskItemDto = {
      id: this.isEditMode ? this.formId : '00000000-0000-0000-0000-000000000000',
      title: this.formTitle.trim(),
      description: this.formDescription.trim(),
      status: this.formStatus,      // ensure number not string from select
      priority: Number(this.formPriority),  // ensure number not string from select
      dueDate: this.formDueDate ? new Date(this.formDueDate) : undefined,
      boardId: this.formBoardId.trim()
    };

    const request$ = this.isEditMode
      ? this.service.Update(payload.id, payload)
      : this.service.Create(payload);

    request$.subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.closeModal();
          this.refresh();        // refresh card grid
        } else {
          this.modalError = response.message || 'Operation failed.';
        }
        this.isSaving = false;
      },
      error: () => {
        this.modalError = 'Something went wrong. Please try again.';
        this.isSaving = false;
      }
    });
  }

  onDelete(id: string) {
    if (!confirm('Are you sure you want to delete this task?')) return;

    this.service.Delete(id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.refresh();
        } else {
          this.errorMessage = response.message || 'Delete failed.';
        }
      },
      error: () => {
        this.errorMessage = 'Delete failed. Please try again.';
      }
    });
  }

}
