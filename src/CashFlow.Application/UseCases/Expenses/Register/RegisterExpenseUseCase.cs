using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
  public class RegisterExpenseUseCase
  {
    public ResponseRegisteredExpenseJson Execute(RequestRegisterExpenseJson request)
    {
      Validate(request);

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
