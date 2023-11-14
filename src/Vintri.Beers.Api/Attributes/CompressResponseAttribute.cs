using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Http.Filters;

namespace Vintri.Beers.Api.Attributes;

    /// <summary>
    /// Compress Response Content
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CompressResponseAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuted(HttpActionExecutedContext context) => TryCompressResponse();

        private static bool CanCompressResponse()
        {
            var acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            return !string.IsNullOrEmpty(acceptEncoding) && (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate"));
        }

        private static void TryCompressResponse()
        {
            var response = HttpContext.Current.Response;

            if (!CanCompressResponse()) return;

            var acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            var contentEncoding = acceptEncoding.Contains("gzip") ? "gzip" : "deflate";

            response.Filter = CreateStream(contentEncoding, response.Filter);
            response.Headers.Remove("Content-Encoding");
            response.AppendHeader("Content-Encoding", contentEncoding);
            response.AppendHeader("Vary", "Content-Encoding");
        }

        private static Stream CreateStream(string contentEncoding, Stream stream) =>
            contentEncoding == "gzip" ? new GZipStream(stream, CompressionMode.Compress) : new DeflateStream(stream, CompressionMode.Compress);
    }