export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
   message: string;
  email: string;
}

export interface SignupRequest {
  email: string;
  password: string;
}

export interface SignupResponse {
  message: string;
}

export interface CurrentUser {
  userId: string;
  email: string;
}

