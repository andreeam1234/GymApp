import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EnrollmentService } from '../../../core/services/enrollment';
import { Enrollment } from '../../../shared/models/gym';

@Component({
  selector: 'app-my-bookings',
  imports: [CommonModule, RouterLink],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.css'
})
export class MyBookings implements OnInit {
  private enrollmentService = inject(EnrollmentService);

  bookings = signal<Enrollment[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  cancellingId = signal<number | null>(null);

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.loading.set(true);
    this.error.set(null);

    this.enrollmentService.getMine().subscribe({
      next: bookings => {
        this.bookings.set(bookings);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Nu am putut incarca rezervarile tale.');
        this.loading.set(false);
      }
    });
  }

  cancel(booking: Enrollment): void {
    this.cancellingId.set(booking.id);

    this.enrollmentService.cancel(booking.id).subscribe({
      next: () => {
        this.cancellingId.set(null);
        this.loadBookings();
      },
      error: () => {
        this.cancellingId.set(null);
        this.error.set('Anularea a esuat. Incearca din nou.');
      }
    });
  }
}
