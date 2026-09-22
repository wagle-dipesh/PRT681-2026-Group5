export interface Task {
  id: number;
  title: string;
  description: string | null;
  isCompleted: boolean;
  createdAt: string;
}

export interface TaskRequest {
  title: string;
  description: string;
  isCompleted: boolean;
}