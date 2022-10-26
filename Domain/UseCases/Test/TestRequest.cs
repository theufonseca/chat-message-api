using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UseCases.Test
{
    public class TestRequest : IRequest<TestRequestResponse>
    {
        public string InValue { get; set; }
    }

    public class TestRequestResponse
    {
        public string OutValue { get; set; }
    }

    public class TestRequestHandler : IRequestHandler<TestRequest, TestRequestResponse>
    {
        public Task<TestRequestResponse> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            var getvalue = request.InValue;
            return Task.FromResult(new TestRequestResponse { OutValue = "response" });
        }
    }
}
