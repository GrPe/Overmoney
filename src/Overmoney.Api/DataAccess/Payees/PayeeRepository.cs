using Overmoney.Api.Features.Payees.Models;

namespace Overmoney.Api.Features.Payees;

public interface IPayeeRepository : IRepository
{
    Task<Payee?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Payee>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Payee> CreateAsync(CreatePayee payee, CancellationToken cancellationToken);
    Task UpdateAsync(UpdatePayee payee, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

public sealed class PayeeRepository : IPayeeRepository
{
    private static readonly List<Payee> _connection = [new(1, 1, "Test")];

    public async Task<Payee> CreateAsync(CreatePayee payee, CancellationToken cancellationToken)
    {
        _connection.Add(new(_connection.Max(x => x.Id) + 1, payee.UserId, payee.Name));
        return await Task.FromResult(_connection.Last());
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var payee = _connection.FirstOrDefault(x => x.Id == id);
        if(payee != null)
        {
            _connection.Remove(payee);
        }
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Payee>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Where(x => x.UserId == userId));
    }

    public async Task<Payee?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Id == id));
    }

    public Task UpdateAsync(UpdatePayee payee, CancellationToken cancellationToken)
    {
        var old = _connection.FirstOrDefault(x => x.Id == payee.Id);
        _connection.Add(new(payee.Id, payee.UserId, payee.Name));
        _connection.Remove(old);
        return Task.CompletedTask;
    }
}
