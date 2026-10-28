namespace Mrx.AspNetCore.Services
{
    [Service]
    public class ExcelService
    {
        public async Task<DataSet?> ReadAsync(IFormFile file)
        {
            if (file is null) return null;
            IExcelDataReader reader;
            MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);
            if (Path.GetExtension(file.FileName) == ".xls")
                reader = ExcelReaderFactory.CreateReader(ms);
            else
                reader = ExcelReaderFactory.CreateOpenXmlReader(ms);

            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            reader.Close();
            return result;
        }
    }
}
