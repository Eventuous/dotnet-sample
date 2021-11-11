using System.Diagnostics;
using Eventuous.Diagnostics;
using Serilog;

namespace Bookings.Payments.Infrastructure; 

public sealed class TestActivityListener : IDisposable{
    readonly ActivityListener _listener;

    public TestActivityListener() {
        _listener = new ActivityListener {
            ShouldListenTo = _ => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> _)
                => ActivitySamplingResult.AllData,
            ActivityStarted = activity => Log.Information(
                "Started {Activity} with {Id}, parent {ParentId}",
                activity.DisplayName,
                activity.Id,
                activity.ParentId
            ),
            ActivityStopped = activity
                => Log.Information("Stopped {Activity}", activity.DisplayName)
        };
    }

    public void Dispose() => _listener.Dispose();
}