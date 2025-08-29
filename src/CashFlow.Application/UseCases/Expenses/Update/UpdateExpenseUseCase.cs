using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update
{
  public class UpdateExpenseUseCase : IUpdateExpenseUseCase
  {
    private readonly IExpensesUpdateOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;

    public UpdateExpenseUseCase(
      IExpensesUpdateOnlyRepository repository,
      IUnityOfWork unityOfWork,
      IMapper mapper)
    {
      _repository = repository;
      _unityOfWork = unityOfWork;
      _mapper = mapper;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
      Validate(request);

      var expense = await _repository.GetById(id);

      if (expense is null)
      {
        throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
      }

      _mapper.Map(request, expense);

      _repository.Equals(expense);

      await _unityOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request)
    {
      var validator = new ExpenseValidator();

      var result = validator.Validate(request);

      if (!result.IsValid)
      {
        var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
      }
    }
  }
}
