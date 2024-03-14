using Overmoney.Api.Features;
using Overmoney.Api.Features.Payees.Models;

namespace Overmoney.Api.DataAccess.Payees;

public interface IPayeeRepository : IRepository
{
    Task<Payee?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Payee>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Payee> CreateAsync(Payee payee, CancellationToken cancellationToken);
    Task UpdateAsync(Payee payee, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

public sealed class PayeeRepository : IPayeeRepository
{
    private static readonly List<PayeeEntity> _connection = [new(1, 1, "Test")];

    public async Task<Payee> CreateAsync(Payee payee, CancellationToken cancellationToken)
    {
        var entity = new PayeeEntity(_connection.Max(x => x.Id) + 1, payee.UserId, payee.Name);
        _connection.Add(entity);
        return await Task.FromResult(new Payee(entity.Id, entity.UserId, entity.Name));
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

    public async Task<IEnumerable<Payee>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Where(x => x.UserId == userId).Select(x => new Payee(x.Id, x.UserId, x.Name)));
    }

    public async Task<Payee?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var entity = _connection.FirstOrDefault(x => x.Id == id);
        if (entity is null)
        {
            return null;
        }

        return await Task.FromResult(new Payee(entity.Id, entity.UserId, entity.Name));
    }

    public Task UpdateAsync(Payee payee, CancellationToken cancellationToken)
    {
        var old = _connection.FirstOrDefault(x => x.Id == payee.Id);
        _connection.Remove(old);
        _connection.Add(new PayeeEntity(old.Id, payee.UserId, payee.Name));
        return Task.CompletedTask;
    }
}
