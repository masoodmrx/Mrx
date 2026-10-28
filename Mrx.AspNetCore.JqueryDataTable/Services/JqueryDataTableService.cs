namespace Mrx.AspNetCore.JqueryDataTable.Services
{
    public class JqueryDataTableService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public JqueryDataTableService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public JqueryDataTablesResult<T> GetResult<T>(JqueryDataTablesParameters table, IEnumerable<T> items, int? totalSize = null)
        {
            totalSize = totalSize ?? items.Count();
            try
            {
                return new JqueryDataTablesResult<T>
                {
                    Draw = table.Draw,
                    Data = items,
                    RecordsFiltered = totalSize.Value,
                    RecordsTotal = totalSize.Value
                };
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        public JqueryDataTablesWithOtherDataResult<T> GetResult<T>(JqueryDataTablesParameters table, IEnumerable<T> items, int? totalSize, object otherData)
        {
            totalSize = totalSize ?? items.Count();
            try
            {
                return new JqueryDataTablesWithOtherDataResult<T>
                {
                    Draw = table.Draw,
                    Data = items,
                    RecordsFiltered = totalSize.Value,
                    RecordsTotal = totalSize.Value,
                    OtherData = otherData
                };
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }
    }
}
