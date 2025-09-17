using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
  public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
  {
    private readonly IExpensesReadOnlyRepository _expensesRepository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetExpenseByIdUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper, ILoggedUser loggedUser)
    {
      _expensesRepository = expensesRepository;
      _mapper = mapper;
      _loggedUser = loggedUser;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
      var loggedUser = await _loggedUser.Get();

      var result = await _expensesRepository.GetById(loggedUser, id) ?? 
        throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

      return _mapper.Map<ResponseExpenseJson>(result);
    }
  }
}
