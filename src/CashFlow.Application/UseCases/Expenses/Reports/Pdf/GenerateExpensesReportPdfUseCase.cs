using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
  public class GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository) : IGenerateExpensesReportPdfUseCase
  {
    private const string CURRENCY_SYMBOL = "$";

    public async Task<byte[]> Execute(DateOnly month)
    {
      var expenses = await repository.FilterByMonth(month);

      if (expenses.Count == 0)
        return [];

      return [];
    }
  }
}
