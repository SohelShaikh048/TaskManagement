import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of, switchMap } from 'rxjs';
import { ProjectDto } from '../../Models/project.model';
import { ProjectService } from '../../Services/project-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-projects',
  imports: [CommonModule, FormsModule],
  templateUrl: './projects.html',
  styleUrl: './projects.css',
})
export class Projects implements OnInit {

  private refresh$ = new BehaviorSubject<void>(undefined);

  projects$!: Observable<ProjectDto[]>;
  errorMessage: string = '';

  // modal state
  showModal = false;
  isEditMode = false;
  isSaving = false;
  modalError = '';

  // form fields
  formId = '';
  formName = '';
  formDescription = '';
  formOwnerId? = '';

  constructor(private service: ProjectService) { }

  ngOnInit() {
    this.projects$ = this.refresh$.pipe(
      switchMap(() =>
        this.service.GetAll().pipe(
          map(response => {
            if (response.isSuccess && Array.isArray(response.result)) {
              this.errorMessage = '';
              return response.result;
            }
            this.errorMessage = response.message || 'Failed to load projects.';
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

  // ── Modal Controls ────────────────────────────────────────

  openCreateModal() {
    this.isEditMode = false;
    this.formId = '';
    this.formName = '';
    this.formDescription = '';
    this.formOwnerId = '';
    this.modalError = '';
    this.showModal = true;
  }

  openEditModal(project: ProjectDto) {
    this.isEditMode = true;
    this.formId = project.id;
    this.formName = project.name;
    this.formDescription = project.description;
    this.formOwnerId = project.ownerId ?? '';
    this.modalError = '';
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.modalError = '';
  }

  // ── Save (Create or Update) ───────────────────────────────

  onSave() {
    if (!this.formName.trim()) {
      this.modalError = 'Project name is required.';
      return;
    }
    if (!this.formDescription.trim()) {
      this.modalError = 'Description is required.';
      return;
    }

    this.isSaving = true;
    this.modalError = '';

    const payload: ProjectDto = {
      id: this.isEditMode ? this.formId : '00000000-0000-0000-0000-000000000000',
      name: this.formName.trim(),
      description: this.formDescription.trim(),
      ownerId: this.formOwnerId?.trim() || undefined  // optional field
    };

    const request$ = this.service.Create(payload);

    request$.subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.closeModal();
          this.refresh();
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

  // ── Delete ────────────────────────────────────────────────

  onDelete(id: string) {
    if (!confirm('Are you sure you want to delete this project?')) return;

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
