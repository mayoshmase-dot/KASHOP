using KASHOP.BLL.Common;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Extenstions
{
    public static class ResultExtensions
    {
        public static IActionResult ToErrorResponse<T> (this Result <T> result)
        {
            var body = new {errors  = result.Errors};
            if (result.ErrorType == ErrorType.UnAuthorized)
             return new UnauthorizedObjectResult(body){ StatusCode = 403};

            if (result.ErrorType == ErrorType.Forbidden)
                return new ObjectResult(body);

            if (result.ErrorType == ErrorType.NotFound)
                return new NotFoundObjectResult(body);

            if (result.ErrorType == ErrorType.Conflict)
                return new ConflictObjectResult(body);

            return new BadRequestObjectResult(body);
        }
    }
}
