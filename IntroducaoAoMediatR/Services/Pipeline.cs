using MediatR;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Services
{
    public class Timer<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var timer = Stopwatch.StartNew();

            var result = next();

            var elapsed = timer.Elapsed;

            return result;
        }
    }

    public class Logging<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            Debug.WriteLine("Log - Start");

            var result = next();

            Debug.WriteLine("Log - End");

            return result;
        }
    }

    public class Validator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var result = next();

            return result;
        }
    }
}