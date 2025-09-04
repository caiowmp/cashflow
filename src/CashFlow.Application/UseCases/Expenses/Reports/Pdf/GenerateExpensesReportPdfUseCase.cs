using System.Reflection;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Features;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
using PdfSharp.Events;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
  public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
  {
    private const string CURRENCY_SYMBOL = "$";
    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
      _repository = repository;

      GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
      var expenses = await _repository.FilterByMonth(month);

      if (expenses.Count == 0)
        return [];

      var document = CreateDocument(month);
      var page = CreatePage(document);

      CreateHeaderWithProfilePhotoAndName(page);

      var totalExpent = expenses.Sum(expenses => expenses.Amount);
      CreateTotalSpentSection(page, month, totalExpent);

      foreach (var expense in expenses)
      {
        var table = CreateExpensesTable(page);

        var row = table.AddRow();
        row.Height = 25;

        row.Cells[0].AddParagraph(expense.Title);
        row.Cells[0].Format.Font= new Font { Name = FontHelper.MONTSERRAT_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        row.Cells[0].Shading.Color = ColorsHelper.RED_LIGHT;
        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        row.Cells[0].MergeRight = 2;
        row.Cells[0].Format.LeftIndent = 20;

        row.Cells[3].AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        row.Cells[3].Format.Font = new Font { Name = FontHelper.MONTSERRAT_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        row.Cells[3].Shading.Color = ColorsHelper.RED_DARK;
        row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

        row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
      }

      return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
      var document = new Document();
      document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
      document.Info.Author = "Caio Miranda Pereira";

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

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
      var table = page.AddTable();
      table.AddColumn();
      table.AddColumn("300");

      var row = table.AddRow();

      var assembly = Assembly.GetExecutingAssembly();
      var directoryName = Path.GetDirectoryName(assembly.Location);
      var pathFile = Path.Combine(directoryName!, "Logo", "logo.png");
      
      row.Cells[0].AddImage(pathFile);

      row.Cells[1].AddParagraph("Hey, Caio Miranda Pereira");
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
