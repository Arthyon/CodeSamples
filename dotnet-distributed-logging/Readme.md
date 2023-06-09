# Distributed logging

Demonstrates a way to set up logging across microservices using correlation id propagation.

This sample sets a correlation id using [AsyncLocal](https://learn.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1?view=net-7.0) to persist across async-calls.
The propagation consists of three parts:
- A Web Api middleware to set correlation id based on a header on the incoming request
- An enricher is configured on the Serilog logger to enrich each log message with the correlation id
- A delegating handler is set up to add the correlation id as a header on outgoing requests using `IHttpClientFactory`

# Running the sample

- `docker compose up`
- Send a request to Service1: `curl http://localhost:5001/test`
- Check in seq GUI (`http://localhost:5341`) to verify the correlation id has been propagated correctly, and can be filtered on to retrieve all log statements
- You can also add the header `correlationid` to the initial request to verify that the first service inherits the correlation id from an external caller
