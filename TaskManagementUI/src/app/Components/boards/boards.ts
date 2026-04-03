import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of, switchMap } from 'rxjs';
import { BoardDto } from '../../Models/board.model';
import { BoardService } from '../../Services/board-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-boards',
  imports: [CommonModule, FormsModule],
  templateUrl: './boards.html',
  styleUrl: './boards.css',
})

export class Boards implements OnInit {

  // BehaviorSubject acts as a refresh trigger
  private refresh$ = new BehaviorSubject<void>(undefined);

  boards$!: Observable<BoardDto[]>;
  errorMessage = '';

  // modal state
  showModal = false;
  isEditMode = false;
  isSaving = false;
  modalError = '';

  // form fields
  formId = '';
  formName = '';
  formProjectId = '';

  constructor(private boardService: BoardService) {}

  ngOnInit() {
    // assign boards$ ONCE — never reassign it again
    this.boards$ = this.refresh$.pipe(
      switchMap(() =>
        this.boardService.GetAll().pipe(
          map(response => {
            if (response.isSuccess && Array.isArray(response.result)) {
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

  // triggers existing boards$ to re-fetch — does NOT reassign it
  private refresh() {
    this.refresh$.next();
  }

  openCreateModal() {
    // TODO: implement create modal
    console.log('Open create modal');
    this.isEditMode = false;
    this.formId = '';
    this.formName = '';
    this.formProjectId = '';
    this.modalError = '';
    this.showModal = true;
  }

  openEditModal(board: BoardDto) {
    // TODO: implement edit modal
    console.log('Edit board:', board);
    this.isEditMode = true;
    this.formId = board.id;
    this.formName = board.name;
    this.formProjectId = board.projectId;
    this.modalError = '';
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.modalError = '';
  }

  // ── Save (Create or Update) ──────────────────────────────

  onSave() {
    if (!this.formName.trim()) {
      this.modalError = 'Board name is required.';
      return;
    }
    if (!this.formProjectId.trim()) {
      this.modalError = 'Project ID is required.';
      return;
    }

    this.isSaving = true;
    this.modalError = '';

    const payload: BoardDto = {
      id: this.isEditMode ? this.formId : '00000000-0000-0000-0000-000000000000',  // for create, backend should ignore this ID and generate a new one
      name: this.formName.trim(),
      projectId: this.formProjectId.trim()
    };

    const request$ = this.isEditMode
      ? this.boardService.Update(payload.id, payload)
      : this.boardService.Create(payload);

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
    if (!confirm('Are you sure you want to delete this board?')) return;

    this.boardService.Delete(id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.refresh();   // re-triggers boards$ observable
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
