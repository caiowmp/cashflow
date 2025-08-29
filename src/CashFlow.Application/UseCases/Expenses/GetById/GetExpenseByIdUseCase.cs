using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
  public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
  {
    private readonly IExpensesRepository _expensesRepository;
    private readonly IMapper _mapper;

    public GetExpenseByIdUseCase(IExpensesRepository expensesRepository, IMapper mapper)
    {
      _expensesRepository = expensesRepository;
      _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
      var result = await _expensesRepository.GetById(id) ?? 
        throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

      return _mapper.Map<ResponseExpenseJson>(result);
    }
  }
}
