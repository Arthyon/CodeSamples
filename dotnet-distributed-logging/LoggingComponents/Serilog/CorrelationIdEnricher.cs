using Serilog.Core;
using Serilog.Events;

namespace LoggingComponents.Serilog;

public class CorrelationIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(new LogEventProperty(Constants.CorrelationIdHeader, new ScalarValue(Correlator.CorrelationId)));
    }
}