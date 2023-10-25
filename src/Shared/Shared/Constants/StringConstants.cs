﻿using System;
namespace Shared.Constants
{
    public class StringConstants
    {
        #region DefaultPass
        public static class DefaultValues
        {
            public const string ROOT_DEFAULT_PASS = "DefaultPass#123";
            public const string ROOT_ADMIN_PHONENUMBER = "08038441300";
            public const string STERLOAN_DB_CONNECTION = "Host=localhost;Database=sterloan;";
            public const string STERLOAN_REDIS_CONNECTION = "localhost:6479";
            public const string JWT_SECRET_KEY = "SampleKey&392929SampleKey&392929SampleKey&392929";
            public const string ROOT_ADMIN_EMAIL = "dev.nuhu@smsbuja.com";
            public const string MINIO_ACCESS_KEY = "minioadmin";
            public const string MINIO_SECRET_KEY = "minioadmin";
            public const string MINIO_ENDPOINT = "127.0.0.1:9000";
        }
        #endregion

        #region ResponseCodes
        public static class ResponseCodes
        {
            public const int Status200OK = 200;
            public const int Status201Created = 201;
            public const int Status400BadRequest = 400;
            public const int Status401Unauthorized = 401;
            public const int Status403Forbidden = 403;
            public const int Status404NotFound = 404;
            public const int Status405MethodNotAllowed = 405;
            public const int Status406NotAcceptable = 406;
            public const int Status500InternalServerError = 500;
        }
        #endregion
    }
}

