using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Op.Persistance.Identity;

namespace Op.Persistance.Tenant;



public sealed class InMemoryCompanyProvisioningQueue : ICompanyProvisioningQueue
{
    private readonly Channel<Company> _channel = Channel.CreateUnbounded<Company>();

    public Task EnqueueAsync(Company company, CancellationToken ct = default)
        => _channel.Writer.WriteAsync(company, ct).AsTask();

    public async IAsyncEnumerable<Company> DequeueAllAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        while (await _channel.Reader.WaitToReadAsync(ct))
        while (_channel.Reader.TryRead(out var c))
            yield return c;
    }
}