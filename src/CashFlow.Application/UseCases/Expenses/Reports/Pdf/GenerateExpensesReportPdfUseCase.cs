using System.Reflection;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
  public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
  {
    private const string CURRENCY_SYMBOL = "$";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;

    private readonly IExpensesReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
    {
      _repository = repository;
      _loggedUser = loggedUser;

      GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
      var loggedUser = await _loggedUser.Get();

      var expenses = await _repository.FilterByMonth(loggedUser, month);

      if (expenses.Count == 0)
        return [];

      var document = CreateDocument(loggedUser.Name, month);
      var page = CreatePage(document);

      CreateHeaderWithProfilePhotoAndName(loggedUser.Name, page);

      var totalExpent = expenses.Sum(expenses => expenses.Amount);
      CreateTotalSpentSection(page, month, totalExpent);

      foreach (var expense in expenses)
      {
        var table = CreateExpensesTable(page);

        var row = table.AddRow();
        row.Height = HEIGHT_ROW_EXPENSE_TABLE;

        AddExpenseTitle(row.Cells[0], expense.Title);

        AddHeaderForAmount(row.Cells[3]);

        row = table.AddRow();
        row.Height = HEIGHT_ROW_EXPENSE_TABLE;

        row.Cells[0].AddParagraph(expense.Date.ToString("D"));
        SetStyleBaseForExpenseInformation(row.Cells[0]);
        row.Cells[0].Format.LeftIndent = 20;

        row.Cells[1].AddParagraph(expense.Date.ToString("t"));
        SetStyleBaseForExpenseInformation(row.Cells[1]);

        row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
        SetStyleBaseForExpenseInformation(row.Cells[2]);

        AddAmountForExpense(row.Cells[3], expense.Amount);

        if (!string.IsNullOrEmpty(expense.Description))
        {
          var description = table.AddRow();
          description.Height = HEIGHT_ROW_EXPENSE_TABLE;

          description.Cells[0].AddParagraph(expense.Description);
          description.Cells[0].Format.Font = new Font { Name = FontHelper.OPENSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
          description.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
          description.Cells[0].VerticalAlignment = VerticalAlignment.Center;
          description.Cells[0].MergeRight = 2;
          description.Cells[0].Format.LeftIndent = 20;

          row.Cells[3].MergeDown = 1;
        }

        AddWhiteSpace(table);
      }

      return RenderDocument(document);
    }

    private Document CreateDocument(string author, DateOnly month)
    {
      var document = new Document();
      document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
      document.Info.Author = author;

      var style = document.Styles["Normal"];
      style!.Font.Name = FontHelper.DEFAULT_FONT;


      return document;
    }

    private Section CreatePage(Document document)
    {
      var section = document.AddSection();
      section.PageSetup = document.DefaultPageSetup.Clone();

      section.PageSetup.PageFormat = PageFormat.A4;
      section.PageSetup.LeftMargin = 40;
      section.PageSetup.RightMargin = 40;
      section.PageSetup.TopMargin = 80;
      section.PageSetup.BottomMargin = 80;

      return section;
    }

    private byte[] RenderDocument(Document document)
    {
      var renderer = new PdfDocumentRenderer
      {
        Document = document,
      };

      renderer.RenderDocument();

      using var file = new MemoryStream();
      renderer.PdfDocument.Save(file);

      return file.ToArray();
    }

    private void CreateHeaderWithProfilePhotoAndName(string name, Section page)
    {
      var table = page.AddTable();
      table.AddColumn();
      table.AddColumn("300");

      var row = table.AddRow();

      var assembly = Assembly.GetExecutingAssembly();
      var directoryName = Path.GetDirectoryName(assembly.Location);
      var pathFile = Path.Combine(directoryName!, "Logo", "logo.png");

      row.Cells[0].AddImage(pathFile);

      row.Cells[1].AddParagraph($"Hey, {name}");
      row.Cells[1].Format.Font = new Font { Name = FontHelper.MONTSERRAT_BLACK, Size = 16 };
      row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private Table CreateExpensesTable(Section page)
    {
      var table = page.AddTable();

      table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
      table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
      table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
      table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

      return table;
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
      cell.AddParagraph(expenseTitle);
      cell.Format.Font = new Font { Name = FontHelper.MONTSERRAT_BLACK, Size = 14, Color = ColorsHelper.BLACK };
      cell.Shading.Color = ColorsHelper.RED_LIGHT;
      cell.VerticalAlignment = VerticalAlignment.Center;
      cell.MergeRight = 2;
      cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForAmount(Cell cell)
    {
      cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
      cell.Format.Font = new Font { Name = FontHelper.MONTSERRAT_BLACK, Size = 14, Color = ColorsHelper.WHITE };
      cell.Shading.Color = ColorsHelper.RED_DARK;
      cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
      cell.Format.Font = new Font { Name = FontHelper.OPENSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
      cell.Shading.Color = ColorsHelper.GREEN_DARK;
      cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
      cell.AddParagraph($"-{amount} {CURRENCY_SYMBOL}");
      cell.Format.Font = new Font { Name = FontHelper.OPENSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
      cell.Shading.Color = ColorsHelper.WHITE;
      cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
      var row = table.AddRow();
      row.Height = 30;
      row.Borders.Visible = false;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpent)
    {
      var paragraph = page.AddParagraph();
      paragraph.Format.SpaceBefore = "40";
      paragraph.Format.SpaceAfter = "40";

      var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

      paragraph.AddFormattedText(title, new Font { Name = FontHelper.MONTSERRAT_REGULAR, Size = 15 });

      paragraph.AddLineBreak();

      paragraph.AddFormattedText($"{totalExpent} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.OPENSANS_REGULAR, Size = 50 });
    }
  }
}
