using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete
{
  public class DeleteExpenseUseCase : IDeleteExpenseUseCase
  {
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;

    public DeleteExpenseUseCase(
      IExpensesWriteOnlyRepository repository,
      IUnitOfWork unityOfWork,
      ILoggedUser loggedUser,
      IExpensesReadOnlyRepository expensesReadOnlyRepository)
    {                           
      _repository = repository;
      _unityOfWork = unityOfWork;
      _loggedUser = loggedUser;
      _expensesReadOnlyRepository = expensesReadOnlyRepository;
    }

    public async Task Execute(long id)
    {
      var loggedUser = await _loggedUser.Get();

      var expense = _expensesReadOnlyRepository.GetById(loggedUser, id)
        ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
      
      await _repository.Delete(id);

      await _unityOfWork.Commit();
    }
  }
}
