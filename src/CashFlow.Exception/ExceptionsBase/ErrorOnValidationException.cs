namespace CashFlow.Exception.ExceptionsBase
{
  public class ErrorOnValidationException : CashFlowException
  {
    public required List<string> ErrorsMessage { get; set; } = new List<string>();
  }
}
