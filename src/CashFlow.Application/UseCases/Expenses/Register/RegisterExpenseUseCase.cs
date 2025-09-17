using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
  public class RegisterExpenseUseCase : IRegisterExpenseUseCase
  {
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public RegisterExpenseUseCase(
      IExpensesWriteOnlyRepository repository, 
      IUnitOfWork unityOfWork,
      IMapper mapper,
      ILoggedUser loggedUser)
    {
      _repository = repository;
      _unityOfWork = unityOfWork;
      _mapper = mapper;
      _loggedUser = loggedUser;
    }

    public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request)
    {
      Validate(request);

      var loggedUser = await _loggedUser.Get(); 

      var expense = _mapper.Map<Expense>(request);
      expense.UserId = loggedUser.Id;

      await _repository.Add(expense);

      await _unityOfWork.Commit();

      return _mapper.Map<ResponseRegisteredExpenseJson>(expense);
    }

    private void Validate(RequestExpenseJson request)
    {
      var result = new ExpenseValidator().Validate(request);

      if (!result.IsValid)
      {
        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
      }
    }
  }
}
