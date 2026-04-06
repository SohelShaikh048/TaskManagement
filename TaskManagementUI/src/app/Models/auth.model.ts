export interface RegistrationRequestDto {
    email: string;
    name: string;
    phoneNumber: string;
    password: string;
    role?: string;
}

export interface UserDto {
    id: string;
    email: string;
    name: string;
    phoneNumber: string;
}

export interface LoginRequestDto {
    userName: string;
    password: string;
}
export interface LoginResponseDto {
    token: string;
    user: UserDto;
}
