import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClassSessionService } from '../../../core/services/class-session';
import { EnrollmentService } from '../../../core/services/enrollment';
import { AuthService } from '../../../core/services/auth';
import { ClassSession } from '../../../shared/models/gym';

@Component({
  selector: 'app-session-list',
  imports: [CommonModule],
  templateUrl: './session-list.html',
  styleUrl: './session-list.css'
})
export class SessionList implements OnInit {
  private sessionService = inject(ClassSessionService);
  private enrollmentService = inject(EnrollmentService);
  authService = inject(AuthService);

  sessions = signal<ClassSession[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  // id-ul sesiunii aflate curent in proces de booking (pentru spinner per-card)
  bookingId = signal<number | null>(null);
  bookingError = signal<string | null>(null);
  bookingSuccessId = signal<number | null>(null);

  ngOnInit(): void {
    this.loadSessions();
  }

  loadSessions(): void {
    this.loading.set(true);
    this.error.set(null);

    this.sessionService.getUpcoming().subscribe({
      next: sessions => {
        this.sessions.set(sessions);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Nu am putut incarca sesiunile disponibile. Incearca din nou.');
        this.loading.set(false);
      }
    });
  }

  book(session: ClassSession): void {
    if (!this.authService.isAuthenticated()) {
      this.bookingError.set('Trebuie sa fii autentificat pentru a rezerva.');
      return;
    }

    this.bookingId.set(session.id);
    this.bookingError.set(null);
    this.bookingSuccessId.set(null);

    this.enrollmentService.enroll({ classSessionId: session.id }).subscribe({
      next: () => {
        this.bookingId.set(null);
        this.bookingSuccessId.set(session.id);
        this.loadSessions(); // refresh enrolled count
      },
      error: err => {
        this.bookingId.set(null);
        this.bookingError.set(err.error?.message ?? 'Rezervarea a esuat.');
      }
    });
  }

  isFull(session: ClassSession): boolean {
    return session.enrolledCount >= session.capacity;
  }
}
