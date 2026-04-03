export interface ResponseDto<T = any> {
  result: T;
  isSuccess: boolean;
  message: string;
}