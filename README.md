GooglePagination helps you to reduce boilerplate and setup pagination logic similar to what you see in Google search results.

Based On : https://jasonwatmore.com/post/2015/10/30/aspnet-mvc-pagination-example-with-logic-like-google

## Basic usage
Call DataPagingAsync() On Your IQueryable

```
  IQueryable<Alarm> queryable = context.Alarms;
  
  var result = await queryable.DataPagingAsync(pageSize, currentPage);
  await result.DataList.TolistAsync();

```
The result is an instance of #PagingModel

```
 public class PagingModel<T>
    {
        public IQueryable<T> DataList { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }

```

## Longer example 
```
public async Task<PagingModel<T>> GetAlarmsAsync<T>(int pageSize = 15, int currentPage = 1, string filter = ""){
           
                if (currentPage < 1)
                    currentPage = 1;

                IQueryable<Alarm> alarms = GetAll().Include(a => a.Device).ThenInclude(a => a.Location);

                if (!string.IsNullOrEmpty(filter))
                {
                    alarms = alarms.Where(a => EF.Functions.Like(a.AlarmEvent.ToString(), $"%{filter}%") ||
                                            EF.Functions.Like(a.AlarmType.ToString(), $"%{filter}%") ||
                                            EF.Functions.Like(a.Device.Name, $"%{filter}%") ||
                                            EF.Functions.Like(a.Device.Alias, $"%{filter}%") ||
                                            EF.Functions.Like(a.Device.Location.Name, $"%{filter}%"));
                }

                var result = await alarms.DataPagingAsync(pageSize, currentPage);
                await result.DataList.Select(a => new AlarmViewModel
                {
                    Id = a.Id,
                    AlarmDate = a.AlarmDate,
                    AlarmEvent = a.AlarmEvent,
                    AlarmType = a.AlarmType,
                    DeviceName = a.Device.GetName(),
                    LocationName = a.Device.Location.Name,
                    TimeStamp = a.TimeStamp
                }).ToListAsync();
                
                return result;
}
```
