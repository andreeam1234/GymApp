export interface CurrentUser {
  id: string;
  name: string;
  email: string;
  roles: string[];
}

export interface AuthResponse {
  token: string;
  expiresIn: number;
  userId: string;
  email: string;
  fullName: string;
  roles: string[];
}

export interface GymClass {
  id: number;
  name: string;
  description: string;
  durationMinutes: number;
  category: string;
  sessionCount: number;
}

export interface ClassSession {
  id: number;
  gymClassId: number;
  gymClassName: string;
  trainerId: string;
  trainerName: string;
  startTime: string;
  endTime: string;
  capacity: number;
  room: string;
  enrolledCount: number;
}

export interface Enrollment {
  id: number;
  userId: string;
  userName: string;
  classSessionId: number;
  gymClassName: string;
  sessionStartTime: string;
  status: 'Confirmed' | 'Cancelled' | 'Attended' | 'NoShow';
  enrolledAt: string;
}

export interface CreateEnrollmentDto {
  classSessionId: number;
}
