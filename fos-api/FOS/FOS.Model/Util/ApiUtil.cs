using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FOS.Model.Util
{
    public static class ApiUtil
    {
        public static ApiResponse CreateSuccessfulResult()
        {
            return new ApiResponse()
            {
                Success = true,
            };
        }
        public static ApiResponse CreateFailResult(string errorMessage)
        {
            return new ApiResponse()
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
    public static class ApiUtil<T>
    {
        public static ApiResponse<T> CreateSuccessfulResult(T data)
        {
            return new ApiResponse<T>()
            {
                Success = true,
                Data = data
            };
        }
        public static ApiResponse<T> CreateFailResult(string errorMessage)
        {
            return new ApiResponse<T>()
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
