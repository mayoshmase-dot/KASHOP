using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.BLL.Common
{
    public enum ErrorType
    {
        None,
        Validation,
        UnAuthorized,
        Forbidden,
        NotFound,
        Conflict

    }
    public class Result <T>
    {
        public T? Data {  get; set; }
        public bool IsSuccess { get;}
        public List<string> Errors { get; set; }
        public ErrorType ErrorType { get; set; }
        private Result(bool isSuccess, T? data, List<string> errors, ErrorType errorType)
        {
            IsSuccess = isSuccess;
                Data = data;
            Errors = errors;
            ErrorType = errorType;
        }
        public static Result<T> Success(T data) => new(true, data, [], ErrorType.None);
        public static Result<T> Validation(List <string> errors) => new(false, default, errors, ErrorType.Validation);
        public static Result<T> UnAuthorized(List<string> errors) => new(false, default, errors, ErrorType.UnAuthorized);
        public static Result<T> Forbidden(List<string> errors) => new(false, default, errors, ErrorType.Forbidden);
        public static Result<T> NotFound(List<string> errors) => new(false, default, errors, ErrorType.NotFound);
        public static Result<T> Conflict(List<string> errors) => new(false, default, errors, ErrorType.Conflict);


    }
}
