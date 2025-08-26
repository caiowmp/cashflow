using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Infrastructure.DataAccess;


namespace CashFlow.Application.UseCases.Expenses.Register
{
  public class RegisterExpenseUseCase
  {
    public ResponseRegisteredExpenseJson Execute(RequestRegisterExpenseJson request)
    {
      Validate(request);

      var entity = new Expense
      {
        Amount = request.Amount,
        Date = request.Date,
        Description = request.Description,
        Title = request.Title,
        PaymentType = (Domain.Enums.PaymentType)request.PaymentType,
      };

      return new ResponseRegisteredExpenseJson();
    }

    private void Validate(RequestRegisterExpenseJson request)
    {
      var result = new RegisterExpenseValidator().Validate(request);

      if (!result.IsValid)
      {
        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

        throw new ErrorOnValidationException { ErrorsMessage = errorMessages };
      }
    }
  }
}
