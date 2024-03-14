using Overmoney.Api.Features;
using Overmoney.Api.Features.Payees.Models;

namespace Overmoney.Api.DataAccess.Payees;

public interface IPayeeRepository : IRepository
{
    Task<PayeeEntity?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<PayeeEntity>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<PayeeEntity> CreateAsync(CreatePayee payee, CancellationToken cancellationToken);
    Task UpdateAsync(UpdatePayee payee, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

public sealed class PayeeRepository : IPayeeRepository
{
    private static readonly List<PayeeEntity> _connection = [new(1, 1, "Test")];

    public async Task<PayeeEntity> CreateAsync(CreatePayee payee, CancellationToken cancellationToken)
    {
        _connection.Add(new(_connection.Max(x => x.Id) + 1, payee.UserId, payee.Name));
        return await Task.FromResult(_connection.Last());
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var payee = _connection.FirstOrDefault(x => x.Id == id);
        if (payee != null)
        {
            _connection.Remove(payee);
        }
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<PayeeEntity>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Where(x => x.UserId == userId));
    }

    public async Task<PayeeEntity?> GetAsync(int id, CancellationToken cancellationToken)
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
