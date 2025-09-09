using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
  public class RegisterExpenseUseCase : IRegisterExpenseUseCase
  {
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public RegisterExpenseUseCase(
      IExpensesWriteOnlyRepository repository, 
      IUnitOfWork unityOfWork,
      IMapper mapper)
    {
      _repository = repository;
      _unityOfWork = unityOfWork;
      _mapper = mapper;
    }

    public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request)
    {
      Validate(request);

      var entity = _mapper.Map<Expense>(request);

      await _repository.Add(entity);

      await _unityOfWork.Commit();

      return _mapper.Map<ResponseRegisteredExpenseJson>(entity);
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
