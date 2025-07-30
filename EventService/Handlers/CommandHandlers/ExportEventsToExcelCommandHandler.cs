using EventService.Models;
using MediatR;
using ClosedXML.Excel;

public class ExportEventsToExcelCommandHandler : IRequestHandler<ExportEventsToExcelCommand, string>
{
    private readonly IEventRepository _repository;
    public ExportEventsToExcelCommandHandler(IEventRepository repository)
    {
        _repository = repository;
    }
    public async Task<string> Handle(ExportEventsToExcelCommand request, CancellationToken cancellationToken)
    {
        var events = await _repository.GetAllEventsAsync();
        var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Etkinlikler");

        ws.Cell(1, 1).Value = "Id";
        ws.Cell(1, 2).Value = "Title";
        ws.Cell(1, 3).Value = "Location";
        ws.Cell(1, 4).Value = "Start-Date";
        ws.Cell(1, 5).Value = "End-Date";
        ws.Cell(1, 6).Value = "Capacity";
        ws.Cell(1, 7).Value = "Age-Restriction";

        int row = 2;
        foreach (var item in events)
        {
            ws.Cell(row, 1).Value = item.Id.ToString();
            ws.Cell(row, 2).Value = item.Title;
            ws.Cell(row, 3).Value = item.Location;
            ws.Cell(row, 4).Value = item.StartDate.ToString("dd-MM-yyyy");
            ws.Cell(row, 5).Value = item.EndDate.ToString("dd-MM-yyyy");
            ws.Cell(row, 6).Value = item.Capacity;
            ws.Cell(row, 7).Value = item.AgeRestriction;
        }

        var fileName = $"etkinlikler_{DateTime.Now:ddmmyyyy}_{DateTime.Now:HHmmss}.xlsx";
        var filePath = Path.Combine("wwwroot", "reports", fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        wb.SaveAs(filePath);

        return filePath;
    }
}